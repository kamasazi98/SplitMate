using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Commands
{
	public record AddShoppingListItemCommand(AddShoppingListItemCommand.ItemAggregate Item) : IRequest<IResult>
	{
		public int ShoppingListId { get; init; }
		public record ItemAggregate(string Name, decimal Value, ShoppingItemType Type, User? DesiredBy);
		public record User(int Id, string Name);
	}
}
