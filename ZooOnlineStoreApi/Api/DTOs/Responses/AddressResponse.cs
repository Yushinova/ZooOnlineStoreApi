using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class AddressResponse
    {
        public int Id { get; set; }
        public string FullAddress { get; set; } = string.Empty;
        public int UserId { get; set; }

    }
}
