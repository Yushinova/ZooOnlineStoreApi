using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task <User?> GetByPhoneAsync(string phone);
        Task<User?> GetByEmailAsync(string email);
    }
}
