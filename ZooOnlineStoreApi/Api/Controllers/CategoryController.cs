using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService categoryService;

        public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        //
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<CategoryResponse> categories = await categoryService.ListAllAsync();

            return Ok(categories);
        }
        //получить по id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            CategoryResponse response = await categoryService.GetByIdAsync(id);
            return Ok(response);
        }
        //получить по id
        [HttpGet("pettype/{id:int}")]
        public async Task<IActionResult> GetByPetTypeIdAsync(int id)
        {

            List<CategoryResponse> response = await categoryService.ListAllByPetTypeIdAsync(id);
            return Ok(response);
        }
        //добавление
        [HttpPost]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> InsertAsync(CategoryRequest request)
        {
            try
            {
                await categoryService.InsertAsync(request.Name);
                CategoryResponse response = await categoryService.GetByNameAsync(request.Name);
                return Ok(response);
            }
            catch (DuplicationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }
            catch(Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }
        //удаление
        [HttpDelete("{id:int}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                await categoryService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
    }
}
