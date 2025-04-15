using System.Data;
using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;

namespace AppBackend.Repositories
{
    public class TicketBookingRepository : ITicketBookingRepository
    {  
        private readonly AppDbContext _context;
        public TicketBookingRepository(AppDbContext context){
            _context = context;
        }
        public async Task<TicketBooking> AddBookingInfoAsync(BookingInfoDto bookingInfoDto)
        {
            var booking= new TicketBooking{
                SessionId=bookingInfoDto.SessionId,
                FirstName=bookingInfoDto.FirstName,
                LastName=bookingInfoDto.LastName,
                Email=bookingInfoDto.Email,
                PhoneNumber=bookingInfoDto.PhoneNumber,
                BookingTime=DateTime.UtcNow,
                IsPaid=false,
                Price=bookingInfoDto.Price,
            };
         var bookingInfo = _context.TicketBookings.Add(booking);
         await _context.SaveChangesAsync();
         return bookingInfo.Entity;

        }
    }
}