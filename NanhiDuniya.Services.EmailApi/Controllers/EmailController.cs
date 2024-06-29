using Microsoft.AspNetCore.Mvc;
using NanhiDuniya.Services.EmailApi.Models;
using NanhiDuniya.Services.EmailApi.Services.Interfaces;

namespace NanhiDuniya.Services.EmailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("SendVerifyEmail")]

        public async Task<IActionResult> SendEmail(EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if model validation fails
            }
            try
            {
                bool result = await _emailService.SendEmailAsync(emailRequest);
                if (result)
                {
                    return Ok("Email sent successfully");
                }
                else
                {
                    return StatusCode(500, "Failed to send email"); // or return specific error message
                }
            }
            catch(Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }

    }
}
