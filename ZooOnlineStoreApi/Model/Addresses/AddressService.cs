using System.Collections.Specialized;
using System.ComponentModel;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Model.Addresses
{
    public class AddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public async Task InsertAsync(Address address)
        {
           await _addressRepository.InsertAsync(address);
        }
        public async Task DeleteAsync(int id)
        {
            Address? addressFromDb = await _addressRepository.GetByIdAsync(id);
            if (addressFromDb == null)
            {
                throw new NotFoundException();
            }
            await _addressRepository.DeleteAsynk(addressFromDb);
        }
        public async Task<List<Address>?> ListAllByUserIdAsync(int id)
        {
            return await _addressRepository.SelectByUserIdAsync(id);
        }
    }
}
