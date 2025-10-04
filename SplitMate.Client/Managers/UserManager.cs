using SplitMate.Client.Extensions;
using SplitMate.Shared.Extensions;
using SplitMate.Shared.Features.Users.Commands;
using SplitMate.Shared.Features.Users.Queries;
using System.Net.Http.Json;

namespace SplitMate.Client.Managers
{
	public class UserManager(IHttpClientFactory httpClientFactory) : IUserManager
	{
		private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

		public Task<ApiResult<RetrieveAllUsersQuery.Response>> RetrieveAll()
			=> httpClientFactory.MainApiClient().GetAsync("/api/users").ToApiResult<RetrieveAllUsersQuery.Response>();
		public Task<ApiResult<int>> CreateUser(string name)
			=> httpClientFactory.MainApiClient().PostAsJsonAsync("api/users", new AddUserCommand(name)).ToApiResult<int>();
	}
	public interface IUserManager : IManager
	{
		Task<ApiResult<int>> CreateUser(string name);
		Task<ApiResult<RetrieveAllUsersQuery.Response>> RetrieveAll();
	}
}
