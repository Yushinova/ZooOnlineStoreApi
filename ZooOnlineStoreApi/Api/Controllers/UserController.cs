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
            string apiKey = await userService.RegisterAsync(request);
            return Ok(apiKey);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("userToken");
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] UserLoginRequest request)
        {
            string apiKey = await userService.LoginAsync(request);
            return Ok(apiKey);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInfoAsync([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            UserResponse response = await userService.GetUserAsync(apiKey);
            // 200
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            UserResponse response = await userService.GetByIdAsync(id);
            return Ok(response);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await userService.DeleteByIdAsync(id);
            return NoContent();
        }

    }
}
