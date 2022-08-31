using GShopping.Web.Messages;
using GShopping.Web.Models;
using GShopping.Web.Services.IServices;
using GShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GShopping.Web.Services
{
    public class EmailService : IEmailService
    {

        private readonly HttpClient _client;
        private const string BasePath = "api/v1/email";

        public EmailService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        private CheckoutMessage ProcessaMessage(OrderViewModel model)
        {

            return new CheckoutMessage
            {
                CardNumber = model.CartHeader.CardNumber,
                Email = model.CartHeader.Email,
                CouponCode = model.CartHeader.CouponCode,
                Phone = model.CartHeader.Phone,
                CVV = model.CartHeader.CVV,
                DateTime = model.CartHeader.DateTime,
                DiscountAmount = model.CartHeader.DiscountAmount,
                ExpiryMothYear = model.CartHeader.ExpiryMothYear,
                FirstName = model.CartHeader.FirstName,
                LastName = model.CartHeader.LastName,
                Id = model.CartHeader.Id,
                PurchaseAmount = model.CartHeader.PurchaseAmount,
                UserId = model.CartHeader.UserId,
                CartDetails = model.CartDetails.Select(item =>
                {
                    return new CartDetailVO
                    {
                        Id = item.Id,
                        Count = item.Count,
                        Product = new ProductVO
                        {
                            CategoryName = item.Product.CategoryName,
                            Description = item.Product.Description,
                            Id = item.Product.Id,
                            ImageUrl = item.Product.ImageUrl,
                            Name = item.Product.Name,
                            Price = item.Product.Price
                        }
                    };
                })
            };
        }

        public async Task Send(OrderViewModel model, string token)
        {
            var message = ProcessaMessage(model);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.PostAsJson($"{BasePath}/Send", message);
            if (respose.IsSuccessStatusCode)
                return;
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
