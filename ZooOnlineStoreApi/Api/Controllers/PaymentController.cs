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
        private readonly PaymentService paymentService;
        public PaymentController(PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> InsertAsync([FromBody] PaymentRequest request)
        {
            //пока сделаем все оплачено
            //payment.PaidAt = DateTime.UtcNow;
            PaymentResponse newpayment = await paymentService.InsertReturnEntityAsync(request);
            return Ok(newpayment);
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
            PaymentResponse payment = await paymentService.UpdateReturnEntityAsync(id, request);
            return Ok(payment);
        }
    }

}
