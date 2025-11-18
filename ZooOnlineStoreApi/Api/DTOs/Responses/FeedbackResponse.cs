using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class FeedbackResponse
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //связи
        public int UserId { get; set; }
    }
}
