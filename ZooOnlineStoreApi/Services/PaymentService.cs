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
                throw new NotFoundException();
            }
            return paymentFromDb;
        }
        public async Task<PaymentPagedResponse> SelectWithPagination(PaymentRequestParams parameters)
        {
            PaymentFilter filter = _mapper.Map<PaymentFilter>(parameters);
            return _mapper.Map<PaymentPagedResponse>( await _payments.SelectWithPagination(filter));
        }
        public async Task<Payment> InsertReturnEntityAsync(Payment entity)
        {
            //validation
            Payment payment = await _payments.InsertReturnEntityAsync(entity);
            if (payment == null)
            {
                throw new Exception("Payment save exception");
            }
            return payment;
        }

        public async Task<List<Payment>> SelectAllAsync()
        {
            return await _payments.SelectAllAsync();
        }

        public async Task<List<Payment>> SelectAllByUserIdAsync(int userId)
        {
            return await _payments.SelectAllByUserIdAsync(userId);
        }

        public async Task<Payment> UpdateReturnEntityAsync(Payment entity)
        {
            Payment? paymentFromDb = await _payments.GetByIdAsync(entity.Id);
            if (paymentFromDb == null)
            {
                throw new NotFoundException();
            }
            paymentFromDb.Status = entity.Status;
            paymentFromDb.PaidAt = entity.PaidAt;
            return await _payments.UpdateReturnEntityAsync(paymentFromDb);
        }
    }
}
