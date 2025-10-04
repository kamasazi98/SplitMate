using Microsoft.AspNetCore.Components;
using MudBlazor;
using SplitMate.Client.Managers;


namespace SplitMate.Client.Pages
{
	public partial class UsersList : ComponentBase
	{
		[Inject] protected ISnackbar Snackbar { get; private set; } = null!;
		[Inject] protected IDialogService DialogService { get; private set; } = null!;
		[Inject] protected IUserManager UserManager { get; private set; } = null!;
		private bool isLoading = true;
		private List<UserListViewModel> users = [];
		private MudTable<UserListViewModel>? table;
		private string? filterContent;
		private UserListViewModel? newUser;

		protected override async Task OnParametersSetAsync()
		{
			await LoadData();
		}
		private async Task LoadData()
		{
			try
			{
				isLoading = true;

				var response = await UserManager.RetrieveAll();
				if (response.IsSuccess && response.Response != null)
					users = [.. response.Response.Users
						.Select(x => new UserListViewModel()
						{
							Id = x.Id,
							Name = x.Name,
						})];
			}
			finally
			{
				isLoading = false;
			}
		}
		private bool Filter(UserListViewModel model)
		{
			if (string.IsNullOrWhiteSpace(filterContent))
				return true;

			if (!model.Id.HasValue)
				return true;

			return model.Name.Contains(filterContent, StringComparison.InvariantCultureIgnoreCase);
		}
		private async void SaveNewUser(object element)
		{
			if (element is UserListViewModel model)
			{
				var result = await UserManager.CreateUser(model.Name);
				if (result.IsSuccess)
				{
					Snackbar.Add("Pomyślnie dodano użytkownika", Severity.Success);
					newUser!.Id = result.Response!;
					newUser = null;
				}
				else
				{
					Snackbar.Add("Wystąpił błąd podczas dodawania użytkownika", Severity.Error);
					await Task.Delay(10);
					table!.SetEditingItem(newUser);
				}
			}
			StateHasChanged();
		}

		private async Task AddNewUser()
		{
			if (newUser == null)
			{
				newUser = new UserListViewModel();
				users.Insert(0, newUser);
				await Task.Delay(10);
				table!.SetEditingItem(newUser);
			}
		}
		private void CancelAddingUser(object element)
		{
			if (element is UserListViewModel model)
				users.Remove(model);

			newUser = null;
			StateHasChanged();
		}


	}
}
