using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class SendTicketController:ControllerBase
    {
      private readonly AppDbContext _context;
      private readonly IPDFService _pdfService;
      private readonly IBookingService _bookingService;
      private readonly IEMailService _emailService;
      public SendTicketController(AppDbContext context, IPDFService pDFService ,IEMailService eMailService, IBookingService bookingService){
        _context=context;
        _pdfService=pDFService;
        _emailService=eMailService;
        _bookingService=bookingService;

      }

      [HttpPost("send-ticket")]
      public async Task<IActionResult>SendTicketAsPdf([FromBody]ConfirmBookingRequestDto request){
        var booking=await _context.TicketBookings.Include(b=>b.Flights).FirstOrDefaultAsync(b=>
        b.Email.ToLower()==request.Email.ToLower()
         && b.LastName.ToLower()==request.LastName.ToLower()
         && b.ConfirmationCode==request.ConfirmationCode);
        if (booking == null || !booking.IsPaid)
    {

        return NotFound("Booking not found or not paid.");
    }
        var enrichedSeats= await _bookingService.GetEnrichedSeatAsync(booking.Id);
        var pdfBytes= _pdfService.GenerateTicketPdf(booking,enrichedSeats);
         await _emailService.SendEmail(
            booking.Email,
            "Your Flight Ticket",
                "Attached is your flight ticket. Please bring it with you to board.",
                pdfBytes,
                $"{booking.FirstName}-{booking.LastName}.pdf"
         );

     return Ok(new { message = "Ticket sent successfully." });

    }


      }
    }


