
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class FeedbackResponse
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Rating { get; set; } = 0;
        public UserFeedbackResponse? User { get; set; }
        public int ProductId { get; set; }
    }
}
