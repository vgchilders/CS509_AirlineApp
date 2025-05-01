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
        private readonly AppDbContext _context;
        public BookingService(IEMailService eMailService,AppDbContext context){
            _context=context;
             _emailService=eMailService;
        }

        public async Task<List<SeatWithFlightDto>> GetEnrichedSeatAsync(int bookingId)
        {
            var bookedSeats = await _context.BookedSeats
        .Where(s => s.TicketBookingId ==bookingId && s.IsConfirmed)
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
                Gate=GenerateGate.Generate()
            });
        }
    }

    return enrichedSeats;

        }

        public async Task SendConfirmationEmailAsync(TicketBooking booking)
        {
             var seatInfo= string.Join(",", booking.BookedSeats?
             .Where(bs=>bs.IsConfirmed)
             .Select(bs=>bs.SeatNumber)??Enumerable.Empty<string>());
            var subject="Your Flight Booking Confirmation";
            var body=$@"
            <p>Dear {booking.FirstName} {booking.LastName}, </p>
            <p> Your flight has been successfully booked and paid an amount of {booking.Price}.</p>
            <p><strong>Confirmation Code: </strong>{booking.ConfirmationCode}</p>
            <p>Thank you for choosing Flying with us.</p>";
            await _emailService.SendEmail(booking.Email,subject,body);
        }
    }
}
