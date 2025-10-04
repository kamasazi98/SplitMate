using MediatR;
using Microsoft.AspNetCore.Mvc;
using SplitMate.Extensions;
using SplitMate.Shared.Features.Users.Commands;
using SplitMate.Shared.Features.Users.Queries;

namespace SplitMate.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator mediator = mediator;

		[HttpGet]
		public Task<IActionResult> GetUsers()
		{
			return this.ResolveResult(
				resultTask: mediator.Send(new RetrieveAllUsersQuery()),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}
		[HttpPost]
		public Task<IActionResult> AddUser([FromBody] AddUserCommand command)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(command),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}
	}
}
