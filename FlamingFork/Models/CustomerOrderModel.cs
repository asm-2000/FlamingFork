namespace FlamingFork.Models
{
    class CustomerOrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerContact {  get; set; }
        public string? CustomerAddress { get; set; }
        public string? OrderStatus { get; set; }

        public CustomerOrderModel(int customerId, string customerContact, string customerAddress, string orderStatus)
        {
            CustomerId = customerId;
            CustomerContact = customerContact;
            CustomerAddress = customerAddress;
            OrderStatus = orderStatus;
        }
    }
}
