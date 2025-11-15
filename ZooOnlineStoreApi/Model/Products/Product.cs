using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.ProductImages;

namespace ZooOnlineStoreApi.Model.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }
        public string? Brand { get; set; }
        public bool isPromotion { get; set; } = false;
        public bool isActive { get; set; } = true;
        //связи
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        public HashSet<PetType>? PetTypes { get; set; }
        public HashSet<ProductImage>? ProductImages { get; set; }
        public HashSet<Feedback>? Feedbacks { get; set; }

        public Product() { }
    }
}
