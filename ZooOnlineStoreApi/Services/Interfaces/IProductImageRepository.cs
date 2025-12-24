
using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IProductImageRepository:IRepository<ProductImage>
    {
        Task<List<ProductImage>> SelectAllByProductIdAsync(int productId);
        Task DeleteRangeAsynk(List<ProductImage> images);
        Task<ProductImage?> GetByNameAsync(string name);
    }
}
