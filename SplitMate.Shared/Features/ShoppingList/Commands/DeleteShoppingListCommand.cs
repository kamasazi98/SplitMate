using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Commands
{
	public record DeleteShoppingListCommand(int ShoppingListId) : IRequest<IResult>;
}
