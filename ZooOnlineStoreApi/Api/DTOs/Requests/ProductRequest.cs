using ZooOnlineStoreApi.Api.DTOs.Responses;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }
        public string? Brand { get; set; }
        public double? Rating { get; set; }
        public bool isPromotion { get; set; } = false;
        public bool isActive { get; set; } = true;
        public int CategoryId { get; set; }
        public List<int> PetTypeIds { get; set; } = new List<int>();

    }
}
