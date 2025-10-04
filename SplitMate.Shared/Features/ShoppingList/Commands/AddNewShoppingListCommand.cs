using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Commands
{
	public record AddNewShoppingListCommand(string? Name, int DoneByUserId) : IRequest<IResult<int>>;
}
