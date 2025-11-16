using ZooOnlineStoreApi.Model.Addresses;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IAddressRepository:IRepository<Address>
    {
        Task<List<Address>?> SelectByUserIdAsync(int id);
    }
}
