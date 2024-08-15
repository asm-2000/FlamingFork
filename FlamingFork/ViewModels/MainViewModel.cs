using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Pages;
using FlamingFork.Repositories.ApiServices;
using System.Diagnostics;

namespace FlamingFork.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private List<MenuItemModel> _MenuItems;

        [ObservableProperty]
        private List<MenuItemModel> _MomoItems;

        [ObservableProperty]
        private List<MenuItemModel> _NoodleItems;

        [ObservableProperty]
        private List<MenuItemModel> _SandwichItems;

        [ObservableProperty]
        private List<MenuItemModel> _SnackItems;

        [ObservableProperty]
        private List<MenuItemModel> _DrinkItems;

        [ObservableProperty]
        private List<MenuItemModel> _BreakfastItems;

        [ObservableProperty]
        private string _IsFetching;

        [ObservableProperty]
        private string _HasLoaded;

        private INavigation _Navigation;
        private MenuItemFetchServiceRepository _MenuItemFetchService;
        private int _CustomerId;

        #endregion Properties

        #region Constructor

        public MainViewModel(INavigation navigation)
        {
            _MenuItems = [];
            _MomoItems = [];
            _NoodleItems = [];
            _SandwichItems = [];
            _DrinkItems = [];
            _BreakfastItems = [];
            _SnackItems = [];
            _MenuItemFetchService = new MenuItemFetchServiceRepository();
            _Navigation = navigation;
            CheckLoginStatus();
            FetchMenuData();
        }

        #endregion Constructor

        #region Methods

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
            HasLoaded = "False";
            IsFetching = "True";
            while (MenuItems.Count == 0)
            {
                await Task.Delay(3000);
                MenuItems = await _MenuItemFetchService.GetMenuItems();
            }
            SegregateMenuItems();
            IsFetching = "False";
            HasLoaded = "True";
        }

        public void SegregateMenuItems()
        {
            foreach (MenuItemModel menuItem in MenuItems)
            {
                if (menuItem.ItemCategory == "Snacks")
                {
                    SnackItems.Add(menuItem);
                }
                else if (menuItem.ItemCategory == "Momo")
                {
                    MomoItems.Add(menuItem);
                }
                else if (menuItem.ItemCategory == "Noodles")
                {
                    NoodleItems.Add(menuItem);
                }
                else if (menuItem.ItemCategory == "Burgers And Sandwiches")
                {
                    SandwichItems.Add(menuItem);
                }
                else if (menuItem.ItemCategory == "Breakfast")
                {
                    BreakfastItems.Add(menuItem);
                }
                else
                {
                    DrinkItems.Add(menuItem);
                }
            }
        }

        [RelayCommand]
        public void IncreaseQuantity(string itemName)
        {
            foreach (MenuItemModel menuItem in MenuItems)
            {
                if (menuItem.ItemName == itemName)
                {
                    // Only increase the quantity if its value does not exceed 5.
                    if (menuItem.Quantity <= 4)
                    {
                        int quantity = menuItem.Quantity + 1;
                        menuItem.Quantity = quantity;
                    }
                }
            }
            ClearLists();
            SegregateMenuItems();
        }

        [RelayCommand]
        public void DecreaseQuantity(string itemName)
        {
            foreach (MenuItemModel menuItem in MenuItems)
            {
                if (menuItem.ItemName == itemName)
                {
                    // Only decrease quantity if its previous quantity is greater than 1.
                    if (menuItem.Quantity > 1)
                    {
                        menuItem.Quantity -= 1;
                    }
                }
            }
            ClearLists();
            SegregateMenuItems();
        }

        [RelayCommand]
        public async Task AddMenuItemsToCart(string name)
        {
           CustomerModel customer = await SecureStorageHandler.GetCustomerDetails();
           int customerId = Convert.ToInt16(customer.CustomerID);
           MenuItemModel? currentMenuItem = MenuItems.Find(item => item.ItemName == name);
           CartItemModel correspondingCartItem = new(customerId,currentMenuItem.ItemName, currentMenuItem.ItemPrice, currentMenuItem.Quantity);
        }

        public void ClearLists()
        {
            MomoItems.Clear();
            SnackItems.Clear();
            NoodleItems.Clear();
            DrinkItems.Clear();
            SandwichItems.Clear();
            BreakfastItems.Clear();
        }

        #endregion Methods
    }
}
