using GShopping.EmailAPI.Email;
using GShopping.EmailAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace GShopping.EmailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {

        private readonly IMailService mailService;
        public EmailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
