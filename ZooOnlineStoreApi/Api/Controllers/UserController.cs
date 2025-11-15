using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly IMapper mapper;
        public UserController(UserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] UserRequest request)
        {
            try
            {
                User user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Password = request.Password,
                    UUID = Guid.NewGuid(),
                    RegisteredAt = DateTime.UtcNow,
                    Discont = 0,
                    TotalOrders = 0,
                };
                string apikey = await userService.InsertAsync(user);
                return Ok(apikey);
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

        }
        [HttpGet]
        public async Task<IActionResult> ListAllAsync()
        {
            List<User> usersFromDb = await userService.ListAllAsync();
            return Ok(mapper.Map<List<UserResponse>>(usersFromDb));
        }
    }
}
