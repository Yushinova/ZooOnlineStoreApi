using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.Payments;

namespace ZooOnlineStoreApi.Model.Interfaces
{
    public interface IPaymentRepository: IRepository<Payment>
    {
        Task<List<Payment>> SelectAllByUserIdAsync(int userId);
        Task<Payment> InsertReturnEntityAsync(Payment entity);
        Task<Payment> UpdateReturnEntityAsync(Payment entity);
        Task<PaymentPaged> SelectWithPagination(PaymentFilter parameters);
    }
}
