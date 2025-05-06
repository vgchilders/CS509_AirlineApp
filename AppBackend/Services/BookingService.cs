using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using AppBackend.util;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Services
{
    public class BookingService : IBookingService
    {

        private readonly IEMailService _emailService;
        private readonly IPDFService _pdfService;
        private readonly AppDbContext _context;
        public BookingService(IEMailService eMailService, IPDFService pdfService, AppDbContext context)
        {
            _context = context;
            _emailService = eMailService;
            _pdfService = pdfService;
        }

        public async Task<List<SeatWithFlightDto>> GetEnrichedSeatAsync(int bookingId)
        {
            var bookedSeats = await _context.BookedSeats
        .Where(s => s.TicketBookingId == bookingId && s.IsConfirmed)
        .ToListAsync();

            var enrichedSeats = new List<SeatWithFlightDto>();

            foreach (var seat in bookedSeats)
            {
                object? flight = null;

                if (seat.FlightSource == "Delta")
                    flight = await _context.Deltas.FirstOrDefaultAsync(f => f.Id == seat.FlightId);
                else if (seat.FlightSource == "Southwest")
                    flight = await _context.SouthWests.FirstOrDefaultAsync(f => f.Id == seat.FlightId);

                if (flight != null)
                {
                    enrichedSeats.Add(new SeatWithFlightDto
                    {
                        SeatNumber = seat.SeatNumber,
                        Leg = seat.Leg,
                        Direction = seat.Direction,
                        FlightSource = seat.FlightSource,
                        Flight = flight,
                        Gate = GenerateGate.Generate()
                    });
                }
            }

            return enrichedSeats;

        }

        public async Task SendConfirmationEmailAsync(TicketBooking booking)
        {
            // Generate enriched seat data
            var enrichedSeats = await GetEnrichedSeatAsync(booking.Id);
            // Generate PDF ticket
            var pdfBytes = _pdfService.GenerateTicketPdf(booking, enrichedSeats);
            // Prepare email content
            var subject = "Your Flight Ticket and Confirmation";
            var body = $@"
            <p>Dear {booking.FirstName} {booking.LastName},</p>
            <p>Your flight has been successfully booked and paid for an amount of {booking.Price}.</p>
            <p><strong>Confirmation Code:</strong> {booking.ConfirmationCode}</p>
            <p>Please find your ticket attached as a PDF.</p>
            <p>Thank you for choosing to fly with us.</p>";
            // Send email with PDF attachment
            await _emailService.SendEmail(booking.Email, subject, body, pdfBytes, $"Ticket_{booking.BookingReference}.pdf");
        }
    }
}
