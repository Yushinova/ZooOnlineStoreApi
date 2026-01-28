
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; } = 0;
        public string Brand { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public bool isPromotion { get; set; } = false;
        public bool isActive { get; set; } = true;
        public int CategoryId { get; set; }
        public List<int> PetTypeIds { get; set; } = new List<int>();

    }
}
