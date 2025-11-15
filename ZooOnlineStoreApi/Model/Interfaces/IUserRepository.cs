using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task <User?> GetByPhoneAsync(string phone);
        Task<User?> GetByUUIDAsync(Guid uuid);
    }
}
