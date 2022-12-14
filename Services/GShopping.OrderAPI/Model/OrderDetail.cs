using GShopping.OrderAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GShopping.OrderAPI.Model
{
    [Table("order_detail")]
    public class OrderDetail : BaseEntity
    {
        public long OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader? OrderHeader { get; set; }
        public long OrderProductId { get; set; }

        [ForeignKey("OrderProductId")]
        public virtual OrderProduct? OrderProduct { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

    }
}
