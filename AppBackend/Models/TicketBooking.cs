

namespace AppBackend.Models
{
  public class TicketBooking
  {
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public string Email { get; set; }
    public  string PhoneNumber { get; set; }    
    public DateTime BookingTime { get; set; }=DateTime.UtcNow;
    public bool IsPaid  { get; set; }=false;
    public  double Price { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
   


    public ICollection<TicketBookingFlight> Flights { get; set; } 
  }   
}