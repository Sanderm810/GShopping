using GShopping.Web.Models;
using GShopping.Web.Services.IServices;
using GShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GShopping.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        public async Task<IActionResult> OrderIndex()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var orders = await _orderService.FindAllOrders(token);
            return View(orders);
        }

        public async Task<IActionResult> OrderUpdate(long id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _orderService.FindOrderById(id, token);
            if(model != null) return View(model);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> OrderUpdate(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var response = await _orderService.UpdateOrder(model, token);
                if (response != null) return RedirectToAction(nameof(OrderIndex));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> OrderDelete(long id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _orderService.FindOrderById(id, token);
            if (model != null) return View(model);
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> OrderDelete(OrderViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _orderService.DeleteOrderById(model.CartHeader.Id, token);
            if (response) return RedirectToAction(nameof(OrderIndex));
            return View(model);
        }
    }
}
