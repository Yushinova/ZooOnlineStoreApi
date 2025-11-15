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
        private readonly PetTypeService petTypeService;
        private readonly IMapper mapper;
        public ProductController(ProductService productService, PetTypeService petTypeService, IMapper mapper)
        {
            this.productService = productService;
            this.petTypeService = petTypeService;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult> InsertProductAsync([FromBody] ProductRequest request)
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
        [HttpPatch("{id:int}")]
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
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
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

            List<Product> productsFromDb = await productService.ListAllByPetTypeIdAsync(petTypeId);

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
