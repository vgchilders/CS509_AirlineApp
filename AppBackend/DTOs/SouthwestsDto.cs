namespace AppBackend.DTOs
{
 public class   SouthwestsDto{
 public int Id {get;set;}
    public DateTime  DepartDateTime{get;set;}
    public DateTime ArriveDateTime{get;set;}
    public string DepartAirport {get;set;}
    public string ArriveAirport {get;set;}
    public string FlightNumber {get;set;}
 }
}