using DataAccess.Models.PushNotification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Push_Service.Interface;
using Push_Service.Model;
using Push_Service.Services;
using WebPush;

namespace PushNotification_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPushinitialize _pushInital;
        private readonly IPushNotificationService _pushService;
        private readonly IPushNotificationQueue _queue;
        private readonly SubcriptionContext _context;
        public SubscriptionsController(SubcriptionContext context, IConfiguration configuration, IPushinitialize pushinitialize, IPushNotificationQueue queue)
        {
            _queue = queue;
            _context = context;
            _configuration = configuration;
            _pushInital = pushinitialize;
            _pushService = new PushNotificationService(_context, _queue);
        }

        [HttpGet("public-key")]
        public async Task<ContentResult> Get()
        {
            try
            {
                var data = await _pushInital.GetPublicKey();
                if (data == null)
                {
                    return Content(null);
                }
                return Content(data, "text/plain");
            }
            catch
            {
                return Content(null);
            }
        }

        [HttpPost("subscription")]
        public async Task<IActionResult> PostSubscription([FromBody] PushSubscriptVM subscription)
        {
            try
            {
                await _pushService.Subsctiption(subscription);
                return NoContent();
            }
            catch
            {

                return BadRequest();
            }
        }

        [HttpPost("notification")]
        public async Task<IActionResult> SentNotification([FromBody] SentNotificationVM message)
        {
            try
            {
                await _queue.SentNotification(message);
                return NoContent();
            }
            catch
            {

                return BadRequest();
            }

        }

        [HttpDelete("subscriptions")]
        public async Task<IActionResult> DiscardSubscription(string endpoint)
        {
            await _pushService.DiscardSubscriptionAsync(endpoint);

            return NoContent();
        }
    }
}
