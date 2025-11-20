using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<Product?> SelectByIdWithAllInfo(int id);
        IQueryable<Product> SelectAllWithImagesAndPetTypesAsync();
        Task<List<Product>?> SelectAllWithFilters(ProductQueryParameters parameters);
    }
}
