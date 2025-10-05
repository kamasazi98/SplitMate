using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Shared;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Commands
{
	internal class DeleteShoppingListCommandHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<DeleteShoppingListCommand, IResult>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult> Handle(DeleteShoppingListCommand request, CancellationToken cancellationToken)
		{
			var shoppingList = await applicationDbContext.ShoppingLists.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.ShoppingListId, cancellationToken);
			if (shoppingList == null)
				return this.Fail(ErrorCode.NOT_FOUND);

			applicationDbContext.ShoppingItems.RemoveRange(shoppingList.Items);
			applicationDbContext.ShoppingLists.Remove(shoppingList);
			await applicationDbContext.SaveChangesAsync(cancellationToken);
			return this.Success();
		}
	}
}
