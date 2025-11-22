using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.ProductImages;

namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool isPromotion { get; set; } = false;
        public bool isActive { get; set; } = true;
        public int CategoryId { get; set; }
        public List<PetTypeShortResponse>? PetTypes { get; set; }
        public List<ProductImageResponse>? ProductImages { get; set; }
    }
}
