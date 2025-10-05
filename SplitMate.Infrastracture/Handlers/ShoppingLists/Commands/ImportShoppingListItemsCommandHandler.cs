using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Domain.Entities;
using SplitMate.Domain.Specifications;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Infrastracture.Stores;
using SplitMate.Shared;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Commands
{
	internal class ImportShoppingListItemsCommandHandler(IShoppingListStore shoppingListStore, ApplicationDbContext applicationDbContext) : IRequestHandler<ImportShoppingListItemsCommand, IResult>
	{
		private readonly IShoppingListStore shoppingListStore = shoppingListStore;
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult> Handle(ImportShoppingListItemsCommand request, CancellationToken cancellationToken)
		{
			var spec = await shoppingListStore.Retrieve(request.ShoppingListId, cancellationToken);
			foreach (var item in request.Items)
				await AddItem(spec, item, cancellationToken);

			await applicationDbContext.SaveChangesAsync(cancellationToken);
			return this.Success();
		}
		private async Task AddItem(ShoppingListSpecification specification, ImportShoppingListItemsCommand.ItemAggregate itemAggregate, CancellationToken cancellationToken)
		{
			User? user = null;
			if (itemAggregate.Type == ShoppingItemType.OnePerson)
				user = await applicationDbContext.KnownUsers.FirstOrDefaultAsync(x => x.Id == itemAggregate.DesiredBy!.Id, cancellationToken);

			specification.AddItem(new(itemAggregate.Value, itemAggregate.Name, itemAggregate.Type, user));
		}
	}
}
