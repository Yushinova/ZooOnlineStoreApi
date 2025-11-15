using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class ProductImageRequest
    {
        public string ImageName { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public int ProductId { get; set; }
      
    }
}
