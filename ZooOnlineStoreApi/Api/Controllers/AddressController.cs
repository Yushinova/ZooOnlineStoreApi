using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Addresses;
using ZooOnlineStoreApi.Model.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AddressService addressService;
        public AddressController(AddressService addressService)
        {
            this.addressService = addressService;
        }

        [HttpPost]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> InsertAsync([FromBody] AddressRequest request)
        {
            try
            {
                await addressService.InsertAsync(request);
                return Created();
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
         
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> GetByUserIdAsync(int id)
        {
            List<AddressResponse> responses = await addressService.ListAllByUserIdAsync(id);
            return Ok(responses);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await addressService.DeleteAsync(id);
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
