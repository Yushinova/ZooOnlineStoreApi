using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class AddressRequest
    {
        public string FullAddress { get; set; } = string.Empty;
        public int UserId { get; set; }

    }
}
