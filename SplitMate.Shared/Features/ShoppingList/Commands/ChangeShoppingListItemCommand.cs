using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Commands
{
	public record ChangeShoppingListItemCommand(string Name, decimal Value, ShoppingItemType Type, ChangeShoppingListItemCommand.User? DesiredBy) : IRequest<IResult>
	{
		public int ShoppingListId { get; init; }
		public int Id { get; init; }
		public record User(int Id, string Name);
	}
}
