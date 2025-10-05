using Microsoft.AspNetCore.Components;
using MudBlazor;
using SplitMate.Client.Managers;

namespace SplitMate.Client.Pages
{
	public partial class ShoppingListForm : ComponentBase
	{
		[Inject] public IUserManager UserManager { get; private set; } = null!;
		[Inject] public IShoppingListManager ShoppingListManager { get; private set; } = null!;
		[Parameter] public int? ShoppingListId { get; set; }

		private MudForm addItemForm = new();

		private List<UserViewModel> users = [];
		private List<ItemViewModel> itemsOnList = [];

		private AddItemViewModel addItemModel = new();

		protected override async Task OnParametersSetAsync()
		{

			await LoadExistingShoppingListAsync(ShoppingListId!.Value);
			await LoadUsersAsync();
		}
		private async Task LoadExistingShoppingListAsync(int id)
		{
			itemsOnList = [];
			var response = await ShoppingListManager.Retrieve(id);
			if (response != null)
			{
				if (!response.IsSuccess)
					Snackbar.Add($"Nie udało sie pobrać listy. " + response.FailedResponseRaw, Severity.Error);
				else
				{
					foreach (var item in response.Response.ShoppingList.Items)
						itemsOnList.Add(new(item.Id, item.Name, item.Value, item.Type, item.DesiredBy != null ? new UserViewModel(item.DesiredBy.Id, item.DesiredBy.Name) : null));
				}
			}
			else
				Snackbar.Add($"Nie udało sie pobrać listy", Severity.Error);
		}
		private async Task LoadUsersAsync()
		{
			var response = await UserManager.RetrieveAll();
			if (response != null)
			{
				if (!response.IsSuccess)
					Snackbar.Add($"Nie udało sie pobrać użytkowników. " + response.FailedResponseRaw, Severity.Error);
				else
					users = [.. response.Response.Users.Select(x => new UserViewModel(x.Id, x.Name))];
			}
			else
				Snackbar.Add($"Nie udało sie pobrać użytkowników", Severity.Error);
		}

		private async Task HandleAddClickAsync()
		{
			await addItemForm.Validate();
			if (!addItemForm.IsValid || !ShoppingListId.HasValue)
				return;

			var response = await ShoppingListManager.AddItem(ShoppingListId.Value, new(
				Name: addItemModel.Name ?? string.Empty,
				Value: addItemModel.Value ?? decimal.Zero,
				Type: addItemModel.Type,
				DesiredById: addItemModel.DesiredByUserId));

			if (response != null)
			{
				if (!response.IsSuccess)
				{
					Snackbar.Add("Nie udało się dodać przedmiotu. " + response.FailedResponseRaw, Severity.Error);
					return;
				}
			}
			else
			{
				Snackbar.Add("Nie udało się dodać przedmiotu", Severity.Error);
				return;
			}


			await LoadExistingShoppingListAsync(ShoppingListId!.Value);
			Snackbar.Add("Przedmiot został dodany do listy.", Severity.Info);
			await addItemForm.ResetAsync();
		}
	}
}
