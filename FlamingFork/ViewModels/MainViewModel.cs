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
        private List<MenuItemModel> _MenuItems;

        [ObservableProperty]
        private string _IsFetching;

        private INavigation _Navigation;
        private MenuItemFetchServiceRepository _MenuItemFetchService;
        private int _CustomerId;

        public MainViewModel(INavigation navigation)
        {
            _MenuItems = [];
            _MenuItemFetchService = new MenuItemFetchServiceRepository();
            _Navigation = navigation;
            CheckLoginStatus();
            FetchMenuData();
        }

        // Checks if the user has previously logged in or not and navigates accordingly
        public async void CheckLoginStatus()
        {
            string? token = await SecureStorageHandler.GetAuthenticationToken();
            Action navigationAction = token == "Not Found" ? (() => { _Navigation.PushAsync(new UserLoginPage()); }) : (async () =>
            {                   
                // Fetch customerId from secure storage.
                CustomerModel customerDetails = await SecureStorageHandler.GetCustomerDetails();
                _CustomerId = Convert.ToInt16(customerDetails.CustomerID);
            });
            navigationAction();
        }

        public async void FetchMenuData()
        {
            IsFetching = "True";
            while (MenuItems.Count == 0)
            {
                await Task.Delay(3000);
                MenuItems = await _MenuItemFetchService.GetMenuItems();
            }
            IsFetching = "False";
        }
    }
}