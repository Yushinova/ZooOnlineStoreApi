using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<List<Order>?> SelectAllByUserIdAsync(int userId);
        Task<Order?> InsertReturnEntityAsync(Order entity);
        Task<Order> UpdateReturnEntityAsync(Order entity);
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<List<Order>?> GetAllWithPagination(int pageNumber, int countItems);

    }
}
