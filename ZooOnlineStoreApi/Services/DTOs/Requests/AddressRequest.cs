using System.ComponentModel;
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class AddressRequest
    {
        [DefaultValue("г. Москва, ул. Ленина, 1")]
        public string FullAddress { get; set; } = string.Empty;
        [DefaultValue(0)]
        public int UserId { get; set; }

    }
}
