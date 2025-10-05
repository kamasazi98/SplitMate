using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Shared;
using SplitMate.Shared.Features.ShoppingList.Queries;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Queries
{
	internal class RetrieveShoppingListQueryHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<RetrieveShoppingListQuery, IResult<RetrieveShoppingListQuery.Response>>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult<RetrieveShoppingListQuery.Response>> Handle(RetrieveShoppingListQuery request, CancellationToken cancellationToken)
		{
			var shoppingList = await applicationDbContext.ShoppingLists
				.Include(x => x.User)
				.Include(x => x.Items)
				.FirstOrDefaultAsync(x => x.Id == request.ShoppingListId, cancellationToken);
			if (shoppingList == null)
				return this.Fail(ErrorCode.NOT_FOUND, $"ShoppingList not found by id {request.ShoppingListId}");

			var mapped = new RetrieveShoppingListQuery.Response.ShoppingListAggregate(
				Id: shoppingList.Id,
				Name: shoppingList.Name,
				SumValue: shoppingList.SumValue,
				CreateDate: shoppingList.CreateDate,
				DoneByUserName: shoppingList.User.Name,
				IsSettled: shoppingList.IsSettled,
				Items: [.. shoppingList.Items.Select(x => new RetrieveShoppingListQuery.Response.ShoppingListItem(
					Id: x.Id,
					Name: x.Name,
					Value: x.Value,
					Type: x.Type,
					DesiredBy: x.User != null ? new RetrieveShoppingListQuery.Response.User(x.User.Id, x.User.Name) : null))]
				);

			return this.Success(new(mapped));
		}
	}
}
