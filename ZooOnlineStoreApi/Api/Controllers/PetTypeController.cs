using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/pettype")]
    [ApiController]
    public class PetTypeController : ControllerBase
    {
        private readonly PetTypeService petTypeService;
        public PetTypeController(PetTypeService petTypeService)
        {
            this.petTypeService = petTypeService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllWithCategotiesAsync()
        {
            List<PetTypeResponse> response = await petTypeService.ListAllWithCategories();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<PetTypeResponse> response = await petTypeService.ListAllAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithCategories(int id)
        {
            try
            {
                PetTypeResponse response = await petTypeService.SelectByIdWithCategoties(id);
                return Ok(response);
            }
            catch(NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }

        }

        [HttpPost]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> InsertAsync(PetTypeRequest request)
        {
            try
            {
                await petTypeService.InsertAsync(request.Name, request.ImageName);
                PetTypeResponse response = await petTypeService.GetByNameAsync(request.Name);
                return Ok(response);
            }
            catch (DuplicationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Conflict(error);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpPatch]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> UpdatePetTypeAsync(PetTypeUpdate request)
        {
            try
            {
                await petTypeService.UpdateAsync(request);
                PetTypeResponse response = await petTypeService.SelectByIdWithCategoties(request.Id);
                return Ok(response);

            }
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return NotFound(error);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = JwtService.ADMIN_ROLE)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await petTypeService.DeleteByIdAsync(id);
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
