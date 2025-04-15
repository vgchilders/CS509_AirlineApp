namespace AppBackend.DTOs
{
 public class BookingInfoDto{
    public Guid SessionId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public double Price { get; set; }
    public List <FlightLegDto>Flights { get; set; }

 }


public class FlightLegDto
{
   public int FlightId { get; set; }
   public string FlightSource { get; set; }
   public string Direction { get; set; }
}

}