namespace AppBackend.Models
{
 public class TicketBookingFlight
 {
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string FlightSource { get; set;}
    public string Direction { get; set; }  //"outbound" or "return"
    public int TicketBookingId { get; set; }
    public TicketBooking TicketBooking { get; set; }
 }   
}