namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class AdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Role { get; set; } = "admin";
        public string Token { get; set; } = string.Empty;
    }
}
