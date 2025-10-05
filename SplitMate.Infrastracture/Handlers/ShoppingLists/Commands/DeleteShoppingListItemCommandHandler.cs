using MediatR;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Infrastracture.Stores;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Commands
{
	internal class DeleteShoppingListItemCommandHandler(IShoppingListStore shoppingListStore, ApplicationDbContext applicationDbContext) : IRequestHandler<DeleteShoppingListItemCommand, IResult>
	{
		private readonly IShoppingListStore shoppingListStore = shoppingListStore;
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult> Handle(DeleteShoppingListItemCommand request, CancellationToken cancellationToken)
		{
			var spec = await shoppingListStore.Retrieve(request.ShoppingListId, cancellationToken);
			spec.DeleteItem(new(request.ShoppingListItemId));

			await applicationDbContext.SaveChangesAsync(cancellationToken);
			return this.Success();
		}
	}
}
