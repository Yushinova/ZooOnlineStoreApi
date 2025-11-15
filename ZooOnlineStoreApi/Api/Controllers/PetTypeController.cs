using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.PetTypes;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/pettype")]
    [ApiController]
    public class PetTypeController: ControllerBase
    {
        private readonly PetTypeService petTypes;
        private readonly IMapper mapper;
        public PetTypeController(PetTypeService petTypes, IMapper mapper)
        {
            this.petTypes = petTypes;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithCategotiesAsync()
        {   
                List<PetType> petTypeFromDb = await petTypes.ListAllWithCategories();
                return Ok(mapper.Map<List<PetTypeResponse>>(petTypeFromDb));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithCategories(int id)
        {
            PetType? petTypeFromDb =await petTypes.SelectByIdWithCategoties(id);
            return Ok(mapper.Map<PetTypeResponse>(petTypeFromDb));
        }
        [HttpPost]
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

        }
        //редактирование простых свойств
        [HttpPatch]
        public async Task<IActionResult> UpdatePetTypeAsync(PetType data)
        {
            try
            {
                await petTypes.UpdateAsync(data);
                return Ok(await petTypes.GetNyNameAsync(data.Name));
            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }
        //удаление категории из списка типа животного
        [HttpDelete("{petTypeId}/categories/{categoryId}")]
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
