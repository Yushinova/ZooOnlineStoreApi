using ZooOnlineStoreApi.Model.OrderItems;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IOrderItemRepository: IRepository<OrderItem>
    {
        Task InsertRangeAsync(List<OrderItem> items);
        Task DeleteRangeAsync(List<OrderItem> items);
        Task<List<OrderItem>?> SelectAllByOrderIdAsync(int orderId);
    }
}
