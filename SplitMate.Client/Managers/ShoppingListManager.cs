using SplitMate.Client.Extensions;
using SplitMate.Shared.Extensions;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Features.ShoppingList.Queries;
using System.Net.Http.Json;

namespace SplitMate.Client.Managers
{
	public class ShoppingListManager(IHttpClientFactory httpClientFactory) : IShoppingListManager
	{
		private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

		public Task<ApiResult<RetrieveAllShoppingListQuery.Response>> RetrieveAll()
			=> httpClientFactory.MainApiClient().GetAsync("/api/shoppingLists").ToApiResult<RetrieveAllShoppingListQuery.Response>();
		public Task<ApiResult<RetrieveAllNotSettledShoppingListQuery.Response>> RetrieveAllNotSettled()
			=> httpClientFactory.MainApiClient().GetAsync("/api/shoppingLists/NotSettled").ToApiResult<RetrieveAllNotSettledShoppingListQuery.Response>();
		public Task<ApiResult<RetrieveShoppingListQuery.Response>> RetrieveAll(int shoppingListId)
			=> httpClientFactory.MainApiClient().GetAsync($"/api/shoppingLists?shoppingListId={shoppingListId}").ToApiResult<RetrieveShoppingListQuery.Response>();

		public Task<ApiResult<int>> AddNew(string? name, int doneByUserId)
			=> httpClientFactory.MainApiClient().PostAsJsonAsync($"/api/shoppingLists", new AddNewShoppingListCommand(name, doneByUserId)).ToApiResult<int>();

		public Task<ApiResult> Import(int shoppingListId, List<ImportShoppingListItemsCommand.ItemAggregate> items)
			=> httpClientFactory.MainApiClient().PostAsJsonAsync($"/api/shoppingLists/{shoppingListId}/Import", new ImportShoppingListItemsCommand(items)).ToApiResult();

		public Task<ApiResult> AddItem(int shoppingListId, AddShoppingListItemCommand.ItemAggregate item)
			=> httpClientFactory.MainApiClient().PostAsJsonAsync($"/api/shoppingLists/{shoppingListId}/Add", new AddShoppingListItemCommand(item)).ToApiResult();

		public Task<ApiResult> ChangeItem(int shoppingListId, int shoppingListItemId, ChangeShoppingListItemCommand item)
			=> httpClientFactory.MainApiClient().PatchAsJsonAsync($"/api/shoppingLists/{shoppingListId}/Change/{shoppingListItemId}", item).ToApiResult();

		public Task<ApiResult> DeleteItem(int shoppingListId, int shoppingListItemId)
			=> httpClientFactory.MainApiClient().DeleteAsync($"/api/shoppingLists/{shoppingListId}/Item/{shoppingListItemId}").ToApiResult();
	}

	public interface IShoppingListManager : IManager
	{
		Task<ApiResult> AddItem(int shoppingListId, AddShoppingListItemCommand.ItemAggregate item);
		Task<ApiResult<int>> AddNew(string? name, int doneByUserId);
		Task<ApiResult> ChangeItem(int shoppingListId, int shoppingListItemId, ChangeShoppingListItemCommand item);
		Task<ApiResult> DeleteItem(int shoppingListId, int shoppingListItemId);
		Task<ApiResult> Import(int shoppingListId, List<ImportShoppingListItemsCommand.ItemAggregate> items);
		Task<ApiResult<RetrieveAllShoppingListQuery.Response>> RetrieveAll();
		Task<ApiResult<RetrieveShoppingListQuery.Response>> RetrieveAll(int shoppingListId);
		Task<ApiResult<RetrieveAllNotSettledShoppingListQuery.Response>> RetrieveAllNotSettled();
	}
}
