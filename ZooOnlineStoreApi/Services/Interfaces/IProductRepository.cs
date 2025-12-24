
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<Product?> SelectByIdWithAllInfo(int id);
        IQueryable<Product> SelectAllWithImagesAndPetTypesAsync();
        Task<List<Product>?> SelectAllWithFilters(ProductQueryParameters parameters);
        Task<Product> InsertAndReturnAsync(Product entity);
    }
}
