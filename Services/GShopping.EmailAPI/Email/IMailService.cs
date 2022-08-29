using GShopping.EmailAPI.Model;

namespace GShopping.EmailAPI.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
