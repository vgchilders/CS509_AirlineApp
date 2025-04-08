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
        public async Task<bool> HoldSeatAsync(BookeSeatRequestDto dto)
        
        {
              var isTaken = await _context.BookedSeats.AnyAsync(s =>
            s.SeatNumber == dto.SeatNumber &&
            s.FlightType == dto.FlightType &&
            ((dto.FlightType == FlightTypes.Direct &&
              s.FlightId == dto.FlightId && s.FlightSource == dto.FlightSource) ||
             (dto.FlightType == FlightTypes.Connecting &&
              ((s.FlightId == dto.Flight1Id && s.FlightSource == dto.Flight1Sournce) ||
               (s.FlightId == dto.Flight2Id && s.FlightSource == dto.Flight2Source)))) &&
            (s.IsConfirmed || s.HoldExpiresAt > DateTime.UtcNow));

        if (isTaken) return false;

        var expiration = DateTime.UtcNow.AddMinutes(10);

        if (dto.FlightType == FlightTypes.Direct && dto.FlightId.HasValue && dto.FlightSource != null)
        {
            _context.BookedSeats.Add(new BookedSeat
            {
                FlightId = dto.FlightId.Value,
                FlightSource=dto.FlightSource,
                FlightType = FlightTypes.Direct,
                SeatNumber = dto.SeatNumber,
                IsConfirmed = false,
                HoldExpiresAt = expiration,
                SessionId = dto.SessionId,
               
            });
        }
        else if (dto.FlightType == FlightTypes.Connecting && dto.Flight1Id.HasValue && dto.Flight2Id.HasValue
        && dto.Flight1Sournce !=null && dto.Flight2Source != null)
        {
            _context.BookedSeats.AddRange(new[]
            {
                new BookedSeat
                {
                    FlightId = dto.Flight1Id.Value,
                    FlightSource=dto.Flight1Sournce,
                    FlightType = FlightTypes.Connecting,
                    SeatNumber = dto.SeatNumber,
                    IsConfirmed = false,
                    HoldExpiresAt = expiration,
                    SessionId = dto.SessionId,
                    Leg = 1
                },
                new BookedSeat
                {
                    FlightId = dto.Flight2Id.Value,
                    FlightSource=dto.Flight2Source,
                    FlightType = FlightTypes.Connecting,
                    SeatNumber = dto.SeatNumber,
                    IsConfirmed = false,
                    HoldExpiresAt = expiration,
                    SessionId = dto.SessionId,
                    Leg = 2
                }
            });
        }
        else return false;
              
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<List<string>> GetAvailableSeatsForConnectedFlightAsync(int flight1Id, int flight2Id,string source1,string source2)
        {   
             var heldSeats = await _context.BookedSeats
            .Where(s => s.FlightType == FlightTypes.Connecting &&
                        ((s.FlightId == flight1Id && s.FlightSource == source1) ||
                         (s.FlightId == flight2Id && s.FlightSource == source2)) &&
                        (s.IsConfirmed || s.HoldExpiresAt > DateTime.UtcNow))
            .Select(s => s.SeatNumber)
            .ToListAsync();

        return SeatService.GenerateSeatMap().Except(heldSeats).ToList();
        
        }

        public async Task<List<string>> GetAvailableSeatsForDirectFlightAsync(int flightId,string source)
        {
                                             
           var heldSeats = await _context.BookedSeats
            .Where(s => s.FlightType == FlightTypes.Direct &&
                        s.FlightId == flightId &&
                        s.FlightSource == source &&
                        (s.IsConfirmed || s.HoldExpiresAt > DateTime.UtcNow))
            .Select(s => s.SeatNumber)
            .ToListAsync();

        return SeatService.GenerateSeatMap().Except(heldSeats).ToList();
        }



    }
}