
namespace ZooOnlineStoreApi.Services.DTOs.Responses
{
    public class PetTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public List<CategoryResponse>? Categories { get; set; }
    }
    public class PetTypeShortResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
