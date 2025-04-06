namespace AppBackend.DTOs{

public class CombinedFlightDto:FlightDto{
public string Flight_source { get; set; }
public double Price { get; set; }
public string Currency  { get; set; }="USD";
public TimeSpan Duration { get; set; }
}

}