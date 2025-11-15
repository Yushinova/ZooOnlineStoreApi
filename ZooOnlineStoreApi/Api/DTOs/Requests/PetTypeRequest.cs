using System.ComponentModel.DataAnnotations;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class PetTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
    }
}
