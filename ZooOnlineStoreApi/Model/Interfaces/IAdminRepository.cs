using ZooOnlineStoreApi.Model.Admins;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IAdminRepository: IRepository<Admin>
    {
        Task<Admin?> GetByLoginAsync(string login);
    }
}
