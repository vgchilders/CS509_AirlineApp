




using AppBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.Interfaces
{
 public interface IFlightRepository{
 Task<ActionResult<FlightSearchResponses>> SearchFlights(flightSearchDataDtos flightSearch);

 }   
}