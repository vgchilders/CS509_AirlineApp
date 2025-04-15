using AppBackend.Models;
using AppBackend.DTOs;
using System.Collections.Generic;

namespace AppBackend.Interfaces
{

 public interface ITicketBookingFlightRepository{
    Task <List<TicketBookingFlight>> AddTicketBookingFlightAsync(BookingInfoDto dto,int id);
 }

}