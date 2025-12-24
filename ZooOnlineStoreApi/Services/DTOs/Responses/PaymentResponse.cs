
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string ExternalPaymentId { get; set; } = string.Empty;//получен из платежной системы
        public string Status { get; set; } = string.Empty;//pending, succeeded, canceled
        public OrderPaymentResponse Order { get; set; } = new OrderPaymentResponse();
    }
}
