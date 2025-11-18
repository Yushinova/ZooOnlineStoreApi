using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
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
        private readonly IMapper mapper;
        public ProductController(ProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Product> productFromDb = await productService.ListAllAsync();
            return Ok(mapper.Map<List<ProductResponse>>(productFromDb));
        }
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetAllByCategoryIdAsync(int categoryId)
        {

            List<Product> productsFromDb = await productService.ListAllByCategoryIdAsync(categoryId);

            return Ok(mapper.Map<List<ProductResponse>>(productsFromDb));
        }
        [HttpGet("pettype/{petTypeId}")]
        public async Task<IActionResult> GetAllByPetTypeIdAsync(int petTypeId)
        {

            List<Product>? productsFromDb = await productService.ListAllByPetTypeIdAsync(petTypeId);

            return Ok(mapper.Map<List<ProductResponse>>(productsFromDb));
        }
        [HttpGet("category/{categoryId}/pettype/{petTypeId}")]
        public async Task<IActionResult> GetAllByCategoryAndPetTypeIdAsync(int categoryId, int petTypeId)
        {

            List<Product> productsFromDb = await productService.ListAllByCategoryAndPetTypeIdAsync(categoryId, petTypeId);

            return Ok(mapper.Map<List<ProductResponse>>(productsFromDb));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithAllInfoAsync(int id)
        {
            Product? productFromDb = await productService.SelectByIdWithAllInfoAsync(id);
            return Ok(mapper.Map<ProductResponse>(productFromDb));

        }

    }
}
