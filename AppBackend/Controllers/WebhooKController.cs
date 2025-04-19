using AppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;


namespace AppBackend.Controllers
{
    [Route("api/v1/[controller]")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly ITicketBookingRepository _ticketBookingRepository;
    private readonly ILogger<WebhookController> _logger;
    private readonly IConfiguration _configuration;

    public WebhookController(ITicketBookingRepository ticketBookingRepository, ILogger<WebhookController> logger, IConfiguration configuration)
    {
        _ticketBookingRepository = ticketBookingRepository;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var secret = _configuration["Stripe:WebhookSecret"]; // set this from your Stripe dashboard

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], secret);

            if (stripeEvent.Type == "checkout.session.completed")

            {
                var session = stripeEvent.Data.Object as Session;
                var sessionId = session.Metadata["sessionId"];

                var booking = await _ticketBookingRepository.GetBookingBySessionIdAsync(Guid.Parse(sessionId));
                if (booking != null)
                {
                    booking.IsPaid = true;
                    await _ticketBookingRepository.UpdateBookingAsync(booking);
                }
            }

            return Ok();
        }
        catch (StripeException e)
        {
            _logger.LogError($"Stripe webhook error: {e.Message}");
            return BadRequest();
        }
    }
}
}