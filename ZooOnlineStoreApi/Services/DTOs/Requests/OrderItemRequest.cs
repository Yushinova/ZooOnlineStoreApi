
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class OrderItemRequest
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int OrderId { get; set; }

    }
}
