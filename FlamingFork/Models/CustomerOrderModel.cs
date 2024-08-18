namespace FlamingFork.Models
{
    public class CustomerOrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerContact {  get; set; }
        public string? CustomerAddress { get; set; }
        public string? OrderStatus { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public string? StringifiedItems {  get; set; } 
        public int TotalPrice { get; set; }

        public CustomerOrderModel(int customerId, string customerContact, string customerAddress, string orderStatus, List<OrderItemModel> orderItems)
        {
            CustomerId = customerId;
            CustomerContact = customerContact;
            CustomerAddress = customerAddress;
            OrderStatus = orderStatus;
            OrderItems = orderItems;
        }
    }
}
