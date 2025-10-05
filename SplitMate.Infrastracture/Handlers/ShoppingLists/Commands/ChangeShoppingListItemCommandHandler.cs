using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Domain.Entities;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Infrastracture.Stores;
using SplitMate.Shared;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Commands
{
	internal class ChangeShoppingListItemCommandHandler(IShoppingListStore shoppingListStore, ApplicationDbContext applicationDbContext) : IRequestHandler<ChangeShoppingListItemCommand, IResult>
	{
		private readonly IShoppingListStore shoppingListStore = shoppingListStore;
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult> Handle(ChangeShoppingListItemCommand request, CancellationToken cancellationToken)
		{
			User? user = null;
			if (request.Type == ShoppingItemType.OnePerson)
				user = await applicationDbContext.KnownUsers.FirstOrDefaultAsync(x => x.Id == request.DesiredBy!.Id, cancellationToken);

			var spec = await shoppingListStore.Retrieve(request.ShoppingListId, cancellationToken);
			spec.ChangeItem(new(request.Id, request.Value, request.Name, request.Type, user));

			await applicationDbContext.SaveChangesAsync(cancellationToken);
			return this.Success();
		}
	}
}
