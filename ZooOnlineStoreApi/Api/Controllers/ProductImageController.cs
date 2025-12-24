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
    public class ProductImageController: ControllerBase
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
            try
            {
                await productImageService.InsertAsync(request);
                return Ok();
            }
            catch(DuplicationException ex)
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteByNameAsync([FromQuery] string name)
        {
            try
            {
                await productImageService.DeleteByNameAsync(name);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        /////пока не использую
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await productImageService.DeleteByIdAsync(id);
                return Ok();
            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListAllAsync()
        {
            List<ProductImageResponse> response = await productImageService.ListAllAsync();
            return Ok(response);
        }

    }
}
