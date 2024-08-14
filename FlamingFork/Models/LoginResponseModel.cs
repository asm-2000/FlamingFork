namespace FlamingFork.Models
{
    public class LoginResponseModel
    {
        public string? Token {  get; set; }
        public CustomerModel? CustomerDetails { get; set; }
    }
}
