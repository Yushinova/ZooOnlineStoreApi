using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService feedbackService;
        public FeedbackController(FeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        [HttpPost]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> AddNewFeedbackAsync([FromBody] FeedbackRequest request)
        {
            FeedbackResponse response = await feedbackService.InsertAsync(request);
            return Ok(response);
        }

        [HttpGet("check/{productId}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> CheckUserFeedbackAsync(int productId)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new StringMessage("User ID not found in token"));
            }

            int id = int.Parse(userId);
            FeedbackResponse existingReview = await feedbackService.GetByUserIdAndProductIdAsync(id, productId);

            if (existingReview != null)
            {
                return Ok(existingReview); //отзыв найден
            }
            else
            {
                return Ok(new StringMessage($"No feedback found for product {productId}"));
            }
        }

        [HttpGet]//тесты
        public async Task<IActionResult> GetAllFeedbacksAsync()
        {
            List<FeedbackResponse> response = await feedbackService.ListAllAsync();
            return Ok(response);
        }

        [HttpGet("product/top/{productId:int}")]
        public async Task<IActionResult> GetTopByProductIdAsync([FromQuery] int page, [FromQuery] int pageSize, int productId)
        {
            List<FeedbackResponse> response = await feedbackService.GetAllByProductIdWithPaginationAsync(productId, page, pageSize);
            return Ok(response);
        }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetAllByProductIdAsync(int productId)
        {
            List<FeedbackResponse> response = await feedbackService.GetAllByProductIdAsync(productId);
            return Ok(response);
        }
    }
}
