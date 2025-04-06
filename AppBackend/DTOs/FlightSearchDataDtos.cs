using System.Data;

namespace AppBackend.DTOs{
     public class flightSearchDataDtos{
     public string DepartAirport {get;set;}
     public string ArrivalAirport {get;set;}
     public DateTime Departuredate {get;set;}
     public DateTime? ReturnDate {get;set;}=null;


    }
}