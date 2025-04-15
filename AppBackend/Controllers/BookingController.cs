using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Controllers{


[ApiController]
[Route("api/v1/[controller]")]
public class BookingController: ControllerBase
{
    private readonly ISeatRepository _seatRepository;
    private readonly ITicketBookingRepository _TicketRepository;
    private readonly ITicketBookingFlightRepository _ticketBookingFlightRepository;
    private readonly   AppDbContext _context;
    public BookingController(ITicketBookingFlightRepository ticketBookingFlightRepository,ISeatRepository seatRepo, ITicketBookingRepository ticketRepo,AppDbContext context){
        _seatRepository = seatRepo;
        _TicketRepository= ticketRepo;
        _context=context;
        _ticketBookingFlightRepository=ticketBookingFlightRepository;

    }

    [HttpGet("availableseat/direct/{flightId}")]
    public async Task<ActionResult<List<AvailableSeatDto>>> GetDirectSeats(int flightId, [FromQuery] string source)
    {
        var seat= await _seatRepository.GetAvailableSeatsForDirectFlightAsync(flightId, source);
        var seatDto=seat.Select(s => new AvailableSeatDto {SeatNumber=s} ).ToList();
        return Ok(seatDto);
    }

    [HttpGet("availableseat/connecting")]
    public async Task <ActionResult<List<AvailableSeatDto>>> GetConnectingSeats([FromQuery]int flight1Id, [FromQuery] int flight2Id,[FromQuery] string source1, [FromQuery] string source2)
    {
        var seats=await _seatRepository.GetAvailableSeatsForConnectedFlightAsync(flight1Id, flight2Id,source1,source2);
        var seatDto=seats.Select(s=> new AvailableSeatDto {SeatNumber=s}).ToList();
        return Ok(seatDto);
    }

    [HttpPost("selectseat")]
    public async Task<IActionResult> BookSeat([FromBody] BookeSeatRequestDto dto)
    {
        var isHeld = await _seatRepository.HoldSeatAsync(dto);
        if (!isHeld)
            return BadRequest("Seat already booked or invalid request.");

        return Ok("Seat booked successfully.");
    }


    [HttpPost("info")]
    public async Task<IActionResult> SaveBookingInfo([FromBody] BookingInfoDto dto)
    {
        var ticketInfo=await _TicketRepository.AddBookingInfoAsync(dto);
        var ticketbookingflight=await _ticketBookingFlightRepository.AddTicketBookingFlightAsync(dto,ticketInfo.Id);
        return Ok( new {message ="booking info saved. Process to payment"});


    }

     [HttpPost("confirm_after-payment")]
    public async Task<IActionResult> ConfirmAfterPayment([FromBody] ConfirmBookingRequestDto request ){
        var booking= await _context.TicketBookings.FirstOrDefaultAsync(b=>b.SessionId==request.SessionId);
        if (booking==null)  
        {
            return NotFound("Booking not found");
            
        }
        var confirm=await _seatRepository.ConfirmseatsAsync(request.SessionId);
        if(!confirm){
            return BadRequest("Seats were expired or not held.");

        }
        booking.IsPaid=true;
        await _context.SaveChangesAsync();
        return Ok(new { message= "Booking Confirmed and paid",booking});
    }
    
  
}


}