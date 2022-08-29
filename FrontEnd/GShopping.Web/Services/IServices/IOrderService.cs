using GShopping.Web.Models;

namespace GShopping.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> FindAllOrders(string? token);
        Task<OrderViewModel> FindOrderById(long id, string? token);
        Task<OrderViewModel> UpdateOrder(OrderViewModel model, string? token);
        Task<OrderViewModel> UpdateStatus(long id, string status, string? token);
        Task<OrderViewModel> SendEmail(OrderViewModel model, string? token);
        Task<bool> DeleteOrderById(long id, string? token);
    }
}
