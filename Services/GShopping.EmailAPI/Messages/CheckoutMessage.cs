namespace GShopping.EmailAPI.Messages
{
    public class CheckoutMessage
    {
        public long OrderId { get; set; }
        public bool Status { get; set; }
        public string? Email { get; set; }
    }
}
