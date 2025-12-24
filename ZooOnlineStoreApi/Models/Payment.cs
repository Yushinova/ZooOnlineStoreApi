using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ZooOnlineStoreApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ExternalPaymentId { get; set; } = string.Empty;//получен из платежной системы
        public string Status { get; set; } = string.Empty;//pending, succeeded, canceled
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        public Payment() { }
        
    }
    public enum PaymentStatus
    {
        Pending,    // Ожидает оплаты (создан в вашей системе)
        Processing, // В процессе (пользователь на стороне шлюза)
        Succeeded,  // Успешно оплачен (получен webhook "payment.succeeded")
        Canceled,   // Отменен (получен webhook "payment.canceled" или истек срок)
        Failed      // Неуспешен (ошибка на стороне шлюза)
    }
}
