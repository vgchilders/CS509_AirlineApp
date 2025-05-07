using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using AppBackend.util;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Repositories
{
    public class SeatRepository : ISeatRepository
    {   private readonly AppDbContext _context;

        public  SeatRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task ExprireStaleSeatHoldsAsync()
        {

            var expired= await _context.BookedSeats.Where(s => !s.IsConfirmed &&s.HoldExpiresAt<DateTime.UtcNow)
            .ToListAsync();
            if(expired.Any())
            {
                _context.BookedSeats.RemoveRange(expired);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FlightSeatMapDto>> GetAvailableSeatsForConnectedFlightAsync(int flight1Id, int flight2Id,string source1,string source2,string direction)
        {


            var seatFlight1=await GetAvailableSeatsForDirectFlightAsync(flight1Id,source1,direction);
            var seatFlight2=await GetAvailableSeatsForDirectFlightAsync(flight2Id,source2,direction);

            return new List<FlightSeatMapDto>
            {
                new FlightSeatMapDto {
                    FlightId=flight1Id,
                    FlightSource=source1,
                    Direction=direction,
                    Leg=1,
                    AvailableSeats=seatFlight1,

                },
                new FlightSeatMapDto {
                    FlightId = flight2Id,
                    FlightSource = source2,
                    Direction = direction,
                    Leg = 2,
                    AvailableSeats =seatFlight2
                }
            };

        //      var heldSeats = await _context.BookedSeats
        //     .Where(s => s.FlightType == FlightTypes.Connecting &&
        //                   s.Direction == direction &&
        //                 ((s.FlightId == flight1Id && s.FlightSource == source1) ||
        //                  (s.FlightId == flight2Id && s.FlightSource == source2)) &&
        //                 (s.IsConfirmed || s.HoldExpiresAt > DateTime.UtcNow))
        //     .Select(s => s.SeatNumber)
        //     .ToListAsync();

        // return SeatService.GenerateSeatMap().Except(heldSeats).ToList();

        }

        public async Task<List<string>> GetAvailableSeatsForDirectFlightAsync(int flightId,string source,string direction)
        {

           var heldSeats = await _context.BookedSeats
            .Where(s => s.FlightType == FlightTypes.Direct &&
                        s.FlightId == flightId &&
                        s.FlightSource == source &&
                        s.Direction==direction &&
                        (s.IsConfirmed || s.HoldExpiresAt > DateTime.UtcNow))
            .Select(s => s.SeatNumber)
            .ToListAsync();

        return SeatService.GenerateSeatMap().Except(heldSeats).ToList();
        }

    public async Task<bool> ConfirmseatsAsync(Guid sessionId,int bookingId){
        var  HeldSeats= await _context.BookedSeats.Where
        (s=>s.SessionId==sessionId && !s.IsConfirmed && s.HoldExpiresAt> DateTime.UtcNow).ToListAsync();
        if(!HeldSeats.Any()){
            return false;
        }
        foreach(var seat in HeldSeats)
        {
            seat.IsConfirmed=true;
            seat.TicketBookingId=bookingId;
        }
        await _context.SaveChangesAsync();
        return true;
    }

        public async Task<bool> HoldSeatForDirectFlightAsync(BookingDirectSeatsDto dto)
{
    var existing = await _context.BookedSeats
        .Where(s => s.FlightId == dto.FlightId
                 && s.FlightSource == dto.FlightSource
                 && s.SeatNumber == dto.SeatNumber
                 && s.Direction == dto.Direction
                 && (s.IsConfirmed || s.HoldExpiresAt > DateTime.UtcNow))
        .FirstOrDefaultAsync();

    if (existing != null)
        return false;

    var expiration = DateTime.UtcNow.AddMinutes(10);

    _context.BookedSeats.Add(new BookedSeat
    {
        FlightId = dto.FlightId,
        FlightSource = dto.FlightSource,
        FlightType = FlightTypes.Direct,
        SeatNumber = dto.SeatNumber,
        IsConfirmed = false,
        HoldExpiresAt = expiration,
        SessionId = dto.SessionId,
        Direction = dto.Direction
    });

    await _context.SaveChangesAsync();
    return true;
}


       public async Task<bool> HoldSeatForConnectingFlightAsync(BookingConnectingSeatsDto dto)
{
    var now = DateTime.UtcNow;

    var seat1Exists = await _context.BookedSeats
        .AnyAsync(s => s.FlightId == dto.Flight1Id
                    && s.FlightSource == dto.Flight1Sournce
                    && s.SeatNumber == dto.Seat1
                    && s.Direction == dto.Direction
                    && (s.IsConfirmed || s.HoldExpiresAt > now));

    var seat2Exists = await _context.BookedSeats
        .AnyAsync(s => s.FlightId == dto.Flight2Id
                    && s.FlightSource == dto.Flight2Source
                    && s.SeatNumber == dto.Seat2
                    && s.Direction == dto.Direction
                    && (s.IsConfirmed || s.HoldExpiresAt > now));

    if (seat1Exists || seat2Exists)
        return false;

    var expiration = now.AddMinutes(10);

    _context.BookedSeats.AddRange(new[]
    {
        new BookedSeat
        {
            FlightId = dto.Flight1Id,
            FlightSource = dto.Flight1Sournce,
            FlightType = FlightTypes.Connecting,
            SeatNumber = dto.Seat1,
            IsConfirmed = false,
            HoldExpiresAt = expiration,
            SessionId = dto.SessionId,
            Direction = dto.Direction,
            Leg = 1
        },
        new BookedSeat
        {
            FlightId = dto.Flight2Id,
            FlightSource = dto.Flight2Source,
            FlightType = FlightTypes.Connecting,
            SeatNumber = dto.Seat2,
            IsConfirmed = false,
            HoldExpiresAt = expiration,
            SessionId = dto.SessionId,
            Direction = dto.Direction,
            Leg = 2
        }
    });

    await _context.SaveChangesAsync();
    return true;
}



    }
}
