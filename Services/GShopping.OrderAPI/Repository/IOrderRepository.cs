using GShopping.OrderAPI.Model;

namespace GShopping.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder (OrderHeader header);
        Task UpdateOrderPaymentStatus (long orderHeaderId, bool paid);
    }
}
