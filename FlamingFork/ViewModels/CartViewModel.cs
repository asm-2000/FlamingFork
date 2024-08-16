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

        private CartServiceRepository _CartService;

        #endregion Propeties

        #region Constructor

        public CartViewModel()
        {
            _CartService = new CartServiceRepository();
            FetchCartItems();
        }

        #endregion Constructor

        #region Methods

        [RelayCommand]
        public async Task FetchCartItems()
        {
            CartItems = await _CartService.GetAllCartItems();
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

        #endregion Methods
    }
}
