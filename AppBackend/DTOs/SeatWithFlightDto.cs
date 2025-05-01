using AppBackend.Models;

namespace AppBackend.DTOs
{
 public class SeatWithFlightDto{
    public string SeatNumber { get; set; }
    public int? Leg { get; set; }          // 1 or 2 (for connecting flights)
    public string Direction { get; set; } // "inbound" or "return"
    public string FlightSource { get; set; } // "Delta" or "Southwest"
    public object Flight { get; set; }
    public string Gate {get;set;}

 }
}
