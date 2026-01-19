
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<Product?> SelectByIdWithAllInfo(int id);
        IQueryable<Product> SelectAllWithImagesAndPetTypes();
        Task<Product> InsertAndReturnAsync(Product entity);
        Task AddQuantityByIdAsync(int productId, int quantity);
        Task DeleteQuantityByIdAsync(int productId, int quantity);
    }
}
