using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class AddressResponse
    {
        public int Id { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Home { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int UserId { get; set; }

    }
}
