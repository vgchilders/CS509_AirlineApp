




using AppBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.Interfaces
{
 public interface IFlightRepository{
 Task<FlightSearchResponses> SearchFlights(flightSearchDataDtos flightSearch);

 }
}
