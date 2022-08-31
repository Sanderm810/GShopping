using GShopping.Web.Models;

namespace GShopping.Web.Services.IServices
{
    public interface IEmailService
    {
        Task Send(OrderViewModel model, string token);
    }
}
