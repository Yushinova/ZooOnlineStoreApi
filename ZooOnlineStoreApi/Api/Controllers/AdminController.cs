using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Admins;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService adminService;
    
        private readonly IEncoder encoder;
        private readonly IMapper mapper;
        public AdminController(AdminService adminService, IEncoder encoder, IMapper mapper)
        {
            this.adminService = adminService;
            this.encoder = encoder;
            this.mapper = mapper;
        }
        //регистрация авторизация
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] AdminRequest request)
        {
            try
            {
                Admin admin = mapper.Map<Admin>(request);
                admin.Password = encoder.Encode(request.Password);
                admin.RegisteredAt = DateTime.UtcNow;
                await adminService.InsertAsync(admin);
                Admin? adminFromDb = await adminService.GetByLoginAsync(admin.Login);
                AdminResponse response = mapper.Map<AdminResponse>(adminFromDb);
                response.ApiKey = encoder.Encode(generateApiKey(adminFromDb));
                return Ok(response);
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
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] AdminLoginRequest request)
        {
            try
            {
                Admin adminFromDb = await adminService.AuthenticateAsync(request.Login, request.Password);
                AdminResponse response = mapper.Map<AdminResponse>(adminFromDb);
                response.ApiKey = encoder.Encode(generateApiKey(adminFromDb));
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
       

      //мой служебный метод пока что
        [HttpPatch]
        public async Task<IActionResult> UpdateAsync([FromBody] AdminUpdateRequest request)
        {
            try
            {
                Admin? adminFromDb = await adminService.GetByLoginAsync(request.Login);
                if (adminFromDb != null)
                {
                    adminFromDb.Name = request.Name;
                    adminFromDb.Role = request.Role;
                    await adminService.UpdateAsync(adminFromDb);
                }
                return Ok(mapper.Map<AdminResponse>(adminFromDb));

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
        // генерация api-ключа для admin
        private string generateApiKey(Admin admin)
        {
            return encoder.Encode($"{admin.Name} - {admin.Login} - {admin.RegisteredAt}");
        }
    }
}
