using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Model.Payments
{
    public class PaymentService
    {
        IPaymentRepository _payments { get; set; }
        public PaymentService(IPaymentRepository payment)
        {
            _payments = payment;
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
        public async Task<PaymentPagedResponse> SelectWithPagination(PaymentQueryParameters parameters)
        {
            return await _payments.SelectWithPagination(parameters);
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
