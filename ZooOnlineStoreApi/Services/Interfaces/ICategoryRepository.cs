using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<List<Category>> SelectAllByPetTypeIdAsync(int petTypeId);
        Task<Category?> SelectByName(string name);
    }
}
