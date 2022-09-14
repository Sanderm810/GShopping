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

        public async Task<IList<OrderViewModel>> FindAllOrders(string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.GetAsync($"{BasePath}/all-orders");
            return await respose.ReadContentAs<List<OrderViewModel>>();
        }

        public async Task<OrderViewModel> FindOrderById(long id, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.GetAsync($"{BasePath}/find-order/{id}");
            return await respose.ReadContentAs<OrderViewModel>();
        }

        public async Task<OrderViewModel> UpdateOrder(OrderViewModel model, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.PutAsJson($"{BasePath}/Update", model);
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<OrderViewModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public Task<OrderViewModel> SendEmail(OrderViewModel model, string? token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteOrderById(long id, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.DeleteAsync($"{BasePath}/Delete/{id}");
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }

        public Task<OrderViewModel> UpdateStatus(long id, string status, string? token)
        {
            throw new NotImplementedException();
        }
    }
}
