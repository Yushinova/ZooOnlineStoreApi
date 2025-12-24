
using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IPetTypeRepository: IRepository<PetType>
    {
        Task<List<PetType>> SelectAllByProductIdAsync(int productId);
        Task <PetType?> SelectByNameAsync(string name);
        Task<List<PetType>> SelectAllWithCategoies();
        Task<PetType?> SelectByIdWithCategories(int id);
    }
}
