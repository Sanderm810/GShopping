using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GShopping.Web.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Name { get; set; }

        [Range(0.1, 9999)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Required(ErrorMessage = "Preço é obrigatório.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatório.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatório.")]
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; } = "Caminho da Imagem";
        public IFormFile? FormFile { get; set; }

        [Range(1, 100)]
        public int Count { get; set; } = 1;

        public string SubstringName()
        {
            if (Name.Length < 24) return Name;
            return $"{Name.Substring(0, 21)} ...";
        }

        public string SubstringDescription()
        {
            if (Description.Length < 355) return Description;
            return $"{Description.Substring(0, 352)} ...";
        }

    }
}
