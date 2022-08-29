using GShopping.Web.Models;
using GShopping.Web.Services.IServices;
using GShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GShopping.Web.Services
{
    public class OrderService : IOrderService
    {

        private readonly HttpClient _client;
        private const string BasePath = "api/v1/order";

        public OrderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<OrderViewModel>> FindAllOrders(string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.GetAsync($"{BasePath}/all-orders");
            return await respose.ReadContentAs<List<OrderViewModel>>();
        }

        public Task<OrderViewModel> FindOrderById(long id, string? token)
        {
            throw new NotImplementedException();
        }

        public Task<OrderViewModel> UpdateOrder(OrderViewModel model, string? token)
        {
            throw new NotImplementedException();
        }

        public Task<OrderViewModel> SendEmail(OrderViewModel model, string? token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrderById(long id, string? token)
        {
            throw new NotImplementedException();
        }

        public Task<OrderViewModel> UpdateStatus(long id, string status, string? token)
        {
            throw new NotImplementedException();
        }
    }
}
