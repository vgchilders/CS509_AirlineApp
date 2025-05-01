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
        private readonly ISeatRepository _seatRepository;
        private readonly IBookingService _bookingService;

        public WebhookController(
            ITicketBookingRepository ticketBookingRepository,
            ILogger<WebhookController> logger,
            IConfiguration configuration,
            ISeatRepository seatRepository,
            IBookingService bookingService)
        {
            _ticketBookingRepository = ticketBookingRepository;
            _logger = logger;
            _configuration = configuration;
            _seatRepository = seatRepository;
            _bookingService=bookingService;
        }




        [HttpPost]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeSignature = Request.Headers["Stripe-Signature"];
            var webhookSecret =Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                webhookSecret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                if (session == null)
                {
                    _logger.LogError("Stripe event cast to Session failed.");
                    return BadRequest();
                }

                if (session.Metadata.TryGetValue("sessionId", out var appSessionId) &&
                    session.Metadata.TryGetValue("bookingReference", out var bookingReference))
                {
                    var sessionGuid = Guid.Parse(appSessionId);

                    var paid = await _ticketBookingRepository.MarkBookingAsPaidAsync(sessionGuid, bookingReference);
                    if (!paid)
                    {
                        _logger.LogError("Failed to mark booking as paid.");
                        return BadRequest();
                    }

                    var booking = await _ticketBookingRepository.GetBookingBySessionAndReferenceAsync(sessionGuid, bookingReference);

                    var success = await _seatRepository.ConfirmseatsAsync(sessionGuid, booking.Id);
                    if (success)
                    {
                        await _bookingService.SendConfirmationEmailAsync(booking);
                    }

                    return Ok();
                }
                else
                {
                    _logger.LogError("Session metadata missing sessionId or bookingReference.");
                    return BadRequest();
                }
            }
            else
            {
                _logger.LogInformation($"Received unexpected event type: {stripeEvent.Type}");
                return Ok(); // Always 200 for unknown event types
            }
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe webhook processing error.");
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected webhook error.");
            return StatusCode(500);
        }
    }
    }
}
