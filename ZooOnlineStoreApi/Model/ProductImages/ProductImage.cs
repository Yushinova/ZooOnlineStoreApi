using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Model.ProductImages
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        //связи
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public ProductImage() { }
      
    }
}
