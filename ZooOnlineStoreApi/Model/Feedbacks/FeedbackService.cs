using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Model.Feedbacks
{
    public class FeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;

        }
        public async Task<Feedback?> InsertAsync(Feedback feedback)
        {
            return await _feedbackRepository.InsertAndRerunAsync(feedback);
        }
        public async Task<Feedback?> GetByUserIdAndProductIdAsync(int userId, int productId)
        {
            Feedback? feedback = await _feedbackRepository.SelectByUserIdAndProductId(userId,productId);
            return feedback;

        }
        public async Task<List<Feedback>?> ListAllAsync()
        {
            return await _feedbackRepository.SelectAllAsync();
        }
        public async Task<List<Feedback>?> GetAllByUserIdWithPaginationAsync(int userId, int page, int count)
        {
            if (page < 1) page = 1;
            int skip = (page - 1) * count;
            return await _feedbackRepository.SelectByUserIdWithPaginationAsync(userId, skip, count);
        }
        public async Task<List<Feedback>?> GetAllByProductIdWithPaginationAsync(int productId, int page, int count)
        {
            if (page < 1) page = 1;
            int skip = (page - 1) * count;
            return await _feedbackRepository.SelectByProductIdWithPaginationAsync(productId, skip, count);
        }
        public async Task<List<Feedback>?> GetAllByProductIdAsync(int productId)
        {
            return await _feedbackRepository.SelectByProductIdAsync(productId);
        }
        public async Task<double> GetAverageProductRatingAsync(int productId)
        {
            List<Feedback>? feddbaksFromDb = await _feedbackRepository.SelectByProductIdAsync(productId);

            double averageRating = feddbaksFromDb.Any() ? feddbaksFromDb.Average(f => f.Rating) : 0;
            return averageRating;
        }
    }
}
