using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBackend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketBookingRepository _TicketRepository;
        private readonly ITicketBookingFlightRepository _ticketBookingFlightRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(
            ITicketBookingFlightRepository ticketBookingFlightRepository,
            ILogger<BookingController> logger,
            ISeatRepository seatRepo,
            ITicketBookingRepository ticketRepo,
            AppDbContext context)
        {
            _seatRepository = seatRepo;
            _TicketRepository = ticketRepo;
            _context = context;
            _ticketBookingFlightRepository = ticketBookingFlightRepository;
            _logger = logger;
        }

        [HttpGet("availableseat/direct")]
        public async Task<ActionResult<List<AvailableSeatDto>>> GetDirectSeats([FromQuery] GetDirectFlightSeatRequestDto dto)
        {
            try
            {
                var seat = await _seatRepository.GetAvailableSeatsForDirectFlightAsync(dto.flightId, dto.source, dto.direction);
                var seatDto = seat.Select(s => new AvailableSeatDto { SeatNumber = s }).ToList();
                return Ok(seatDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching direct seats for flight {FlightId}", dto.flightId);
                return StatusCode(500, "An error occurred while fetching available seats.");
            }
        }

        [HttpGet("availableseat/connecting")]
        public async Task<ActionResult<List<AvailableSeatDto>>> GetConnectingSeats([FromQuery] GetconnectingFlightSeatRequestDto dto)
        {
            try
            {
                var seats = await _seatRepository.GetAvailableSeatsForConnectedFlightAsync(dto.flight1Id, dto.flight2Id, dto.source1, dto.source2, dto.direction);
                return Ok(seats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching connecting seats for flights {Flight1Id} and {Flight2Id}", dto.flight1Id, dto.flight2Id);
                return StatusCode(500, "An error occurred while fetching connecting flight seats.");
            }
        }

        [HttpPost("selectseat/direct")]
        public async Task<IActionResult> BookDirectSeat([FromBody] BookingDirectSeatsDto dto)
        {
            try
            {
                var isHeld = await _seatRepository.HoldSeatForDirectFlightAsync(dto);
                if (!isHeld)
                {
                    _logger.LogWarning("Failed to book direct seat {Seat} for FlightId={FlightId}", dto.SeatNumber, dto.FlightId);
                    return BadRequest("Seat already booked or invalid request.");
                }

                _logger.LogInformation("Seat {Seat} held for FlightId={FlightId}", dto.SeatNumber, dto.FlightId);
                return Ok("Seat booked successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking direct seat {Seat} for FlightId={FlightId}", dto.SeatNumber, dto.FlightId);
                return StatusCode(500, "An error occurred while booking the seat.");
            }
        }

        [HttpPost("selectseat/connecting")]
        public async Task<IActionResult> BookConnectingSeat([FromBody] BookingConnectingSeatsDto dto)
        {
            try
            {
                var isHeld = await _seatRepository.HoldSeatForConnectingFlightAsync(dto);
                if (!isHeld)
                {
                    _logger.LogWarning("Failed to book connecting seats {Seat1}, {Seat2} for flights {Flight1Id}, {Flight2Id}", dto.Seat1, dto.Seat2, dto.Flight1Id, dto.Flight2Id);
                    return BadRequest("Seat already booked or invalid request.");
                }

                _logger.LogInformation("Seats {Seat1}, {Seat2} held for connecting flights {Flight1Id}, {Flight2Id}", dto.Seat1, dto.Seat2, dto.Flight1Id, dto.Flight2Id);
                return Ok("Seat booked successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking connecting seats {Seat1}, {Seat2} for flights {Flight1Id}, {Flight2Id}", dto.Seat1, dto.Seat2, dto.Flight1Id, dto.Flight2Id);
                return StatusCode(500, "An error occurred while booking connecting seats.");
            }
        }

        [HttpPost("info")]
        public async Task<IActionResult> SaveBookingInfo([FromBody] BookingInfoDto dto)
        {
            try
            {
                var ticketInfo = await _TicketRepository.AddBookingInfoAsync(dto);
                var ticketBookingFlight = await _ticketBookingFlightRepository.AddTicketBookingFlightAsync(dto, ticketInfo.Id);

                _logger.LogInformation("Booking info saved successfully for SessionId={SessionId}, Reference={Reference}", dto.SessionId, ticketInfo.BookingReference);
                return Ok(new
                {
                    message = "Booking info saved. Proceed to payment.",
                    BookingReference = ticketInfo.BookingReference
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving booking info for session {SessionId}", dto.SessionId);
                return StatusCode(500, "An error occurred while saving the booking info.");
            }
        }
    }
}
