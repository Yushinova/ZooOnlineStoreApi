
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;//Pending, Paid, Processing, Shipped, Delivered, Deleted
        public DateTime CreatedAt { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public PaymentResponse? Payment { get; set; }
        public UserOrderResponse User { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
    }
    public class OrderPaymentResponse
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
    }
}
