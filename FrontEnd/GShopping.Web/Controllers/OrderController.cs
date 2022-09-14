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
        private readonly IEmailService _emailService;

        public OrderController(IOrderService orderService, IEmailService emailService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<IActionResult> OrderIndex(string? message = null)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var orders = await _orderService.FindAllOrders(token);
            if (message != null)
            {
                ViewData["AlertInfo"] = message;
            }
            return View(orders);
        }

        public async Task<IActionResult> OrderUpdate(long id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _orderService.FindOrderById(id, token);
            if (model != null) return View(model);
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

        [Authorize]
        [ActionName("SendEmail")]
        public async Task<IActionResult> SendEmail(long id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _orderService.FindOrderById(id, token);
            try
            {
                await _emailService.Send(model, token);
                return RedirectToAction(nameof(OrderIndex), new { message = "Email enviado com sucesso!" });
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(OrderIndex), new { message = "Problema ao tentar enviado email!" });
            }
        }
    }
}
