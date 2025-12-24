using System.ComponentModel.DataAnnotations.Schema;

namespace ZooOnlineStoreApi.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string FullAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //связи
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public Address() { }
        
    }
}
