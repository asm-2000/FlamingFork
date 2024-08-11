namespace FlamingFork.Repositories.Interfaces
{
    public interface IAuthenticationServiceRepository
    {
        Task RegisterCustomer();
        Task LoginCustomer();
    }
}
