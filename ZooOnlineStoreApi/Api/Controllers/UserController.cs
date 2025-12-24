using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;

        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequest request)
        {
            try
            {
                string apiKey = await userService.RegisterAsync(request);
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
                string apiKey = await userService.LoginAsync(request);
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
                UserResponse response = await userService.GetUserAsync(apiKey);
                // 200
                return Ok(response);
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
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                UserResponse response = await userService.GetByIdAsync(id);
                return Ok(response);
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
