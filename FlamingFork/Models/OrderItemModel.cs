namespace FlamingFork.Models
{
    public class OrderItemModel
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string? OrderItemName { get; set; }
        public int OrderItemPrice { get; set; }
        public int Quantity { get; set; }

        public OrderItemModel(string orderItemName, int orderItemPrice, int quantity)
        {
            OrderItemName = orderItemName;
            OrderItemPrice = orderItemPrice;
            Quantity = quantity;
        }
    }
}
