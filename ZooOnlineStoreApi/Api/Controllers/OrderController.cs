using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly ProductService productService;
        private readonly OrderItemService orderItemService;
        private readonly UserService userService;
        private readonly IMapper mapper;
        public OrderController(OrderService orderService,
                ProductService productService,
                OrderItemService orderItemService,
                IMapper mapper,
                UserService userService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.orderItemService = orderItemService;
            this.mapper = mapper;
            this.userService = userService;
        }

        //работа с заказами все авторизованные админы
        [HttpGet("admin")]//с пагинацией все заказы
        [Authorize]
        public async Task<ActionResult> GetOrdersSorted([FromQuery] int page, [FromQuery] int pageSize)
        {
            Console.WriteLine("page: " + page + "size: " + pageSize);
            List<Order>? ordersFromDb = await orderService.ListPaginationAsync(page, pageSize);

            return Ok(mapper.Map<List<OrderResponse>>(ordersFromDb));
        }

        [HttpPatch("admin/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] OrderUpdateRequest request)
        {
            try
            {
                Order? orderFromDb = await orderService.GetByIdAsync(id);
                orderFromDb.Amount -= orderFromDb.ShippingCost;
                orderFromDb.ShippingCost = request.ShippingCost;
                orderFromDb.Status = request.Status;
                orderFromDb.Amount += request.ShippingCost;
                await orderService.UndateAsync(orderFromDb);
                if (request.Status.ToLower().Contains("del"))
                {
                    List<OrderItem>? itemsByOrderId = await orderItemService.ListAllByOrderIdAsync(id);
                    if (itemsByOrderId != null && itemsByOrderId.Count > 0)
                    {
                        foreach (var item in itemsByOrderId)
                        {
                            await productService.AddQuantityByIdAsync(item.ProductId, item.Quantity);
                        }
                    }
                }
                return Ok(mapper.Map<OrderResponse>(orderFromDb));
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPost("user")]//добавление нового заказа юзером
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> AddNewOrderAsync([FromBody] OrderRequest request)
        {
            try
            {
                Order orderInsert = mapper.Map<Order>(request);
                orderInsert.OrderNumber = GenerateGuidBasedOrderNumber();
                orderInsert.CreatedAt = DateTime.UtcNow;
                orderInsert = await orderService.InsertAsync(orderInsert);
                if (orderInsert.OrderItems != null && orderInsert.OrderItems.Count > 0)
                {
                    foreach (var item in orderInsert.OrderItems)
                    {
                        item.OrderId = orderInsert.Id;
                        await productService.DeleteQuantityByIdAsync(item.ProductId, item.Quantity);//убираем количество товара
                    }
                }
                Order? orderFromDb = await orderService.UndateAsync(orderInsert);
                User? userFromDb = await userService.GetByIdAsync(orderInsert.UserId);
                if (userFromDb != null)
                {
                    userFromDb.TotalOrders += orderInsert.Amount;
                    await userService.UpdateAsync(userFromDb);
                }
                return Ok(mapper.Map<OrderResponse>(orderFromDb));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpGet("user/{userId:int}")]//получение всех заказов юзером
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> ListAllByUserId(int userId)
        {
            try
            {
                List<OrderResponse> orderResponse = mapper.Map<List<OrderResponse>>(await orderService.ListAllByUserIdAsync(userId));
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

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
