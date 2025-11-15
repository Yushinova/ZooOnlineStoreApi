using System.ComponentModel.DataAnnotations.Schema;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Model.OrderItems
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        //связи
        public int OrderId { get; set; }

        [ForeignKey (nameof(OrderId))]
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey (nameof(ProductId))]
        public Product? Product { get; set; }
        public OrderItem() { }

    }
}
