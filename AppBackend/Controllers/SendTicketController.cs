using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppBackend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SendTicketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPDFService _pdfService;
        private readonly IBookingService _bookingService;
        private readonly IEMailService _emailService;
        private readonly ILogger<SendTicketController> _logger;

        public SendTicketController(
            AppDbContext context,
            IPDFService pdfService,
            IEMailService emailService,
            IBookingService bookingService,
            ILogger<SendTicketController> logger)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("send-ticket")]
        public async Task<IActionResult> SendTicketAsPdf([FromBody] ConfirmBookingRequestDto request)
        {
            _logger.LogInformation("SendTicket request received for email: {Email}, last name: {LastName}", request.Email, request.LastName);

            try
            {
                var booking = await _context.TicketBookings
                    .Include(b => b.Flights)
                    .FirstOrDefaultAsync(b =>
                        b.Email.ToLower() == request.Email.ToLower() &&
                        b.LastName.ToLower() == request.LastName.ToLower() &&
                        b.ConfirmationCode == request.ConfirmationCode);

                if (booking == null || !booking.IsPaid)
                {
                    _logger.LogWarning("Booking not found or not paid. Email: {Email}, LastName: {LastName}", request.Email, request.LastName);
                    return NotFound("Booking not found or not paid.");
                }

                _logger.LogInformation("Booking found. Generating PDF for BookingReference={Reference}", booking.BookingReference);

                var enrichedSeats = await _bookingService.GetEnrichedSeatAsync(booking.Id);
                var pdfBytes = _pdfService.GenerateTicketPdf(booking, enrichedSeats);

                await _emailService.SendEmail(
                    booking.Email,
                    "Your Flight Ticket",
                    "Attached is your flight ticket. Please bring it with you to board.",
                    pdfBytes,
                    $"{booking.FirstName}-{booking.LastName}.pdf"
                );

                _logger.LogInformation("Ticket email sent to {Email} for BookingReference={Reference}", booking.Email, booking.BookingReference);

                return Ok(new { message = "Ticket sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending ticket for email: {Email}, last name: {LastName}", request.Email, request.LastName);
                return StatusCode(500, "An internal server error occurred while sending the ticket.");
            }
        }
    }
}
