using GShopping.EmailAPI.Messages;

namespace GShopping.EmailAPI.Repository
{
    public interface IEmailRepository
    {
        Task LogEmail (CheckoutMessage message);
    }
}
