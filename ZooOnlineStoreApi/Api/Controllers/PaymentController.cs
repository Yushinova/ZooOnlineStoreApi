using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly PaymentService paymentService;
        public PaymentController(PaymentService paymentService, IMapper mapper)
        {
            this.paymentService = paymentService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> InsertAsync([FromBody] PaymentRequest request)
        {

            Payment payment = mapper.Map<Payment>(request);
            //пока сделаем все оплачено
            //payment.PaidAt = DateTime.UtcNow;
            Payment paymentFromDb = await paymentService.InsertReturnEntityAsync(payment);
            return Ok(mapper.Map<PaymentResponse>(paymentFromDb));
        }

        [HttpGet("user/{userId:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> GetByUserIdAsync(int userId, PaymentRequestParams parameters)
        {
            parameters.UserId = userId;
            PaymentPagedResponse response = await paymentService.SelectWithPagination(parameters);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListWithPagination([FromBody] PaymentRequestParams parameters)
        {

            return Ok(await paymentService.SelectWithPagination(parameters));
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PaymentRequest request)
        {
            Payment paymentUpdated = mapper.Map<Payment>(request);
            paymentUpdated.Id = id;
            if (request.Status.ToLower() == PaymentStatus.Succeeded.ToString().ToLower())
            {
                paymentUpdated.PaidAt = DateTime.UtcNow;
            }
            Payment paymentFromDb = await paymentService.UpdateReturnEntityAsync(paymentUpdated);
            return Ok(mapper.Map<PaymentResponse>(paymentFromDb));
        }
    }

}
