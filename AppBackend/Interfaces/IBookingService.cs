using AppBackend.DTOs;
using AppBackend.Models;

namespace AppBackend.Interfaces
{
 public interface IBookingService
 {
    Task SendConfirmationEmailAsync(TicketBooking booking);
    Task <List<SeatWithFlightDto>> GetEnrichedSeatAsync(int bookingId);
 }
}
