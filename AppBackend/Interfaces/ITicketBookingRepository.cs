using AppBackend.DTOs;
using AppBackend.Models;

namespace AppBackend.Interfaces
{

 public interface ITicketBookingRepository{
Task <TicketBooking> AddBookingInfoAsync(BookingInfoDto bookingInfoDto);
Task <TicketBooking>GetBookingBySessionIdAsync(Guid sessionId);
Task UpdateBookingAsync(TicketBooking booking);

 }

}