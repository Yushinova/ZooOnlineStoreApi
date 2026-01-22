using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _payments;
        private readonly IMapper _mapper;
        public PaymentService(IPaymentRepository payment, IMapper mapper)
        {
            _payments = payment;
            _mapper = mapper;
        }
        public async Task<Payment?> GetByIdAsync(int id)
        {
            Payment? paymentFromDb = await _payments.GetByIdAsync(id);
            if (paymentFromDb == null)
            {
                throw new NotFoundException("payment not found");
            }
            return paymentFromDb;
        }
        public async Task<PaymentPagedResponse> SelectWithPagination(PaymentRequestParams parameters)
        {
            PaymentFilter filter = _mapper.Map<PaymentFilter>(parameters);
            return _mapper.Map<PaymentPagedResponse>( await _payments.SelectWithPagination(filter));
        }
        public async Task<PaymentResponse> InsertReturnEntityAsync(PaymentRequest request)
        {
            Payment payment = _mapper.Map<Payment>(request);
            Payment newpayment = await _payments.InsertReturnEntityAsync(payment);
            if (payment == null)
            {
                throw new Exception("Payment save exception");
            }
            return _mapper.Map<PaymentResponse>(newpayment);
        }

        public async Task<List<Payment>> SelectAllAsync()
        {
            return await _payments.SelectAllAsync();
        }

        public async Task<List<Payment>> SelectAllByUserIdAsync(int userId)
        {
            return await _payments.SelectAllByUserIdAsync(userId);
        }

        public async Task<PaymentResponse> UpdateReturnEntityAsync(int id, PaymentRequest request)
        {
            Payment paymentUpdated = _mapper.Map<Payment>(request);
            if (request.Status.ToLower() == PaymentStatus.Succeeded.ToString().ToLower())
            {
                paymentUpdated.PaidAt = DateTime.UtcNow;
            }
            Payment? paymentFromDb = await _payments.GetByIdAsync(id);
            if (paymentFromDb == null)
            {
                throw new NotFoundException();
            }
            paymentFromDb.Status = paymentUpdated.Status;
            paymentFromDb.PaidAt = paymentUpdated.PaidAt;
            return _mapper.Map<PaymentResponse>(await _payments.UpdateReturnEntityAsync(paymentFromDb));
        }
    }
}
