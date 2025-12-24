using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Storage;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
     
        public async Task<OrderResponse> InsertAsync(OrderRequest request)
        {
            Order orderInsert = _mapper.Map<Order>(request);
            orderInsert.OrderNumber = GenerateGuidBasedOrderNumber();
            orderInsert.CreatedAt = DateTime.UtcNow;
            Order orderFromDb = await _orderRepository.InsertReturnEntityAsync(orderInsert);

            return _mapper.Map<OrderResponse>(orderFromDb);
        }
        public async Task<OrderResponse> UndateAsync(int id, OrderUpdateRequest request)
        {
            Order? orderFromDb = await _orderRepository.GetByIdAsync(id);
            if (orderFromDb == null)
            {
                throw new NotFoundException();
            }
            orderFromDb.Amount -= orderFromDb.ShippingCost;
            orderFromDb.ShippingCost = request.ShippingCost;
            orderFromDb.Status = request.Status;
            orderFromDb.Amount += request.ShippingCost;
            Order orderUpdated = await _orderRepository.UpdateReturnEntityAsync(orderFromDb);
            return _mapper.Map<OrderResponse>(orderUpdated);
        }
        public async Task<List<Order>> ListAllAsync()
        {
            return await _orderRepository.SelectAllAsync();
        }

        public async Task<List<OrderResponse>> ListAllByUserIdAsync(int userId)
        {
            List<Order>? ordersFromDb = await _orderRepository.SelectAllByUserIdAsync(userId);
            return _mapper.Map<List<OrderResponse>>(ordersFromDb);
        }
        public async Task<List<OrderResponse>?> ListPaginationAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            int skip = (page - 1) * pageSize;
            List<Order>? orders = await _orderRepository.GetAllWithPagination(skip, pageSize);
            return _mapper.Map<List<OrderResponse>>(orders);
        }
        //генерация номера заказа
        public static string GenerateGuidBasedOrderNumber()
        {
            var guid = Guid.NewGuid().ToString("N"); // без дефисов
            var shortGuid = guid.Substring(0, 8).ToUpper();
            var timestamp = DateTime.UtcNow.ToString("yyMMdd");

            return $"ORD-{timestamp}-{shortGuid}";
        }
    }
}
