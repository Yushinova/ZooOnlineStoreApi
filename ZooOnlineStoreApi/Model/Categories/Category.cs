using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Model.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public HashSet<PetType>? PetTypes { get; set; }
        public HashSet<Product>? Products { get; set; }
        public Category() { }
    }
}
