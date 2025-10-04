using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SplitMate.Client.Layout
{
	public partial class MainLayout : LayoutComponentBase
	{
		[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
		private bool _drawerOpen = true;
		private readonly MudTheme _currentTheme = new ApplicationTheme();
		private void DrawerToggle()
		{
			_drawerOpen = !_drawerOpen;
		}
	}
}
