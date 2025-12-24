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
            try
            {
                await adminService.InsertAsync(request);
                string apiKey =  await adminService.AuthenticateAsync(request.Login, request.Password);
                return Ok(apiKey);
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
                HttpContext.Response.Cookies.Delete("adminToken");

                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] AdminLoginRequest request)
        {
            try
            {
                string apiKey = await adminService.AuthenticateAsync(request.Login, request.Password); 
                return Ok(apiKey);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInfoAsync([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            try
            {
                AdminResponse admin = await adminService.GetAdminAsync(apiKey);
                // 200
                return Ok(admin);
            }
            catch (NotFoundException ex)
            {
                // 404
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        //мой служебный метод пока что
        [HttpPatch]
        public async Task<IActionResult> UpdateAsync([FromBody] AdminUpdateRequest request)
        {
            try
            {
                await adminService.UpdateAsync(request);
                AdminResponse response = await adminService.GetByLoginAsync(request.Login);
                return Ok(response);

            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
            catch(Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
    }
}
