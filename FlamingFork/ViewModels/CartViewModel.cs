using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Models;
using FlamingFork.Repositories.ApiServices;
using System.Collections.ObjectModel;

namespace FlamingFork.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        #region Propeties

        [ObservableProperty]
        private List<CartItemModel> _CartItems;

        [ObservableProperty]
        private int _TotalItems;

        [ObservableProperty]
        private int _TotalPrice;

        [ObservableProperty]
        private string _IsFetching;

        [ObservableProperty]
        private string _HasFetched;

        private CartServiceRepository _CartService;

        #endregion Propeties

        #region Constructor

        public CartViewModel()
        {
            _HasFetched = "False";
            _IsFetching = "True";
            _TotalItems = 0;
            _CartService = new CartServiceRepository();
            FetchCartItems();
        }

        #endregion Constructor

        #region Methods

        [RelayCommand]
        public async Task FetchCartItems()
        {
            IsFetching = "True";
            HasFetched = "False";
            CartItems = await _CartService.GetAllCartItems();
            TotalItems = CartItems.Count;
            TotalPrice = CalculateTotalPrice();
            IsFetching = "False";
            HasFetched = "True";
        }

        [RelayCommand]
        public async Task DeleteCartItem()
        {
        }

        [RelayCommand]
        public async Task ClearALlCartItems()
        {
            await _CartService.ClearCart();
            CartItems.Clear();
        }

        public int CalculateTotalPrice()
        {
            int totalPrice = 0;
            foreach (CartItemModel cartItem in CartItems)
            {
                totalPrice += cartItem.Quantity * cartItem.CartItemPrice;
            }
            return totalPrice;
        }

        #endregion Methods
    }
}
