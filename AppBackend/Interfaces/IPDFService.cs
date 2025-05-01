using AppBackend.DTOs;
using AppBackend.Models;

namespace AppBackend.Interfaces
{

 public interface IPDFService{
   public byte[]GenerateTicketPdf(TicketBooking booking, List<SeatWithFlightDto> seatFlight);
 }

}
