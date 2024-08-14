namespace FlamingFork.Models
{
    public class CustomerLoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public CustomerLoginModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
