using GShopping.EmailAPI.Messages;

namespace GShopping.EmailAPI.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(CheckoutMessage message);
    }
}
