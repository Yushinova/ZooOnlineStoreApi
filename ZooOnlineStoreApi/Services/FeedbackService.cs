using AutoMapper;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Services
{
    public class FeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public FeedbackService(IFeedbackRepository feedbackRepository, IProductRepository productRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _productRepository = productRepository;
            _mapper = mapper;

        }
        public async Task<FeedbackResponse> InsertAsync(FeedbackRequest request)
        {
            Feedback? existing = await _feedbackRepository.SelectByUserIdAndProductId(request.UserId, request.ProductId);
            if (existing != null)
            {
                throw new Exception($"the user: {request.UserId} has already left a review for product: {request.ProductId}");
            }
            Feedback feedbackInsert = _mapper.Map<Feedback>(request);
            feedbackInsert.CreatedAt = DateTime.UtcNow;
            Feedback? newFeedback = await _feedbackRepository.InsertAndRerunAsync(feedbackInsert);
            double ratingAverage = await GetAverageProductRatingAsync(feedbackInsert.ProductId);
            Product? product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product != null)
            {
                product.Rating = ratingAverage;
                await _productRepository.UpdateAsync(product);
            }
            return _mapper.Map<FeedbackResponse>(newFeedback);
        }
        public async Task<FeedbackResponse> GetByUserIdAndProductIdAsync(int userId, int productId)
        {
            Feedback? feedback = await _feedbackRepository.SelectByUserIdAndProductId(userId,productId);
            return _mapper.Map<FeedbackResponse>(feedback);

        }
        public async Task<List<FeedbackResponse>> ListAllAsync()
        {
            List<Feedback>? feedbacks = await _feedbackRepository.SelectAllAsync();
            return _mapper.Map<List<FeedbackResponse>>(feedbacks);
        }
        public async Task<List<Feedback>?> GetAllByUserIdWithPaginationAsync(int userId, int page, int count)
        {
            if (page < 1) page = 1;
            int skip = (page - 1) * count;
            return await _feedbackRepository.SelectByUserIdWithPaginationAsync(userId, skip, count);
        }
        public async Task<List<FeedbackResponse>> GetAllByProductIdWithPaginationAsync(int productId, int page, int count)
        {
            if (page < 1) page = 1;
            int skip = (page - 1) * count;
            List<Feedback>? feedbacks = await _feedbackRepository.SelectByProductIdWithPaginationAsync(productId, skip, count);
            return _mapper.Map<List<FeedbackResponse>>(feedbacks);
        }
        public async Task<List<FeedbackResponse>> GetAllByProductIdAsync(int productId)
        {
            List<Feedback>? feedbacks = await _feedbackRepository.SelectByProductIdAsync(productId);
            return _mapper.Map<List<FeedbackResponse>>(feedbacks);
        }
        public async Task<double> GetAverageProductRatingAsync(int productId)
        {
            List<Feedback>? feddbaksFromDb = await _feedbackRepository.SelectByProductIdAsync(productId);

            double averageRating = feddbaksFromDb.Any() ? feddbaksFromDb.Average(f => f.Rating) : 0;
            return averageRating;
        }
    }
}
