using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<List<Product>> SelectAllByCategoryAndPetTypeIdAsync(int categoryId, int petTypeId);
        Task<List<Product>> SelectAllByPetTypeIdAsync(int petTypeId);
        Task<List<Product>> SelectAllByCategoryIdAsync(int CategoryId);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
        Task<Product?> SelectByIdWithAllInfo(int id);
    }
}
