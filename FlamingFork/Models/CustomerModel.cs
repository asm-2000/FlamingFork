namespace FlamingFork.Models
{
    public class CustomerModel
    {
        public int? CustomerID   { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }

        public CustomerModel(string customerName, string email, string password, string address, string contact)
        {
            CustomerID = null;
            CustomerName = customerName;
            Email = email;
            Password = password;
            Address = address;
            Contact = contact;
        }

        public CustomerModel(int? customerId, string customerName, string address, string contact)
        {
            CustomerID = customerId;
            CustomerName = customerName;
            Address = address;
            Contact = contact;
        }

        public CustomerModel() { }
    }
}
