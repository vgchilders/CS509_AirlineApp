using AppBackend.DTOs;
using AppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.Controllers{


[ApiController]
[Route("api/v1/[controller]")]
public class SeatBookingController: ControllerBase
{
    private readonly ISeatRepository _seatRepository;
    public SeatBookingController(ISeatRepository seatRepo){
        _seatRepository = seatRepo;

    }

    [HttpGet("available/direct/{flightId}")]
    public async Task<ActionResult<List<AvailableSeatDto>>> GetDirectSeats(int flightId, [FromQuery] string source)
    {
        var seat= await _seatRepository.GetAvailableSeatsForDirectFlightAsync(flightId, source);
        var seatDto=seat.Select(s => new AvailableSeatDto {SeatNumber=s} ).ToList();
        return Ok(seatDto);
    }

    [HttpGet("available/connecting")]
    public async Task <ActionResult<List<AvailableSeatDto>>> GetConnectingSeats([FromQuery]int flight1Id, [FromQuery] int flight2Id,[FromQuery] string source1, [FromQuery] string source2)
    {
        var seats=await _seatRepository.GetAvailableSeatsForConnectedFlightAsync(flight1Id, flight2Id,source1,source2);
        var seatDto=seats.Select(s=> new AvailableSeatDto {SeatNumber=s}).ToList();
        return Ok(seatDto);
    }

    [HttpPost("select")]
    public async Task<IActionResult> BookSeat([FromBody] BookeSeatRequestDto dto)
    {
        var isHeld = await _seatRepository.HoldSeatAsync(dto);
        if (!isHeld)
            return BadRequest("Seat already booked or invalid request.");

        return Ok("Seat booked successfully.");
    }
    
  
}


}