using CommunityToolkit.Mvvm.ComponentModel;
using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Pages;
using FlamingFork.Repositories.ApiServices;

namespace FlamingFork.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<CartItemModel> _MenuItems;

        private INavigation _Navigation;
        private MenuItemFetchServiceRepository _MenuItemFetchService;

        public MainViewModel(INavigation navigation)
        {
            _MenuItems = [];
            _MenuItemFetchService = new MenuItemFetchServiceRepository();
            _Navigation = navigation;
            CheckLoginStatus();
        }

        // Checks if the user has previously logged in or not and navigates accordingly
        public async void CheckLoginStatus()
        {
            string? token = await SecureStorageHandler.GetAuthenticationToken();
            Action navigationAction = token == "Not Found" ? (() => { _Navigation.PushAsync(new UserLoginPage()); }) : (() => { FetchMenuData(); });
            navigationAction();
        }

        public async void FetchMenuData()
        {
            MenuItems = await _MenuItemFetchService.GetMenuItems();
        }
    }
}
