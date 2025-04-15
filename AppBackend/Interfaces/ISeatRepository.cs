using AppBackend.DTOs;

namespace AppBackend.Interfaces
{
  public interface ISeatRepository
  {
    Task<List<string>> GetAvailableSeatsForDirectFlightAsync(int flightId, string source);
    Task <List<string>> GetAvailableSeatsForConnectedFlightAsync(int flight1Id,int flight2Id,string source1,string source2);
    Task<bool>HoldSeatAsync(BookeSeatRequestDto dto);
    Task ExprireStaleSeatHoldsAsync();
     Task<bool>ConfirmseatsAsync(Guid sessionId);
  }   
}