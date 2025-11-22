using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//Kart, Paid, Processing, Shipped, Delivered, Deleted
        public DateTime CreatedAt { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public UserOrderResponse User { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
    }
}
