using GShopping.Web.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GShopping.Web.Models
{
    public class CartHeaderViewModel
    {

        public long Id { get; set; }
        public string UserId { get; set; }
        public string? CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int Count { get; set; }

        public decimal DiscountAmount { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        public string Email { get; set; }
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public string? ExpiryMothYear { get; set; }

        [Required(ErrorMessage = "Endereço completo é obrigatório.")]
        public string FullAddress { get; set; }
        public string? Observation { get; set; }
        public int Status { get; set; } = (int)StatusPedido.processando;
        

        //public string FormatPhone()
        //{
        //    string fone = Phone[0] == 0 ? Phone.Substring(1, Phone.Length) : Phone;

        //    switch (fone.Length)
        //    {
        //        case 8:
        //            return String.Format("({0}) {1}-{2}", "00", fone.Substring(0, 4), fone.Substring(4, fone.Length - 4));
        //        case 9:
        //            return String.Format("({0}) {1}-{2}", "00", fone.Substring(0, 5), fone.Substring(5, fone.Length - 5));
        //        case 10:
        //            return String.Format("({0}) {1}-{2}", fone.Substring(0, 2), fone.Substring(2, 4), fone.Substring(6, fone.Length - 6));
        //        case 11:
        //            return String.Format("({0}) {1}-{2}", fone.Substring(0, 2), fone.Substring(2, 5), fone.Substring(7, fone.Length - 7));
        //        case 12:
        //            return String.Format("({0}) {1}-{2}", fone.Substring(0, 3), fone.Substring(3, 5), fone.Substring(8, fone.Length - 8));
        //        default:
        //            return Phone;
        //    }
        //}

        public string DateFomart()
        {
            return DateTime.ToString("dd/MM/yyyy HH:mm");
        }

        public string DisplayStatusName()
        {

            StatusPedido status = ((StatusPedido)Status);
            return status.GetType().GetMember(status.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName();
        }
    }
}
