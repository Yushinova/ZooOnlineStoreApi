using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Services
{
    public class OrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        public async Task InsertRangeAsync(List<OrderItem> items)
        {
            await _orderItemRepository.InsertRangeAsync(items);
        }
        public async Task InsertAsync(OrderItem item)
        {
            await _orderItemRepository.InsertAsync(item);
        }
        public async Task<List<OrderItem>?> ListAllByOrderIdAsync(int orderId)
        {
            return await _orderItemRepository.SelectAllByOrderIdAsync(orderId);
        }
        public async Task DeleteRangeAsync(List<OrderItem> items)
        {
            await _orderItemRepository.DeleteRangeAsync(items);
        }
    }
}
