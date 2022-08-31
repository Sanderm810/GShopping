using GShopping.EmailAPI.Email;
using GShopping.EmailAPI.Messages;
using Microsoft.AspNetCore.Mvc;

namespace GShopping.EmailAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {

        private readonly IMailService mailService;
        public EmailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] CheckoutMessage model)
        {
            try
            {
                await mailService.SendEmailAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
