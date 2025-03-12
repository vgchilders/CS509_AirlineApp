namespace AppBackend.DTOs{
public class FlightSearchResponses
{
public IEnumerable<CombinedFlightDto> DirectDepartFlights{ get; set; }
public IEnumerable<ConnectingFlightDto> ConnectingDepartFlights{ get; set; }
public IEnumerable<CombinedFlightDto>DirectReturnFlights{ get; set; }
public IEnumerable<ConnectingFlightDto>ConnectingReturnFlights{get;set;}   
}

}