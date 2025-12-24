using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IAddressRepository:IRepository<Address>
    {
        Task<List<Address>?> SelectByUserIdAsync(int id);
    }
}
