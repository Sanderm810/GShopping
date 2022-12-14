using GShopping.CartAPI.Data.ValueObjects;
using GShopping.MessageBus;

namespace GShopping.CartAPI.Messages
{
    public class CheckoutHeaderVO: BaseMessage
    {
        public string UserId { get; set; }
        public string? CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }

        public decimal? DiscountAmount { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public string? ExpiryMothYear { get; set; }
        public int? CartTotalItems { get; set; }
        public string FullAddress { get; set; }
        public int Status { get; set; }
        public string? Observation { get; set; }
        public IEnumerable<CartDetailVO>? CartDetails { get; set; }
    }
}
