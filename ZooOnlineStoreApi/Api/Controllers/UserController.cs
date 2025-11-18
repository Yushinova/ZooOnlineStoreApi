using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly OrderService orderService;
        private readonly ProductService productService;
        private readonly IMapper mapper;
        private readonly IEncoder encoder;
        public UserController(UserService userService,OrderService orderService, IMapper mapper, IEncoder encoder, ProductService productService)
        {
            this.userService = userService;
            this.orderService = orderService;
            this.mapper = mapper;
            this.encoder = encoder;
            this.productService = productService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequest request)
        {
            try
            {
                User user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Password = encoder.Encode(request.Password),//хэшируем
                    UUID = Guid.NewGuid(),
                    RegisteredAt = DateTime.UtcNow,
                    Discont = 0,
                    TotalOrders = 0,
                };
                User? userFromDb = await userService.RegisterAsync(user);
                UserAuthResponse response = mapper.Map<UserAuthResponse>(userFromDb);
                response.Token = encoder.Encode(generateApiKey(user));
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch(DuplicationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }
            catch(Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }

        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] UserLoginRequest request)
        {
            try
            {
                User userFromDb = await userService.AuthenticateAsync(request.Phone, request.Password);
                UserAuthResponse response = mapper.Map<UserAuthResponse>(userFromDb);
                response.Token = encoder.Encode(generateApiKey(userFromDb));
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch(UnauthorizedAccessException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListAllAsync()
        {
            List<User> usersFromDb = await userService.ListAllAsync();
            return Ok(mapper.Map<List<UserResponse>>(usersFromDb));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsynk(int id)
        {
            try
            {
                User? userFromDb = await userService.GetByIdAsync(id);
                return Ok(mapper.Map<UserResponse>(userFromDb));
            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await userService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        ///Ограничение доступа юзера
        [HttpPost("order")]//добавление нового заказа юзером
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

        [HttpGet("orders/{userId:int}")]//получение всех заказов юзером
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
        // генерация api-ключа для пользователя
        private string generateApiKey(User user)
        {
            return encoder.Encode($"{user.UUID} - {user.Phone} - {user.Email} - {user.RegisteredAt}");
        }
    }
}
