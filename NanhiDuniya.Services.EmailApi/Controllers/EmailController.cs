using Microsoft.AspNetCore.Mvc;
using NanhiDuniya.Services.EmailApi.Extentions;
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
            throw new NotImplementedException();
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Validation failed."));
            }

            bool result = await _emailService.SendEmailAsync(emailRequest);
            if (result)
            {
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Email sent successfully."));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Failed to send email."));

        }

    }
}
