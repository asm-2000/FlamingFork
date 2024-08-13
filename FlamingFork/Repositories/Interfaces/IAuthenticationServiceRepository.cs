using FlamingFork.Models;

namespace FlamingFork.Repositories.Interfaces
{
    public interface IAuthenticationServiceRepository
    {
        Task<string> RegisterCustomer(CustomerModel customerDetails);
        Task<string> LoginCustomer(CustomerLoginModel customerCredentials);
    }
}
