using ZooOnlineStoreApi.Model.Categories;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<List<Category>> SelectAllByPetTypeIdAsync(int petTypeId);
        Task<Category?> SelectByName(string name);
    }
}
