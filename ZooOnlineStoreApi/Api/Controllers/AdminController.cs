using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
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
        private readonly ProductService productService;
        private readonly OrderService orderService;
        private readonly PetTypeService petTypeService;
        private readonly IEncoder encoder;
        private readonly IMapper mapper;
        public AdminController(AdminService adminService,
                            ProductService productService,
                            OrderService orderService,
                            PetTypeService petTypeService,
                            IEncoder encoder,
                            IMapper mapper)
        {
            this.adminService = adminService;
            this.productService = productService;
            this.orderService = orderService;
            this.petTypeService = petTypeService;
            this.encoder = encoder;
            this.mapper = mapper;
        }
        [HttpPost("product")]
        public async Task<ActionResult> InsertProductAsync([FromBody] ProductRequest request)
        {
            try
            {
                Product productInsert = mapper.Map<Product>(request);
                if (request.PetTypeIds != null && request.PetTypeIds.Any())
                {
                    List<PetType> petTypesFromDb = await petTypeService.ListAllAsync();
                    productInsert.PetTypes ??= new HashSet<PetType>();
                    foreach (var item in petTypesFromDb)
                    {
                        if (request.PetTypeIds.Contains(item.Id))
                        {
                            productInsert.PetTypes.Add(item);
                        }
                    }
                }
                await productService.InsertAsync(productInsert);
                return Ok(mapper.Map<ProductResponse>(productInsert));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }
        [HttpPatch("product/{id:int}")]
        public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] ProductRequest request)
        {
            try
            {

                Product petTypeUpdate = mapper.Map<Product>(request);
                if (request.PetTypeIds != null && request.PetTypeIds.Any())
                {
                    List<PetType> petTypesFromDb = await petTypeService.ListAllAsync();
                    petTypeUpdate.PetTypes = new HashSet<PetType>();
                    foreach (var item in petTypesFromDb)
                    {
                        if (request.PetTypeIds.Contains(item.Id))
                        {
                            petTypeUpdate.PetTypes.Add(item);
                        }
                    }
                }
                petTypeUpdate.Id = id;
                await productService.UpdateAsync(petTypeUpdate);
                Product? productFromDb = await productService.SelectByIdWithAllInfoAsync(id);
                return Ok(mapper.Map<ProductResponse>(productFromDb));

            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        [HttpGet("order")]//с пагинацией все заказы
        public async Task<ActionResult> GetOrdersSorted([FromQuery] int page, [FromQuery] int pageSize)
        {
            List<Order>? ordersFromDb = await orderService.ListPaginationAsync(page, pageSize);

            return Ok(mapper.Map<List<OrderResponse>>(ordersFromDb));
        }
        [HttpPatch("order/{id:int}")]
        public async Task<IActionResult> UpdateByIdAsync([FromBody] OrderUpdateRequest request, int id)
        {
            try
            {
                Order? orderFromDb = await orderService.GetByIdAsync(id);
                if (orderFromDb != null)
                {
                    orderFromDb.ShippingCost = request.ShippingCost;
                    orderFromDb.ShippingAddress = request.ShippingAddress;
                    orderFromDb.Status = request.Status;
                    await orderService.UndateAsync(orderFromDb);
                }
                return Ok(mapper.Map<OrderResponse>(orderFromDb));
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
                response.Token = encoder.Encode(generateApiKey(admin));
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
                response.Token = encoder.Encode(generateApiKey(adminFromDb));
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }
      
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
            return encoder.Encode($"{admin.Login} - {admin.Password}- {admin.RegisteredAt}");
        }
    }
}
