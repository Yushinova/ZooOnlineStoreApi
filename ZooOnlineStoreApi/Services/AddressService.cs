using AutoMapper;
using System.Collections.Specialized;
using System.ComponentModel;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services
{
    public class AddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }
        public async Task InsertAsync(AddressRequest request)
        {
            Address addressInsert = _mapper.Map<Address>(request);
            addressInsert.CreatedAt = DateTime.UtcNow;
            await _addressRepository.InsertAsync(addressInsert);
        }
        public async Task DeleteAsync(int id)
        {
            Address? addressFromDb = await _addressRepository.GetByIdAsync(id);
            if (addressFromDb == null)
            {
                throw new NotFoundException("address not found");
            }
            await _addressRepository.DeleteAsync(addressFromDb);
        }
        public async Task<List<AddressResponse>> ListAllByUserIdAsync(int id)
        {
            List<Address>? addressesFromDb = await _addressRepository.SelectByUserIdAsync(id);

            return _mapper.Map<List<AddressResponse>>(addressesFromDb);
        }
    }
}
