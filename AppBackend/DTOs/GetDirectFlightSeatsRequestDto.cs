namespace AppBackend.DTOs
{
 public class GetDirectFlightSeatRequestDto{
    public int flightId {get;set;}
    public string source {get;set;}
    public string direction {get;set;}
 }

public class GetconnectingFlightSeatRequestDto{
 public int flight1Id {get;set;}
 public int flight2Id  {get;set;}
 public string source1 {get;set;}
 public string source2 {get;set;}
 public string direction {get;set;}
 }

}
