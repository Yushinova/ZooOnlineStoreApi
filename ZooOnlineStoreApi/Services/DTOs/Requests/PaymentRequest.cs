
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
        public string PaymentMethod { get; set; } = string.Empty;
        public string ExternalPaymentId { get; set; } = string.Empty;//получен из платежной системы
        public string Status { get; set; } = string.Empty;//pending, succeeded, canceled
        public int OrderId { get; set; }
    }
}
