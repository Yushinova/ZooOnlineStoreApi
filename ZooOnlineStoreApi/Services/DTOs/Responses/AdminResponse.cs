namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class AdminResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Role { get; set; } = "admin";
    }
}
