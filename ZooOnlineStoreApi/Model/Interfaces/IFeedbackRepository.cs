using ZooOnlineStoreApi.Model.Feedbacks;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IFeedbackRepository: IRepository<Feedback>
    {
        Task<List<Feedback>?>  SelectByUserIdWithPaginationAsync(int userId, int page, int count);
        Task<List<Feedback>?> SelectByProductIdWithPaginationAsync(int  productId, int page, int count);
        Task<List<Feedback>?> SelectByProductIdAsync(int productId);
    }
}
