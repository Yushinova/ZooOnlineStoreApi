using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Admins;
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

        private readonly IMapper mapper;
        public UserController(UserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;

        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequest request)
        {
            try
            {
                User user = mapper.Map<User>(request);
                string apiKey = await userService.RegisterAsync(user);
                return Ok(apiKey);
            }
            catch (ValidationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch (DuplicationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }

        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
               HttpContext.Response.Cookies.Delete("userToken");

               return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] UserLoginRequest request)
        {
            try
            {
                string apiKey = await userService.LoginAsync(request.Phone, request.Password);
                return Ok(apiKey);
            }
            catch (ValidationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }

        //[HttpGet]//test
        //[Authorize]
        //public async Task<IActionResult> ListAllAsync()
        //{
        //    List<User> usersFromDb = await userService.ListAllAsync();
        //    return Ok(mapper.Map<List<UserResponse>>(usersFromDb));
        //}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInfoAsync([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            try
            {
                User userFromDb = await userService.GetUserAsync(apiKey);
                // 200
                return Ok(mapper.Map<UserResponse>(userFromDb));
            }
            catch (NotFoundException ex)
            {
                // 404
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> GetByIdAsynk(int id)
        {
            try
            {
                User? userFromDb = await userService.GetByIdAsync(id);
                return Ok(mapper.Map<UserResponse>(userFromDb));
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await userService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

    }
}
