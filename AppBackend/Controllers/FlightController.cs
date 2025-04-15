// using AppBackend.Data;
// using AppBackend.DTOs;
// using AppBackend.Models;
// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;


// namespace AppBackend.Controllers
// {

// [ApiController]
// [Route("api/v1/[controller]")]

// public class GetAllFlight : ControllerBase
// {
//    private readonly AppDbContext _context;
//    private readonly IMapper _mapper;
//    public GetAllFlight(AppDbContext context,IMapper mapper){
//     _context=context;
//     _mapper=mapper;
//    }

//     [HttpGet]
//     public async Task<ActionResult> getAllFlight(int pageNumber = 1, int pageSize = 100)
// {
//     var skip = (pageNumber - 1) * pageSize;
//     var deltas = await _context.Deltas.Skip(skip).Take(pageSize).ToListAsync();
//     var southWests = await _context.SouthWests.Skip(skip).Take(pageSize).ToListAsync();

//     var deltasDtos = _mapper.Map<List<DeltasDto>>(deltas);
//     var southwestsDtos = _mapper.Map<List<SouthwestsDto>>(southWests);
//     var allflights = new AllflightDto
//     {
//         Deltas = deltasDtos,
//         Southwests = southwestsDtos
//     };
//     return Ok(allflights);
// }


// [HttpGet("specific")]
// public async Task<ActionResult<SpecificFlightDto>> GetSpecificFlight(
//     string departAirport,
//     string arriveAirport,
//     DateTime departDay,
//     DateTime arrivalDay)
// {
//     // Retrieve a flight from the Deltas table that matches the criteria
//     var deltaFlight = await _context.Deltas
//         .Where(d => d.DepartAirport.Contains(departAirport) &&
//                     d.ArriveAirport.Contains(arriveAirport) &&
//                     d.DepartDateTime.Date == departDay.Date &&
//                     d.ArriveDateTime.Date == arrivalDay.Date)
//         .FirstOrDefaultAsync();

//     // Retrieve a flight from the SouthWests table that matches the criteria
//     var southWestFlight = await _context.SouthWests
//         .Where(s => s.DepartAirport.Contains(departAirport) &&
//                     s.ArriveAirport.Contains(arriveAirport) &&
//                     s.DepartDateTime.Date == departDay.Date &&
//                     s.ArriveDateTime.Date == arrivalDay.Date)
//         .FirstOrDefaultAsync();


//     var deltaDto = deltaFlight != null ? _mapper.Map<DeltasDto>(deltaFlight) : null;
//     var southWestDto = southWestFlight != null ? _mapper.Map<SouthwestsDto>(southWestFlight) : null;

//     var specificFlight = new SpecificFlightDto
//     {
//         Deltas = deltaDto,
//         Southwests = southWestDto
//     };

//     return Ok(specificFlight);
// }


// }
// }