using Microsoft.AspNetCore.Components;
using MudBlazor;
using SplitMate.Client.Managers;

namespace SplitMate.Client.Pages
{
	public partial class ShoppingLists : ComponentBase
	{
		[Inject] public IShoppingListManager ShoppingListManager { get; private set; } = null!;

		private MudTable<ShoppingListViewModel> MudTable = new();
		private string searchString = string.Empty;
		private IEnumerable<ShoppingListViewModel> shoppingLists = [];
		private bool isLoading = true;

		protected override async Task OnInitializedAsync()
		{
			try
			{
				isLoading = true;
				await LoadData();
				StateHasChanged();
			}
			finally
			{
				isLoading = false;
			}
		}
		private Task<TableData<ShoppingListViewModel>> Reload(TableState tableState, CancellationToken cancellationToken)
		{
			if (!string.IsNullOrWhiteSpace(searchString))
			{
				shoppingLists = shoppingLists.Where(item =>
					(item.Name?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
					(item.DoneByUserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false)
				);
			}

			shoppingLists = tableState.SortLabel switch
			{
				"name_field" => shoppingLists.OrderByDirection(tableState.SortDirection, o => o.Name),
				"sum_field" => shoppingLists.OrderByDirection(tableState.SortDirection, o => o.SumValue),
				"date_field" => shoppingLists.OrderByDirection(tableState.SortDirection, o => o.CreateDate),
				"user_field" => shoppingLists.OrderByDirection(tableState.SortDirection, o => o.DoneByUserName),
				"isSettled_field" => shoppingLists.OrderByDirection(tableState.SortDirection, o => o.IsSettled),
				_ => shoppingLists
			};
			var totalItems = shoppingLists.Count();

			var pagedData = shoppingLists.Skip(tableState.Page * tableState.PageSize).Take(tableState.PageSize).ToList();
			return Task.FromResult(new TableData<ShoppingListViewModel>() { TotalItems = totalItems, Items = pagedData });
		}
		private void OnSearch(string text)
		{
			searchString = text;
			MudTable.ReloadServerData();
		}

		private void ShowDetails(int id)
		{
			NavigationManager.NavigateTo($"/shopping-lists/details/{id}");
		}
		private async Task Delete(int id)
		{
			try
			{
				isLoading = true;
				await ShoppingListManager.Delete(id);
				await LoadData();
			}
			finally
			{
				isLoading = false;
			}
		}
		private async Task LoadData()
		{
			var response = await ShoppingListManager.RetrieveAll();
			if (response.IsSuccess && response.Response != null)
				shoppingLists = [.. response.Response.Shoppings
						.Select(x => new ShoppingListViewModel(
							Id: x.Id,
							Name: x.Name,
							SumValue: x.SumValue,
							CreateDate: x.CreateDate,
							DoneByUserName: x.DoneByUserName,
							IsSettled: x.IsSettled
							))];
		}
	}
}
