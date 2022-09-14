using GShopping.OrderAPI.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GShopping.OrderAPI.Model
{
    [Table("order_product")]
    public class OrderProduct
    {
        [Key]
        [Column("key")]
        public long Key { get; set; }

        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("price")]
        [Required]
        [Range(0.1, 10000)]
        public decimal Price { get; set; }

        [Column("Description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Column("Category_name")]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [Column("image_url", TypeName = "longtext")]
        public string ImageUrl { get; set; }

        public static explicit operator OrderProduct(ProductVO v)
        {
            throw new NotImplementedException();
        }
    }
}
