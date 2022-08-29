namespace GShopping.Web.Models
{
    public class OrderViewModel
    {
        public CartHeaderViewModel CartHeader { get; set; }
        public IEnumerable<CartDetailViewModel> CartDetails { get; set; }
    }
}
