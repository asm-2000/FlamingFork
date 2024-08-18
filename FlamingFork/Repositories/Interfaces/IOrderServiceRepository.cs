using FlamingFork.Models;

namespace FlamingFork.Repositories.Interfaces
{
    interface IOrderServiceRepository
    {
        Task<List<CustomerOrderModel>> GetCustomerOrders();
        Task<string> CancelCustomerOrder(CustomerOrderModel customerOrder);
    }
}
