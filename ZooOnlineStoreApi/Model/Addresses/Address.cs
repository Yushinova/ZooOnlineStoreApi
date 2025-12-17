using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Model.Addresses
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
