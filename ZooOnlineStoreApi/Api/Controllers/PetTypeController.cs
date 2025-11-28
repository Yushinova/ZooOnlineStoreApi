using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/pettype")]
    [ApiController]
    public class PetTypeController: ControllerBase
    {
        private readonly PetTypeService petTypes;
        private readonly CategoryService categoryService;
        private readonly IMapper mapper;
        public PetTypeController(PetTypeService petTypes,  IMapper mapper, CategoryService categoryService)
        {
            this.petTypes = petTypes;
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllWithCategotiesAsync()
        {   
                List<PetType> petTypeFromDb = await petTypes.ListAllWithCategories();
                return Ok(mapper.Map<List<PetTypeResponse>>(petTypeFromDb));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<PetType> petTypesFromDb = await petTypes.ListAllAsync();
            return Ok(mapper.Map<List<PetTypeShortResponse>>(petTypesFromDb));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithCategories(int id)
        {
            PetType? petTypeFromDb =await petTypes.SelectByIdWithCategoties(id);
            return Ok(mapper.Map<PetTypeResponse>(petTypeFromDb));
        }
        
        [HttpPost]
       // [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> InsertAsync(PetTypeRequest data)
        {
            try
            {
                await petTypes.InsertAsync(data.Name, data.ImageName);
                PetType? petTypeFromDb = await petTypes.GetNyNameAsync(data.Name);
                return Ok(mapper.Map<PetTypeResponse>(petTypeFromDb));
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

        [HttpPatch]
       // [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> UpdatePetTypeAsync(PetTypeUpdate request)
        {
            try
            {

                PetType petTypeUpdate = mapper.Map<PetType>(request);
                if (request.CategoriesIds != null && request.CategoriesIds.Any())
                {
                    List<Category> categoriesFromDb = await categoryService.ListAllAsync();
                    petTypeUpdate.Categories = new HashSet<Category>();
                    foreach (var item in categoriesFromDb)
                    {
                        if (request.CategoriesIds.Contains(item.Id))
                        {
                            petTypeUpdate.Categories.Add(item);
                        }
                    }
                }
                await petTypes.UpdateAsync(petTypeUpdate);
                PetType? petTypeFromDb = await petTypes.SelectByIdWithCategoties(petTypeUpdate.Id);
                return Ok(mapper.Map<PetTypeResponse>(petTypeFromDb));

            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        [HttpDelete("{id:int}")]
       // [Authorize(Roles =JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await petTypes.DeleteByIdAsync(id);
                return Ok();
            }
            catch (NotFoundException ex) {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);

            }
        }
        //удаление категории из списка типа животного
        [HttpDelete("{petTypeId}/categories/{categoryId}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> DeleteCategoryFromPetType(int petTypeId, int categoryId)
        {
            try
            {
               await petTypes.RemoveCategoryByIdFromPetType(petTypeId, categoryId);
                return Ok();
            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        //добавление категории в список категорий животного
        [HttpPost("{petTypeId}/categories/{categoryId}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> AddCategoryToPetType(int petTypeId, int categoryId)
        {
            try
            {
                await petTypes.AddCategoryToPetTypeAsync(petTypeId, categoryId);
                return Ok();
            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
            catch (DuplicationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }
        }
    }
}
