using System.ComponentModel.DataAnnotations.Schema;

namespace ZooOnlineStoreApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        //связи
        public int OrderId { get; set; }

        [ForeignKey (nameof(OrderId))]
        public Order? Order { get; set; }
        public OrderItem() { }

    }
}
