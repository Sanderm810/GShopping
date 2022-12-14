using GShopping.Web.Models;
using GShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GShopping.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CartController(IProductService productService, ICartService cartService, IWebHostEnvironment hostingEnvironment)
        {
            _productService = productService;
            _cartService = cartService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await FindUserCart());
        }
        
        public async Task<IActionResult> Remove(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            var response = await _cartService.RemoveFromCart(id, token);

            if (response)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View(await FindUserCart());
        } 
        
        [HttpPost]
        public async Task<IActionResult> Checkout(CartViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var response = await _cartService.Checkout(model.CartHeader, token);

            if (response != null)
            {
                return RedirectToAction(nameof(Confirmation));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        private async Task<CartViewModel> FindUserCart()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            var response = await _cartService.FindCartByUserId(userId, token);

            if (response?.CartHeader != null)
            {
                foreach (var detail in response.CartDetails)
                {
                    response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);
                }
            }
            return response;
        }

        [HttpGet]
        public JsonResult AutoComplete(string prefix)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;

            var fileContent = System.IO.File.ReadAllText(rootPath + "/_cidades.json");

            var estadosView = JsonSerializer.Deserialize<EstadosView>(fileContent,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

            List<string>? cidades = estadosView?.Estados.SelectMany(x =>
            {
                return x.Cidades.Select(c => c + " - " + x.Sigla).ToList();
            }).ToList();

            return Json(cidades);
        }
    }
}
