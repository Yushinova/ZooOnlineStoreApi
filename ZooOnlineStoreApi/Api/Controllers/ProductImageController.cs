using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly ProductImageService productImageService;
        public ProductImageController(ProductImageService productImageService)
        {
            this.productImageService = productImageService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertAsync([FromBody] ProductImageRequest request)
        {
            await productImageService.InsertAsync(request);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteByNameAsync([FromQuery] string name)
        {
            await productImageService.DeleteByNameAsync(name);
            return Ok();
        }

        /////пока не использую
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await productImageService.DeleteByIdAsync(id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> ListAllAsync()
        {
            List<ProductImageResponse> response = await productImageService.ListAllAsync();
            return Ok(response);
        }

    }
}
