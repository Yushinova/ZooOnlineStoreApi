using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IFeedbackRepository: IRepository<Feedback>
    {
        Task<List<Feedback>?>  SelectByUserIdWithPaginationAsync(int userId, int page, int count);
        Task<List<Feedback>?> SelectByProductIdWithPaginationAsync(int  productId, int page, int count);
        Task<List<Feedback>?> SelectByProductIdAsync(int productId);
        Task<Feedback?> InsertAndRerunAsync(Feedback entity);
        Task<Feedback?> SelectByUserIdAndProductId(int userId, int productId);
    }
}
