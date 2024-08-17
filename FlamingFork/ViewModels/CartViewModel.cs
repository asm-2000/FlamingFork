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

        [ObservableProperty]
        private string _CartIsEmpty;

        [ObservableProperty]
        private string _CheckoutStatus;

        [ObservableProperty]
        private string _CheckoutStatusVisible;

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
            if (TotalItems > 0)
            {
                HasFetched = "True";
                CartIsEmpty = "False";
            }
            else
            {
                CartIsEmpty = "True";
            }
        }

        [RelayCommand]
        public async Task DeleteCartItem(string cartItemName)
        {
            // Find specific cart item by name from the list, CartItems.
            CartItemModel specificCartItem = CartItems.Find(cartItem => cartItem.CartItemName == cartItemName);
            await _CartService.DeleteSpecificCartItem(specificCartItem);
            await FetchCartItems();
        }

        [RelayCommand]
        public async Task ClearAllCartItems()
        {
            await _CartService.ClearCart();
            await FetchCartItems();
        }

        [RelayCommand]
        public async Task CheckoutFromCart()
        {
            CheckoutStatus = await _CartService.CheckoutAndPlaceOrder(CartItems);
            // Show sucess label if order is placed sucessfully and clear cart.
            if(CheckoutStatus == "Order Placed Sucessfully!")
            {
                CheckoutStatusVisible = "True";
                await _CartService.ClearCart();
                await FetchCartItems();
                CheckoutStatusVisible = "False";
            }   
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
