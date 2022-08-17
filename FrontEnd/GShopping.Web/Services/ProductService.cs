using GShopping.Web.Models;
using GShopping.Web.Services.IServices;
using GShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GShopping.Web.Services
{
    public class ProductService : IProductService
    {

        private readonly HttpClient _client;
        private const string BasePath = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<ProductViewModel>> FindAllProducts(string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.GetAsync(BasePath);
            return await respose.ReadContentAs<List<ProductViewModel>>();
        }

        public async Task<ProductViewModel> FindProductById(long id, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.GetAsync($"{BasePath}/{id}");
            return await respose.ReadContentAs<ProductViewModel>();
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel model, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.PostAsJson(BasePath, model);
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<ProductViewModel>();
            else throw new Exception("Something went wrong when calling API");
        }
       
        public async Task<ProductViewModel> UpdateProduct(ProductViewModel model, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.PutAsJson(BasePath, model);
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<ProductViewModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> DeleteProductById(long id, string? token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respose = await _client.DeleteAsync($"{BasePath}/{id}");
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }

    }
}
