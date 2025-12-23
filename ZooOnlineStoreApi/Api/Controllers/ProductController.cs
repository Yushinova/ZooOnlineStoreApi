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
            try
            {
                ProductResponse response = await productService.SelectByIdWithAllInfoAsync(id);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }

        //работа с продуктами (только роль админ)
        [HttpPost("admin")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<ActionResult> InsertProductAsync([FromBody] ProductRequest request)
        {
            try
            {
                ProductResponse response = await productService.InsertAsync(request);
                return Ok(response);
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
                ProductResponse response = await productService.UpdateAsync(id, request);
                return Ok(response);
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
    }
}
