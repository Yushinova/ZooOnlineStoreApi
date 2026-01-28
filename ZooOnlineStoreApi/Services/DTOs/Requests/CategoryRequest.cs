using System.ComponentModel;

namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class CategoryRequest
    {
        [DefaultValue("Test")]
        public string Name { get; set; } = string.Empty;
    }
}
