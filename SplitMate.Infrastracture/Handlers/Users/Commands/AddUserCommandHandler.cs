using MediatR;
using SplitMate.Infrastracture.Data;
using SplitMate.Infrastracture.Extensions;
using SplitMate.Shared.Features.Users.Commands;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Handlers.Users.Commands
{
	internal class AddUserCommandHandler(ApplicationDbContext applicationDbContext) : IRequestHandler<AddUserCommand, IResult<int>>
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

		public async Task<IResult<int>> Handle(AddUserCommand request, CancellationToken cancellationToken)
		{
			var user = applicationDbContext.KnownUsers.FirstOrDefault(x => x.Name == request.Name);
			if (user != null)
				return this.Fail(409, "User already exists");

			user = new()
			{
				Name = request.Name,
			};

			await applicationDbContext.KnownUsers.AddAsync(user, cancellationToken);
			await applicationDbContext.SaveChangesAsync(cancellationToken);
			return this.Success(user.Id);
		}
	}
}
