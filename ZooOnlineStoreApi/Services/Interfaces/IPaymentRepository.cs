
using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IPaymentRepository: IRepository<Payment>
    {
        Task<List<Payment>> SelectAllByUserIdAsync(int userId);
        Task<Payment> InsertReturnEntityAsync(Payment entity);
        Task<Payment> UpdateReturnEntityAsync(Payment entity);
        Task<PaymentPaged> SelectWithPagination(PaymentFilter parameters);
    }
}
