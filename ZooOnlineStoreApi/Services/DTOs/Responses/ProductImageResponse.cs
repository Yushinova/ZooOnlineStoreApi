
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class ProductImageResponse
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
