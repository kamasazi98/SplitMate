using Microsoft.AspNetCore.Components;
using MudBlazor;
using SplitMate.Client.Managers;

namespace SplitMate.Client.Pages
{
	public partial class ShoppingListDetails : ComponentBase
	{
		[Inject] public IUserManager UserManager { get; private set; } = null!;
		[Inject] public IShoppingListManager ShoppingListManager { get; private set; } = null!;
		[Parameter] public int ShoppingListId { get; set; }
		private bool _isLoading = true;
		private ShoppingListDetailVieModel? _shoppingList;

		protected override async Task OnParametersSetAsync()
		{
			await LoadShoppingListDetails();
		}
		private async Task LoadShoppingListDetails()
		{
			try
			{
				_isLoading = true;

				var response = await ShoppingListManager.Retrieve(ShoppingListId);
				if (response != null)
				{
					if (!response.IsSuccess)
						Snackbar.Add($"Nie udało sie pobrać listy. " + response.FailedResponseRaw, Severity.Error);
					else
					{
						_shoppingList = new(
							Id: response.Response.ShoppingList.Id,
							Name: response.Response.ShoppingList.Name,
							SumValue: response.Response.ShoppingList.SumValue,
							CreateDate: response.Response.ShoppingList.CreateDate,
							DoneByUserName: response.Response.ShoppingList.DoneByUserName,
							DoneByUserId: response.Response.ShoppingList.DoneByUserId,
							IsSettled: response.Response.ShoppingList.IsSettled,
							Items: [.. response.Response.ShoppingList.Items.Select(x => new ShoppingListItemViewModel(
								Id: x.Id,
								Name: x.Name,
								Value: x.Value,
								Type: x.Type,
								DesiredBy: x.DesiredBy != null ? new UserViewModel(x.DesiredBy.Id, x.DesiredBy.Name) : null))]);
					}
				}
				else
					Snackbar.Add($"Nie udało sie pobrać listy", Severity.Error);
			}
			finally
			{
				_isLoading = false;
			}
		}
		private void GoToEdit()
		{
			NavigationManager.NavigateTo($"/shopping-lists/edit/{ShoppingListId}");
		}

		private void GoBackToList()
		{
			NavigationManager.NavigateTo("/shopping-lists");
		}
	}
}
