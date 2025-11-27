using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.ProductImages;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ProductImageController: ControllerBase
    {
        private readonly ProductImageService productImageService;
        private readonly IMapper mapper;
        public ProductImageController(ProductImageService productImageService, IMapper mapper)
        {
            this.productImageService = productImageService;
            this.mapper = mapper;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertAsync([FromBody] ProductImageRequest request)
        {
            try
            {
                ProductImage imageInsert = mapper.Map<ProductImage>(request);
                await productImageService.InsertAsync(imageInsert);
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
            List<ProductImage> imagesFromDb = await productImageService.ListAllAsync();
            return Ok(mapper.Map<List<ProductImageResponse>>(imagesFromDb));
        }

    }
}
