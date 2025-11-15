using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Model.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//Kart, Paid, Processing, Shipped, Delivered
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //связи 
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public HashSet<OrderItem>? OrderItems { get; set; }
        public Order() { }
    }
}
