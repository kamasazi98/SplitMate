using Microsoft.AspNetCore.Components;
using MudBlazor;
using SplitMate.Client.Managers;

namespace SplitMate.Client.Pages
{
	public partial class ShoppingListNewForm : ComponentBase
	{
		[Inject] public IUserManager UserManager { get; private set; } = null!;
		[Inject] public IShoppingListManager ShoppingListManager { get; private set; } = null!;

		private MudForm createForm = new();
		private List<UserViewModel> users = [];
		private CreateListViewModel createModel = new();
		private bool isLoading;

		protected override async Task OnParametersSetAsync()
		{
			try
			{
				isLoading = true;
				await LoadUsersAsync();
			}
			finally
			{
				isLoading = false;
			}
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
		private async Task HandleCreateListAsync()
		{
			await createForm.Validate();
			if (!createForm.IsValid)
				return;

			var response = await ShoppingListManager.AddNew(createModel.Name, createModel.DoneByUserId!.Value);
			if (response == null || !response.IsSuccess)
			{
				Snackbar.Add($"Nie udało się dodać listy. " + response?.FailedResponseRaw, Severity.Error);
				return;
			}

			var createdListId = response.Response;
			Snackbar.Add("Lista została pomyślnie utworzona!", Severity.Success);
			NavigationManager.NavigateTo($"/shopping-lists/edit/{createdListId}");
		}
	}
}
