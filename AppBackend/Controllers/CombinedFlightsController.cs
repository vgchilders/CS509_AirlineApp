using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/v1/[controller]")]
public class CombinedFlightsController : ControllerBase
{
 
   private readonly IFlightRepository _flightRepository;
  
    public CombinedFlightsController(IFlightRepository flightRepository)

    {  
       _flightRepository=flightRepository;
    } 

    [HttpGet("search")]
    public async Task<ActionResult<FlightSearchResponses>> SearchFlights(
        string departAirport, 
        string arriveAirport, 
        DateTime departureDate, 
        DateTime? returnDate = null)
    {
        var searchDto=new flightSearchDataDtos{
              ArrivalAirport=arriveAirport,
              DepartAirport=departAirport,
              Departuredate=departureDate,
              ReturnDate=returnDate,
        };
        var response=await _flightRepository.SearchFlights(searchDto);
   
      return Ok(response);
    }
}
