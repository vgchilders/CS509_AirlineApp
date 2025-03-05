using AppBackend.Data;
using AppBackend.DTOs;
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
    private readonly AppDbContext _context; 
    private readonly IMapper _mapper;

    public CombinedFlightsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    } 

    [HttpGet("search")]
    public async Task<ActionResult<FlightSearchRespones>> SearchFlights(
        string departAirport, 
        string arriveAirport, 
        DateTime departureDate, 
        DateTime? returnDate = null)
    {
        //  Fetch direct one-way flights
        var directDepartFlights = await _context.CombineFlights
            .Where(f => f.DepartAirport.Contains(departAirport) 
                        && f.ArriveAirport.Contains(arriveAirport) 
                        && f.DepartDateTime.Date == departureDate.Date)
            .ToListAsync();

        var directDepartFlightDtos = _mapper.Map<IEnumerable<CombinedFlightDto>>(directDepartFlights);

        //  Fetch connecting one-way flights
        var connectingDepartFlights = await _context.ConnectingFlights
            .Where(f => f.Flight1_DepartAirport.Contains(departAirport)
                        && f.Flight2_ArriveAirport.Contains(arriveAirport)
                        && f.Flight1_DepartureDateTime.Date == departureDate.Date)
            .ToListAsync();

        var connectingDepartFlightDtos = _mapper.Map<IEnumerable<ConnectingFlightDto>>(connectingDepartFlights);

        //  Combine direct & connecting flights
        var response = new FlightSearchRespones{
         DirectDepartFlights=directDepartFlightDtos,
         ConnectingDepartFlights=connectingDepartFlightDtos
        
        };
        

        //  Fetch round-trip flights if `returnDate` is provided
        var returnFlights = new List<object>();

        if (returnDate.HasValue)
        {
            var directReturnFlights = await _context.CombineFlights
                .Where(f => f.DepartAirport.Contains(arriveAirport) 
                            && f.ArriveAirport.Contains(departAirport) 
                            && f.DepartDateTime.Date == returnDate.Value.Date)
                .ToListAsync();

            var directReturnFlightDtos = _mapper.Map<IEnumerable<CombinedFlightDto>>(directReturnFlights);

            var connectingReturnFlights = await _context.ConnectingFlights
                .Where(f => f.Flight1_DepartAirport.Contains(arriveAirport)
                            && f.Flight2_ArriveAirport.Contains(departAirport)
                            && f.Flight1_DepartureDateTime.Date == returnDate.Value.Date)
                .ToListAsync();

            var connectingReturnFlightDtos = _mapper.Map<IEnumerable<ConnectingFlightDto>>(connectingReturnFlights);
            response.DirectReturnFlights=directReturnFlightDtos;
            response.ConnectingReturnFlights=connectingReturnFlightDtos;
        }
     
      
        return Ok(response);
    }
}
