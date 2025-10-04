using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Commands
{
	public record DeleteShoppingListItemCommand(int ShoppingListId, int ShoppingListItemId) : IRequest<IResult>;
}
