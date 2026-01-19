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
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("admin")]
        [Authorize]
        public async Task<ActionResult> GetOrdersSorted([FromQuery] int page, [FromQuery] int pageSize)
        {
            Console.WriteLine("page: " + page + "size: " + pageSize);
            List<OrderResponse>? response = await _orderService.ListPaginationAsync(page, pageSize);
            return Ok(response);
        }

        [HttpPatch("admin/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] OrderUpdateRequest request)
        {
            OrderResponse response = await _orderService.UndateAsync(id, request);
            return Ok(response);
        }

        [HttpPost("user")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> AddNewOrderAsync([FromBody] OrderRequest request)
        {
            OrderResponse orderInsert = await _orderService.InsertAsync(request);
            return Ok(orderInsert);
        }

        [HttpGet("user/{userId:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> ListAllByUserId(int userId)
        {
            List<OrderResponse> orderResponse = await _orderService.ListAllByUserIdAsync(userId);
            return Ok(orderResponse);
        }
    }
}
