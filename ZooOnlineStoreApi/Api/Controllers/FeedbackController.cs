using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService feedbackService;
        private readonly ProductService productService;
        private readonly IMapper mapper;
        public FeedbackController(FeedbackService feedbackService, ProductService productService, IMapper mapper)
        {
            this.feedbackService = feedbackService;
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewFeedbackAsync([FromBody] FeedbackRequest request)
        {
            try
            {
                Feedback feedbackInsert = mapper.Map<Feedback>(request);
                feedbackInsert.CreatedAt = DateTime.UtcNow;
                Feedback? newFeedback = await feedbackService.InsertAsync(feedbackInsert);
                double ratingAverage = await feedbackService.GetAverageProductRatingAsync(feedbackInsert.ProductId);
                await productService.UpdateRatingAsync(feedbackInsert.ProductId, ratingAverage);
                return Ok(mapper.Map<FeedbackResponse>(newFeedback));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpGet("check/{productId}")]
        [Authorize]
        public async Task<IActionResult> CheckUserReview(int productId) // или Guid
        {
            try
            {
                // Получаем int userId из токена
                var userId = User.FindFirst("userId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new StringMessage("User ID not found in token"));
                }
                int id = int.Parse(userId);
                // ⭐ Ищем отзыв с int userId
                var existingReview = await feedbackService.GetByUserIdAndProductIdAsync(id, productId);

                if (existingReview != null) {
                    return Ok(mapper.Map<FeedbackResponse>(existingReview));
                }
                else
                {
                    return Ok(new StringMessage("Feedback exist with product: "+productId));
                }

            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }

        [HttpGet]//тесты
        public async Task<IActionResult> GetAllFeedbacksAsync()
        {
            List<Feedback>? feedbacksFromDb = await feedbackService.ListAllAsync();
            return Ok(mapper.Map<List<FeedbackResponse>>(feedbacksFromDb));
        }

        [HttpGet("product/top/{productId:int}")]
        public async Task<IActionResult> GetTopByProductIdAsync([FromQuery] int page, [FromQuery] int pageSize, int productId )
        {
            List<Feedback>? feedbacksFromDb = await feedbackService.GetAllByProductIdWithPaginationAsync(productId, page, pageSize);
            return Ok(mapper.Map<List<FeedbackResponse>>(feedbacksFromDb));
        }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetAllByProductIdAsync(int productId)
        {
            List<Feedback>? feedbacksFromDb = await feedbackService.GetAllByProductIdAsync(productId);
            return Ok(mapper.Map<List<FeedbackResponse>>(feedbacksFromDb));
        }
    }
}
