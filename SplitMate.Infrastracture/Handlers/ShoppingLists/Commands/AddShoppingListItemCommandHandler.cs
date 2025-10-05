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
	internal class AddShoppingListItemCommandHandler(IShoppingListStore shoppingListStore, ApplicationDbContext applicationDbContext) : IRequestHandler<AddShoppingListItemCommand, IResult>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult> Handle(AddShoppingListItemCommand request, CancellationToken cancellationToken)
		{
			User? user = null;
			if (request.Item.Type == ShoppingItemType.OnePerson)
				user = await applicationDbContext.KnownUsers.FirstOrDefaultAsync(x => x.Id == request.Item.DesiredById, cancellationToken);

			var spec = await shoppingListStore.Retrieve(request.ShoppingListId, cancellationToken);

			var addedItem = spec.AddItem(new(request.Item.Value, request.Item.Name, request.Item.Type, user));
			await applicationDbContext.ShoppingItems.AddAsync(addedItem, cancellationToken);
			await applicationDbContext.SaveChangesAsync(cancellationToken);

			return this.Success();
		}
	}
}
