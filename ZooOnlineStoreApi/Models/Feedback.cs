using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ZooOnlineStoreApi.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Rating { get; set; } = 0;
        //связи
        public int UserId { get; set; }
        [ForeignKey (nameof (UserId))]
        public User? User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey (nameof (ProductId))]
        public Product? Product { get; set; }
        public Feedback() { }
       
    }
}
