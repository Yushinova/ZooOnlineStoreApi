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
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IOrderItemRepository orderItemRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> InsertAsync(OrderRequest request)
        {
            Order orderInsert = _mapper.Map<Order>(request);
            orderInsert.OrderNumber = GenerateGuidBasedOrderNumber();
            orderInsert.CreatedAt = DateTime.UtcNow;
            Order? orderFromDb = await _orderRepository.InsertReturnEntityAsync(orderInsert);

            if (request.OrderItems != null && request.OrderItems.Count > 0 && orderFromDb != null)
            {
                foreach (var item in orderFromDb.OrderItems)
                {
                    item.OrderId = orderFromDb.Id;
                    await _productRepository.DeleteQuantityByIdAsync(item.ProductId, item.Quantity);
                }
            }

            User? user = await _userRepository.GetByIdAsync(request.UserId);
            if (user != null)
            {
                user.TotalOrders += orderFromDb.Amount;
                await _userRepository.UpdateAsync(user);
            }

            return _mapper.Map<OrderResponse>(orderFromDb);
        }

        public async Task<OrderResponse> UndateAsync(int id, OrderUpdateRequest request)
        {
            Order? orderFromDb = await _orderRepository.GetByIdAsync(id);
            if (orderFromDb == null)
            {
                throw new NotFoundException("order not found");
            }
            orderFromDb.ShippingAddress = request.ShippingAddress;
            orderFromDb.ShippingCost = request.ShippingCost;
            orderFromDb.Status = request.Status;
            Order orderUpdated = await _orderRepository.UpdateReturnEntityAsync(orderFromDb);

            if (request.Status.ToLower().Contains("del"))
            {
                List<OrderItem>? itemsByOrderId = await _orderItemRepository.SelectAllByOrderIdAsync(id);
                if (itemsByOrderId != null && itemsByOrderId.Count > 0)
                {
                    foreach (var item in itemsByOrderId)
                    {
                        await _productRepository.AddQuantityByIdAsync(item.ProductId, item.Quantity);
                    }
                }
                //TODO: уменьшить количество заказо юзера
                User? user = await _userRepository.GetByIdAsync(orderFromDb.UserId);
                if (user != null)
                {
                    user.TotalOrders -= orderFromDb.Amount;
                    await _userRepository.UpdateAsync(user);
                }
            }

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

        public static string GenerateGuidBasedOrderNumber()
        {
            var guid = Guid.NewGuid().ToString("N");
            var shortGuid = guid.Substring(0, 8).ToUpper();
            var timestamp = DateTime.UtcNow.ToString("yyMMdd");
            return $"ORD-{timestamp}-{shortGuid}";
        }
    }
}
