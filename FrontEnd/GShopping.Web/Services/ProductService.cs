using GShopping.Web.Models;
using GShopping.Web.Services.IServices;
using GShopping.Web.Utils;

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

        public async Task<IEnumerable<ProductModel>> FindAllProducts()
        {
            var respose = await _client.GetAsync(BasePath);
            return await respose.ReadContentAs<List<ProductModel>>();
        }

        public async Task<ProductModel> FindProductById(long id)
        {
            var respose = await _client.GetAsync($"{BasePath}/{id}");
            return await respose.ReadContentAs<ProductModel>();
        }

        public async Task<ProductModel> CreateProduct(ProductModel model)
        {
            var respose = await _client.PostAsJson(BasePath, model);
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<ProductModel>();
            else throw new Exception("Something went wrong when calling API");
        }
       
        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            var respose = await _client.PutAsJson(BasePath, model);
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<ProductModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> DeleteProductById(long id)
        {
            var respose = await _client.DeleteAsync($"{BasePath}/{id}");
            if (respose.IsSuccessStatusCode)
                return await respose.ReadContentAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }

    }
}
