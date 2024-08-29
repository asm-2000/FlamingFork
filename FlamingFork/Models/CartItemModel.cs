namespace FlamingFork.Models
{
    public class CartItemModel
    {
        public int? CartItemId { get; set; }
        public int CustomerId { get; set; }
        public string? CartItemName { get; set; }
        public int CartItemPrice { get; set; }
        public int Quantity { get; set; }
        public string? CartItemImageUrl { get; set; }

        public CartItemModel(int customerId, string cartItemName, int cartItemPrice, int quantity, string cartItemImageUrl) 
        {
            CartItemId = null;
            CustomerId = customerId;
            CartItemName = cartItemName;
            CartItemPrice = cartItemPrice;
            Quantity = quantity;
            CartItemImageUrl = cartItemImageUrl;
        }

    }
}
