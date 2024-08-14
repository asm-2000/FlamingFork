using FlamingFork.Models;

namespace FlamingFork.Repositories.Interfaces
{
    public interface IMenuItemFetchServiceRepository
    {
        Task<List<CartItemModel>> GetMenuItems();
    }
}
