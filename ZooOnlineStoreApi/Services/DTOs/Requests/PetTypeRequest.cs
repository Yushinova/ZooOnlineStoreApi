
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class PetTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
    }

    public class PetTypeUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public List<int> CategoriesIds { get; set; } = new List<int>();
    }
}
