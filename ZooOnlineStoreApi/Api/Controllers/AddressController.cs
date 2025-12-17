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
        private readonly IMapper mapper;
        public AddressController(AddressService addressService, IMapper mapper)
        {
            this.addressService = addressService;
            this.mapper = mapper;
        }
        [HttpPost]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> InsertAsync([FromBody] AddressRequest request)
        {
            try
            {
                Address addressInsert = mapper.Map<Address>(request);
                addressInsert.CreatedAt = DateTime.UtcNow;
                await addressService.InsertAsync(addressInsert);
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
            List<Address>? adressesFromDb = await addressService.ListAllByUserIdAsync(id);
            return Ok(mapper.Map<List<AddressResponse>>(adressesFromDb));
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
