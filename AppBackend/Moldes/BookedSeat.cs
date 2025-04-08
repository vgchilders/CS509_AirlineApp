using AppBackend.DTOs;

namespace AppBackend.Models
{
    public class BookedSeat
    {
        public int Id { get; set; }
        public FlightTypes FlightType { get; set; }
        public int FlightId { get; set; }
        public string FlightSource { get; set; }
        public int? Leg { get; set; }
        public string SeatNumber    { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime HoldExpiresAt { get; set;}
        public Guid SessionId { get; set; }

        
    }
}