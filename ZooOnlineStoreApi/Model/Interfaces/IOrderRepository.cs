using ZooOnlineStoreApi.Model.Orders;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<List<Order>?> SelectAllByStatusAsync(string status);
        Task<List<Order>?> SelectAllByUserIdAsync(int userId);
        Task<List<Order>?> SelectAllByDataAsync(DateTime date);
        Task<Order> InsertReturnEntityAsync(Order entity);
        Task<Order> UpdateReturnEntityAsync(Order entity);
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<List<Order>?> GetAllWithPagination(int pageNumber, int countItems);

    }
}
