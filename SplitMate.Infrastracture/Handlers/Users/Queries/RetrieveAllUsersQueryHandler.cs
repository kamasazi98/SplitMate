using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Shared.Features.Users.Queries;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.Users.Queries
{
	internal class RetrieveAllUsersQueryHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<RetrieveAllUsersQuery, IResult<RetrieveAllUsersQuery.Response>>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult<RetrieveAllUsersQuery.Response>> Handle(RetrieveAllUsersQuery request, CancellationToken cancellationToken)
		{
			var users = await applicationDbContext.KnownUsers.ToListAsync(cancellationToken);
			var mapped = users.Select(x => new RetrieveAllUsersQuery.Response.User(x.Id, x.Name)).ToList();

			return this.Success(new(mapped));
		}
	}
}
