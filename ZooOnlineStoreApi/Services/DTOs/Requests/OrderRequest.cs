
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class OrderRequest
    {
        public decimal ShippingCost { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//Pending, Paid, Processing, Shipped, Delivered, Deleted
        public string ShippingAddress { get; set; } = string.Empty;
        public int UserId { get; set; }
        public List<OrderItemRequest>? OrderItems { get; set; }
    }

    public class OrderUpdateRequest
    {
        public decimal ShippingCost { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//Kart, Paid, Processing, Shipped, Delivered, Deleted
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
