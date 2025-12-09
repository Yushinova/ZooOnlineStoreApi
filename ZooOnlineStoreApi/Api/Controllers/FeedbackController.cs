using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        //[Authorize]
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
