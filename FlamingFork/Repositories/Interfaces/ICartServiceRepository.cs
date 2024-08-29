using FlamingFork.Models;

namespace FlamingFork.Repositories.Interfaces
{
    public interface ICartServiceRepository
    {
        Task<string> AddItemToCart(CartItemModel cartItem);

        Task<List<CartItemModel>> GetAllCartItems();

        Task<string> ClearCart();

        Task<string> CheckoutAndPlaceOrder(List<CartItemModel> cartItems, string customerCurrentAddress);
        
    }
}
