namespace ZooOnlineStoreApi.Models
{
    public class User
    {
        public int Id { get; set; }
        // уникальный идентификатор пользователя, выдается системой   
        public Guid UUID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Discont { get; set; } = 0;
        public string? Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public decimal TotalOrders { get; set; } = 0;
        public HashSet<Address>? Addresses { get; set; }
        public HashSet<Feedback>? Feedbacks { get; set; }
        public HashSet<Order>? Orders { get; set; }
        public User() { }

    }
}
