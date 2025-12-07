using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Users;
using ZooOnlineStoreApi.Storage;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService categories;
        private readonly IMapper mapper;

        public CategoryController(CategoryService categories, IMapper mapper)
        {
            this.categories = categories;
            this.mapper = mapper;
        }
        //
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Category> categoriesFromDb = await categories.ListAllAsync();

            return Ok(mapper.Map<List<CategoryResponse>>(categoriesFromDb));
        }
        //получить по id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            Category? categoryFromDb = await categories.GetByIdAsync(id);
            return Ok(mapper.Map<CategoryResponse>(categoryFromDb));
        }
        //получить по id
        [HttpGet("pettype/{id:int}")]
        public async Task<IActionResult> GetByPetTypeIdAsync(int id)
        {

            List<Category>? categoryFromDb = await categories.ListAllByPetTypeIdAsync(id);
            return Ok(mapper.Map<List<CategoryResponse>>(categoryFromDb));
        }
        //добавление
        [HttpPost]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> InsertAsync(CategoryRequest data)
        {
            try
            {
                await categories.InsertAsync(data.Name);
                Category? categoryFromDb = await categories.GetByNameAsync(data.Name);
                return Ok(mapper.Map<CategoryResponse>(categoryFromDb));
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
                await categories.DeleteAsync(id);
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
