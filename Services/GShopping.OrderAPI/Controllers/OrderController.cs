using GShopping.OrderAPI.Model;
using GShopping.OrderAPI.Models;
using GShopping.OrderAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GShopping.OrderAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderRepository _repository;

        public OrderController(IOrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("find-order/{id}")]
        [Authorize]
        public async Task<ActionResult<OrderViewModel>> FindById(long id)
        {
            var order = await _repository.FindOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("all-orders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> FindAll()
        {
            var orders = await _repository.FindAllOrders();
            return Ok(orders);
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<ActionResult<OrderViewModel>> Update([FromBody] OrderViewModel vo)
        {
            if (vo == null) return BadRequest();
            var order = await _repository.UpdateOrder(vo);
            return Ok(order);
        }

        [HttpPost("[action]/{id}/{status}")]
        [Authorize]
        public async Task<ActionResult<OrderHeader>> UpdateStatus(long id, string status)
        {
            var order = await _repository.FindOrderById(id);
            if (order == null) return NotFound();
            await _repository.UpdateOrderStatus(id, status);
            return Ok(order);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult<OrderHeader>> SendEmail([FromBody] OrderHeader vo)
        {
            if (vo == null) return BadRequest();
            var order = await _repository.SendEmail(vo);
            return Ok(order);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _repository.DeleteOrderById(id);
            if (!status) return BadRequest();
            return Ok(status);
        }
    }
}
