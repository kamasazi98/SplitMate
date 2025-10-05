using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Shared.Features.ShoppingList.Queries;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Queries
{
	internal class RetrieveAllShoppingListQueryHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<RetrieveAllShoppingListQuery, IResult<RetrieveAllShoppingListQuery.Response>>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult<RetrieveAllShoppingListQuery.Response>> Handle(RetrieveAllShoppingListQuery request, CancellationToken cancellationToken)
		{
			var shoppingLists = await applicationDbContext.ShoppingLists.AsNoTracking().Include(x => x.User).ToListAsync(cancellationToken);
			var mapped = shoppingLists.Select(x => new RetrieveAllShoppingListQuery.Response.ShoppingListItem(
				Id: x.Id,
				Name: x.Name,
				SumValue: x.SumValue,
				CreateDate: x.CreateDate,
				DoneByUserName: x.User.Name,
				IsSettled: x.IsSettled)).ToList();

			return this.Success(new(mapped));
		}
	}
}
