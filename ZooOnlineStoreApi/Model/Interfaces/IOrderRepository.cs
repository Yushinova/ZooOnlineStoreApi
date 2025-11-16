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
    }
}
