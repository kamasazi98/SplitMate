using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Shared.Features.ShoppingList.Queries;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.ShoppingLists.Queries
{
	internal class RetrieveAllNotSettledShoppingListQueryHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<RetrieveAllNotSettledShoppingListQuery, IResult<RetrieveAllNotSettledShoppingListQuery.Response>>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult<RetrieveAllNotSettledShoppingListQuery.Response>> Handle(RetrieveAllNotSettledShoppingListQuery request, CancellationToken cancellationToken)
		{
			var shoppingLists = await applicationDbContext.ShoppingLists.AsNoTracking().Include(x => x.User).Where(x => !x.IsSettled).ToListAsync(cancellationToken);
			var mapped = shoppingLists.Select(x => new RetrieveAllNotSettledShoppingListQuery.Response.ShoppingListItem(
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
