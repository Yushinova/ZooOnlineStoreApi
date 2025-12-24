
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Discont { get; set; } = 0;
        public string? Email { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public decimal TotalOrders { get; set; } = 0;
    }
    public class UserOrderResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
    public class UserFeedbackResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class UserAuthResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Discont { get; set; } = 0;
        public string? Email { get; set; }
        public double TotalOrders { get; set; } = 0;
    }
}
