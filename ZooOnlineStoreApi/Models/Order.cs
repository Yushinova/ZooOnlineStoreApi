using System.ComponentModel.DataAnnotations.Schema;

namespace ZooOnlineStoreApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//pending, paid, processing, shipped, delivered, deleted
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; } = string.Empty;
        //связи 
        public Payment? Payment { get; set; }//один к одному связь
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public HashSet<OrderItem>? OrderItems { get; set; }
        public Order() { }
    }
}
