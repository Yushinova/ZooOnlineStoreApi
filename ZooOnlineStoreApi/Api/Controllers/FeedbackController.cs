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
        [Authorize]
        public async Task<IActionResult> AddNewFeedbackAsync([FromBody] FeedbackRequest request)
        {
            try
            {
                Feedback feedbackInsert = mapper.Map<Feedback>(request);
                feedbackInsert.CreatedAt = DateTime.UtcNow;
                await feedbackService.InsertAsync(feedbackInsert);
                //Product rating update
                Product? productFromDb = await productService.SelectByIdAsync(feedbackInsert.ProductId);
                double ratingAverage = await feedbackService.GetAverageProductRatingAsync(feedbackInsert.ProductId);
                if (productFromDb != null)
                {
                    productFromDb.Rating = ratingAverage;
                    await productService.UpdateAsync(productFromDb);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacksAsync()
        {
            List<Feedback>? feedbacksFromDb = await feedbackService.ListAllAsync();
            return Ok(mapper.Map<List<FeedbackResponse>>(feedbacksFromDb));
        }
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetAllByProductIdAsync([FromQuery] int page, [FromQuery] int pageSize, int productId )
        {
            List<Feedback>? feedbacksFromDb = await feedbackService.GetAllByProductIdWithPaginationAsync(productId, page, pageSize);
            return Ok(mapper.Map<List<FeedbackResponse>>(feedbacksFromDb));
        }

    }
}
