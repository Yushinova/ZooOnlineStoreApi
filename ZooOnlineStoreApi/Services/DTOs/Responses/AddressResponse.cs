
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class AddressResponse
    {
        public int Id { get; set; }
        public string FullAddress { get; set; } = string.Empty;
        public int UserId { get; set; }

    }
}
