using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.ShoppingList.Queries
{
	public record RetrieveShoppingListQuery(int ShoppingListId) : IRequest<IResult<RetrieveShoppingListQuery.Response>>
	{
		public record Response(Response.ShoppingListAggregate ShoppingList)
		{
			public record ShoppingListAggregate(int Id, string? Name, decimal SumValue, DateTime CreateDate, string DoneByUserName, bool IsSettled, IReadOnlyList<ShoppingListItem> Items)
			{
				public IReadOnlyList<ShoppingListItem> Items { get; init; } = Items ?? [];
			}
			public record ShoppingListItem(int Id, string Name, decimal Value, ShoppingItemType Type, User? DesiredBy);
			public record User(int Id, string Name);
		}
	}
}
