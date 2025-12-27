using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;
        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWithFilterAndPagination([FromQuery] ProductQueryParameters parameters)
        {
            List<ProductResponse> response = await productService.SuperPagination(parameters);
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithAllInfoAsync(int id)
        {
            ProductResponse response = await productService.SelectByIdWithAllInfoAsync(id);
            return Ok(response);
        }
        //работа с продуктами (только роль админ)
        [HttpPost("admin")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<ActionResult> InsertProductAsync([FromBody] ProductRequest request)
        {
            ProductResponse response = await productService.InsertAsync(request);
            return Ok(response);
        }
        [HttpPatch("admin/{id:int}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] ProductRequest request)
        {
            ProductResponse response = await productService.UpdateAsync(id, request);
            return Ok(response);
        }
        [HttpDelete("admin/{id:int}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await productService.DeleteAsync(id);
            return Ok();
        }
    }
}
