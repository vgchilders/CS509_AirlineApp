using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;

namespace AppBackend.Repositories
{
    public class TicketBookingFlightRepository : ITicketBookingFlightRepository
    {   
        private readonly AppDbContext _context;
       

        public TicketBookingFlightRepository(AppDbContext context){
            _context = context;
            
        }
        public async  Task<List <TicketBookingFlight>> AddTicketBookingFlightAsync(BookingInfoDto dto,int bookingId)
        {
           var bookingFlight = dto.Flights.Select(f => new TicketBookingFlight
           {
            FlightId=f.FlightId,
            FlightSource=f.FlightSource,
            Direction=f.Direction, //"outbound" or "return"
            TicketBookingId=bookingId
           }).ToList();
           _context.TicketBookingFlights.AddRange(bookingFlight);
           await _context.SaveChangesAsync();
           return bookingFlight;
    
        }
    }
}