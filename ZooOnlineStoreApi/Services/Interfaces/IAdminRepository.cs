using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IAdminRepository: IRepository<Admin>
    {
        Task<Admin?> GetByLoginAsync(string login);
    }
}
