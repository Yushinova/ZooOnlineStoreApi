using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Model.PetTypes
{
    public class PetType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public HashSet<Category>? Categories { get; set; }
        public HashSet<Product>? Products { get; set; }
        public PetType() { }

    }
}
