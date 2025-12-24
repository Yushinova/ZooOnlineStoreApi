using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

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
        public OrderController(OrderService orderService,
                ProductService productService,
                OrderItemService orderItemService,
                UserService userService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.orderItemService = orderItemService;
            this.userService = userService;
        }

        //работа с заказами все авторизованные админы
        [HttpGet("admin")]//с пагинацией все заказы
        [Authorize]
        public async Task<ActionResult> GetOrdersSorted([FromQuery] int page, [FromQuery] int pageSize)
        {
            Console.WriteLine("page: " + page + "size: " + pageSize);
            List<OrderResponse>? response = await orderService.ListPaginationAsync(page, pageSize);

            return Ok(response);
        }

        [HttpPatch("admin/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] OrderUpdateRequest request)
        {
            try
            {
                 OrderResponse response = await orderService.UndateAsync(id, request);
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
                return Ok(response);
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
                 OrderResponse orderInsert = await orderService.InsertAsync(request);
                if (request.OrderItems != null && request.OrderItems.Count > 0)
                {
                    foreach (var item in orderInsert.OrderItems)
                    {
                        item.OrderId = orderInsert.Id;
                        await productService.DeleteQuantityByIdAsync(item.ProductId, item.Quantity);//убираем количество товара
                    }
                }
                UserResponse user = await userService.GetByIdAsync(request.UserId);
                if (user != null)
                {
                    user.TotalOrders += orderInsert.Amount;
                    await userService.UpdateAsync(user);
                }
                return Ok(orderInsert);
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
                List<OrderResponse> orderResponse = await orderService.ListAllByUserIdAsync(userId);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }
       

    }
}
