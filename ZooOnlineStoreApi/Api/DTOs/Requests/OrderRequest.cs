using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class OrderRequest
    {
        public decimal ShippingCost { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//Kart, Paid, Processing, Shipped, Delivered, Deleted
        public string ShippingAddress { get; set; } = string.Empty;
        public int UserId { get; set; }
        public List<OrderItemRequest>? OrderItems { get; set; }
    }
}
