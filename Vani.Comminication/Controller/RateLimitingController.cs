using Microsoft.AspNetCore.Mvc;
using Vani.Comminication.Service;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vani.Comminication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateLimitingController : ControllerBase
    {
        private readonly IRateLimiterService _rateLimiter;

        public RateLimitingController(IRateLimiterService rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        [HttpGet("can-send")]
        public async Task<IActionResult> CanSend(string phoneNumber)
        {
            var canSend = await _rateLimiter.CanSendFromNumber(phoneNumber);
            
            return Ok(new { canSend });
        }
    }
}
