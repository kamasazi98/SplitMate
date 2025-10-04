using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.Users.Queries
{
    public record RetrieveAllUsersQuery() : IRequest<IResult<RetrieveAllUsersQuery.Response>>
    {
        public record Response(IReadOnlyList<Response.User> Users)
        {
            public IReadOnlyList<User> Users { get; init; } = Users ?? [];

            public record User(int Id, string Name);
        }
    }
}