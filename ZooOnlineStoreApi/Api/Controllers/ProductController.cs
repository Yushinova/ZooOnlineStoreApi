using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;
        private readonly PetTypeService petTypeService;
        private readonly IMapper mapper;
        public ProductController(ProductService productService, PetTypeService petTypeService, IMapper mapper)
        {
            this.productService = productService;
            this.petTypeService = petTypeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithFilterAndPagination([FromQuery] ProductQueryParameters parameters)
        {
            List<Product>? productsFromDb = await productService.SuperPagination(parameters);
            return Ok(mapper.Map<List<ProductResponse>>(productsFromDb));
        }

        //[HttpGet]//test
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    List<Product> productFromDb = await productService.ListAllAsync();
        //    return Ok(mapper.Map<List<ProductResponse>>(productFromDb));
        //}

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithAllInfoAsync(int id)
        {
            Product? productFromDb = await productService.SelectByIdWithAllInfoAsync(id);
            return Ok(mapper.Map<ProductResponse>(productFromDb));

        }

        //работа с продуктами (только роль админ)
        [HttpPost("admin")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
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
              Product productFromDb = await productService.InsertAsync(productInsert);
                //нужновернуть с id!
                return Ok(mapper.Map<ProductResponse>(productFromDb));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpPatch("admin/{id:int}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
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
        [HttpDelete("admin/{id:int}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await productService.DeleteAsync(id);
                return Ok();
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
