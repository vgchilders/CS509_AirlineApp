using AppBackend.DTOs;
using AppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ITicketBookingRepository _ticketBookingRepository;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ITicketBookingRepository ticketBookingRepository,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _ticketBookingRepository = ticketBookingRepository;
            _logger = logger;
        }

        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession([FromBody] CreateStripeSessionRequestDto request)
        {
            try
            {
                if (!Guid.TryParse(request.SessionId, out var sessionGuid))
                {
                    _logger.LogWarning("Invalid session ID: {SessionId}", request.SessionId);
                    return BadRequest("Invalid session ID.");
                }

                var booking = await _ticketBookingRepository.GetBookingBySessionAndReferenceAsync(sessionGuid, request.BookingReference);
                if (booking == null)
                {
                    _logger.LogWarning("Booking not found for SessionId={SessionId}, Reference={BookingReference}", request.SessionId, request.BookingReference);
                    return NotFound("Booking not found.");
                }

                _logger.LogInformation("Creating Stripe session for BookingReference={BookingReference}, Amount={Price}", booking.BookingReference, booking.Price);

                var url = await _paymentService.CreateCheckoutSessionAsync(
                    booking.Price,
                    booking.SessionId.ToString(),
                    "http://localhost:5173/success?session_id={CHECKOUT_SESSION_ID}",
                    "http://localhost:5173/cancel",
                    booking.BookingReference!
                );

                _logger.LogInformation("Stripe session created for BookingReference={BookingReference}: {Url}", booking.BookingReference, url);

                return Ok(new { url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Stripe session: {Message}", ex.Message);
                return StatusCode(500, "An internal server error occurred while creating the payment session.");
            }
        }
    }
}
