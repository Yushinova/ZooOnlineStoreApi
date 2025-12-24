namespace ZooOnlineStoreApi.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "admin";
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public Admin() { }
    }
}
