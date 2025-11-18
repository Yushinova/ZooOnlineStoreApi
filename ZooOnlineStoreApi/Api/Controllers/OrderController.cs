using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly OrderItemService orderItemService;
        private readonly UserService userService;
        private readonly ProductService productService;
        private readonly IMapper mapper;
        public OrderController(OrderService orderService,
            OrderItemService orderItemService,
            UserService userService,
            ProductService productService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.orderItemService = orderItemService;
            this.userService = userService;
            this.productService = productService;
            this.mapper = mapper;
        }
        [HttpPost]//добавление нового заказа юзером
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
                return Ok(mapper.Map<OrderResponse>(orderFromDb));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }
        [HttpGet("{userId:int}")]//получение всех заказов юзером
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
        [HttpGet("status/{status}")]
        public async Task<IActionResult> ListAllByStatus(string status)
        {
            try
            {
                List<OrderResponse> orderResponse = mapper.Map<List<OrderResponse>>(await orderService.ListAllByStatusAsync(status));
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }
        [HttpGet("admin/sorted")]//с пагинацией все заказы
        public async Task<ActionResult> GetOrdersSorted([FromQuery] int page, [FromQuery] int pageSize)
        {
            List<Order>? ordersFromDb = await orderService.ListPaginationAsync(page, pageSize);

            return Ok(mapper.Map<List<OrderResponse>>(ordersFromDb));
        }
        //нужны фильтры для фильтрации заказов 
        public static string GenerateGuidBasedOrderNumber()
        {
            var guid = Guid.NewGuid().ToString("N"); // без дефисов
            var shortGuid = guid.Substring(0, 8).ToUpper();
            var timestamp = DateTime.UtcNow.ToString("yyMMdd");

            return $"ORD-{timestamp}-{shortGuid}";
        }
    }
}
