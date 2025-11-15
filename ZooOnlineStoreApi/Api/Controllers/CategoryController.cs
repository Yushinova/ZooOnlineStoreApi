using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
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
        public async Task<IActionResult> GetAllAsynk()
        {
            List<Category> categoriesFromDb = await categories.ListAllAsync();

            return Ok(mapper.Map<List<CategoryResponse>>(categoriesFromDb));
        }
        //получить по id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllByPetTypeIdAsynk(int id)
        {

            List<Category> categoriesFromDb = await categories.ListAllByPetTypeIdAsync(id);
            return Ok(mapper.Map<List<CategoryResponse>>(categoriesFromDb));
        }
        //добавление
        [HttpPost]
        public async Task<IActionResult> InsertAsync(CategoryRequest data)
        {
            try
            {
                await categories.InsertAsync(data.Name);
                Category categoryFromDb = await categories.GetByNameAsynk(data.Name);
                return Ok(mapper.Map<CategoryResponse>(categoryFromDb));
            }
            catch (DuplicationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }

        }
        //редактирование
        [HttpPatch]
        public async Task<IActionResult> UpdateCategoryAsync(Category data)
        {
            try
            {
                await categories.UpdateAsync(data);
                return Ok(await categories.GetByNameAsynk(data.Name));
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
    }
}
