using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Feedbacks;

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
            try
            {
                FeedbackResponse response = await feedbackService.InsertAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpGet("check/{productId}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> CheckUserFeedbackAsync(int productId)
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
                FeedbackResponse existingReview = await feedbackService.GetByUserIdAndProductIdAsync(id, productId);

                if (existingReview != null) {
                    return Ok(existingReview);
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
            List<FeedbackResponse> response = await feedbackService.ListAllAsync();
            return Ok(response);
        }

        [HttpGet("product/top/{productId:int}")]
        public async Task<IActionResult> GetTopByProductIdAsync([FromQuery] int page, [FromQuery] int pageSize, int productId )
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
