using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Model.Addresses;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Payments;

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
            try
            {
                Payment payment = mapper.Map<Payment>(request);
                //пока сделаем все оплачено
                //payment.PaidAt = DateTime.UtcNow;
                Payment paymentFromDb = await paymentService.InsertReturnEntityAsync(payment);
                return Ok(mapper.Map<PaymentResponse>(paymentFromDb));
            }
            catch (ValidationException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }

        [HttpGet("user/{userId:int}")]
        [Authorize(Roles = JwtService.USER_ROLE)]
        public async Task<IActionResult> GetByUserIdAsync(int userId)
        {
            try
            {
                List<Payment>? payments = await paymentService.SelectAllByUserIdAsync(userId);
                return Ok(mapper.Map<List<PaymentResponse>>(payments));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }

        [HttpGet]//для тетов
        [Authorize]
        public async Task<IActionResult> ListAllAsync()
        {
            List<Payment> paymentsFromDb = await paymentService.SelectAllAsync();
            return Ok(mapper.Map<List<PaymentResponse>>(paymentsFromDb));
        }

        [HttpGet("pagin")]
        [Authorize]
        public async Task<IActionResult> ListWithPagination([FromBody] PaymentQueryParameters parameters)
        {
            try
            {
                return Ok(await paymentService.SelectWithPagination(parameters));
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PaymentRequest request)
        {
            try
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
            catch (NotFoundException ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return BadRequest(error);
            }

        }
    }

}
