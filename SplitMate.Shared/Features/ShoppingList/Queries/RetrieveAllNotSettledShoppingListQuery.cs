using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Queries
{
	public record RetrieveAllNotSettledShoppingListQuery() : IRequest<IResult<RetrieveAllNotSettledShoppingListQuery.Response>>
	{
		public record Response(IReadOnlyList<Response.ShoppingListItem> Shoppings)
		{
			public IReadOnlyList<ShoppingListItem> Shoppings { get; init; } = Shoppings ?? [];
			public record ShoppingListItem(int Id, string? Name, decimal SumValue, DateTime CreateDate, string DoneByUserName, bool IsSettled);
		}
	}
}
