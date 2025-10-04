using MediatR;
using SplitMate.Shared.Wrappers;
using static SplitMate.Shared.Features.ShoppingList.Commands.ImportShoppingListItemsCommand;

namespace SplitMate.Shared.Features.ShoppingList.Commands
{
	public record ImportShoppingListItemsCommand(IReadOnlyList<ItemAggregate> Items) : IRequest<IResult>
	{
		public int ShoppingListId { get; init; }
		public IReadOnlyList<ItemAggregate> Items { get; init; } = Items ?? [];
		public record ItemAggregate(string Name, decimal Value, ShoppingItemType Type, User? DesiredBy);
		public record User(int Id, string Name);
	}
}
