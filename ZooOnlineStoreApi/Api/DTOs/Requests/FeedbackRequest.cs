using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class FeedbackRequest
    {
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
