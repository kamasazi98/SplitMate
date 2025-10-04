using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Shared.Features.Users.Commands
{
	public record AddUserCommand(string Name) : IRequest<IResult<int>>;
}