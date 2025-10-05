using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Domain;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Infrastracture.Stores;
using SplitMate.Shared;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Commands
{
	internal class AddNewShoppingListCommandHandler(IShoppingListStore shoppingListStore, ApplicationDbContext applicationDbContext) : IRequestHandler<AddNewShoppingListCommand, IResult<int>>
	{
		private readonly IShoppingListStore shoppingListStore = shoppingListStore;
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult<int>> Handle(AddNewShoppingListCommand request, CancellationToken cancellationToken)
		{
			var user = await applicationDbContext.KnownUsers.FirstOrDefaultAsync(x => x.Id == request.DoneByUserId, cancellationToken)
				?? throw new ProblemException(ErrorCode.NOT_FOUND, $"User not found [{request.DoneByUserId}]");
			var spec = shoppingListStore.CreateNew();
			spec.CreateNew(new(request.Name, user));

			await applicationDbContext.ShoppingLists.AddAsync(spec.Entity, cancellationToken);
			await applicationDbContext.SaveChangesAsync(cancellationToken);

			return this.Success(spec.Entity.Id);
		}
	}
}
