using Microsoft.AspNetCore.Mvc;
using Vani.Comminication.Contracts;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vani.Comminication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateLimitingController(IRateLimiter rateLimiter) : ControllerBase
    {
        private readonly IRateLimiter _rateLimiter = rateLimiter;

        [HttpGet("can-send")]
        public IActionResult CanSend(string phoneNumber)
        {
            var canSend = _rateLimiter.CanSendFromNumber(phoneNumber);
            return Ok(new { canSend });
        }
    }
}
