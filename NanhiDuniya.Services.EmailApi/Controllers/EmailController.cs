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
          
            if (!ModelState.IsValid)
            {
                //return BadRequest(new ApiResponse<object>(StatusCodes.Status400BadRequest));
            }

            bool result = await _emailService.SendEmailAsync(emailRequest);

                return Ok(new ApiResponse<object>(true, "Email sent successfully.",null,StatusCodes.Status200OK,null));
            
            

        }

    }
}
