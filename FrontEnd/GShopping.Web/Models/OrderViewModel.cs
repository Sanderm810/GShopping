namespace GShopping.Web.Models
{
    public class OrderViewModel
    {
        public CartHeaderViewModel CartHeader { get; set; }
        public IList<CartDetailViewModel> CartDetails { get; set; }
    }
}
