namespace AppBackend.DTOs
{
   public class BookeSeatRequestDto
   {
    public FlightTypes FlightType { get; set; } 
    public int? FlightId { get; set; }
    public int? Flight1Id { get; set; }
    public int? Flight2Id { get; set; }
    public string? FlightSource { get; set; }
    public string? Flight1Sournce { get; set; }
    public string? Flight2Source { get; set; }
    public string SeatNumber { get; set; }
    public Guid SessionId   {get; set; } 

   }   
public class AvailableSeatDto
{
    public string SeatNumber { get; set; }
}
  
  public enum FlightTypes{
    Direct=1,
    Connecting=2

  }

  

}