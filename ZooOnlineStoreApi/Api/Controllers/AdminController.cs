using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService adminService;
        public AdminController(AdminService adminService)
        {
            this.adminService = adminService;
        }

        //регистрация авторизация
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] AdminRequest request)
        {

            await adminService.InsertAsync(request);
            string apiKey = await adminService.AuthenticateAsync(request.Login, request.Password);
            return Ok(apiKey);

        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {

            HttpContext.Response.Cookies.Delete("adminToken");
            return Ok(new { message = "Logged out successfully" });
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] AdminLoginRequest request)
        {
            string apiKey = await adminService.AuthenticateAsync(request.Login, request.Password);
            return Ok(apiKey);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInfoAsync([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            AdminResponse admin = await adminService.GetAdminAsync(apiKey);
            // 200
            return Ok(admin);
        }
        //мой служебный метод пока что
        [HttpPatch]
        public async Task<IActionResult> UpdateAsync([FromBody] AdminUpdateRequest request)
        {
            await adminService.UpdateAsync(request);
            AdminResponse response = await adminService.GetByLoginAsync(request.Login);
            return Ok(response);
        }
    }
}
