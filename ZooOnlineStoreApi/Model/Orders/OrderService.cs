using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Storage;

namespace ZooOnlineStoreApi.Model.Orders
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            Order? orderFromDb = await _orderRepository.GetByIdAsync(id);
            if (orderFromDb == null)
            {
                throw new NotFoundException();
            }
            return orderFromDb;
        }

        public async Task<Order> InsertAsync(Order entity)
        {
            return await _orderRepository.InsertReturnEntityAsync(entity);
        }
        public async Task<Order> UndateAsync(Order entity)
        {
            Order? orderFromDb = await _orderRepository.GetByIdAsync(entity.Id);
            if (orderFromDb == null)
            {
                throw new NotFoundException();
            }
            orderFromDb.ShippingCost = entity.ShippingCost;
            orderFromDb.Status = entity.Status;
            orderFromDb.Amount = entity.Amount;
            return await _orderRepository.UpdateReturnEntityAsync(orderFromDb);
        }
        public async Task<List<Order>> ListAllAsync()
        {
            return await _orderRepository.SelectAllAsync();
        }

        public async Task<List<Order>?> ListAllByDataAsync(DateTime date)
        {
            return await _orderRepository.SelectAllByDataAsync(date);
        }

        public async Task<List<Order>?> ListAllByStatusAsync(string status)
        {
            return await _orderRepository.SelectAllByStatusAsync(status);
        }

        public async Task<List<Order>?> ListAllByUserIdAsync(int userId)
        {
            return await _orderRepository.SelectAllByUserIdAsync(userId);
        }


    }
}
