using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IOrderItemRepository: IRepository<OrderItem>
    {
        Task InsertRangeAsync(List<OrderItem> items);
        Task DeleteRangeAsync(List<OrderItem> items);
        Task<List<OrderItem>?> SelectAllByOrderIdAsync(int orderId);
    }
}
