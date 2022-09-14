using System.ComponentModel.DataAnnotations;

namespace GShopping.Web.Models
{
    public class CartDetailViewModel
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public CartHeaderViewModel? CartHeader { get; set; }
        public long ProductId { get; set; }
        public ProductViewModel Product { get; set; }

        [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
        [Range(1, 999999, ErrorMessage = "O campo Quantidade deve estar entre 1 e 999999.")]
        public int Count { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
