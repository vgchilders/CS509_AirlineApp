namespace Appbackend.Tests;
using System.Net;
using System.Net.Http.Json;
using AppBackend.DTOs;
using AppBackend.Models;
using AppBackend.Tests;
using AppBackend;
using FluentAssertions;
using Xunit;
using System.Text.Json;
using AppBackend.Data;

public class SeatBookingTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SeatBookingTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

 [Fact]
public async Task Should_Get_Available_Direct_Seats()
{
    var flightId = 1001;
    var source = "Delta";
    var direction = "outbound";

    var url = $"/api/v1/Booking/availableseat/direct?flightId={flightId}&source={source}&direction={direction}";
    var response = await _client.GetAsync(url);

    var raw = await response.Content.ReadAsStringAsync();
    // Console.WriteLine("==== RAW RESPONSE START ====");
    // Console.WriteLine(raw);
    // Console.WriteLine("==== RAW RESPONSE END ====");

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var seatList = JsonSerializer.Deserialize<List<AvailableSeatDto>>(raw);
    seatList.Should().NotBeNull();
}




    [Fact]
    public async Task Should_Book_Seat_Successfully()
    {
        var dto = new
        {
            Direction="outbound",
            FlightId = 1001,
            FlightSource = "Delta",
            SeatNumber = "12A",
            SessionId = Guid.NewGuid(),

        };

        var response = await _client.PostAsJsonAsync("/api/v1/Booking/selectseat/direct", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Seat booked successfully.");
    }

    [Fact]
    public async Task Should_Reject_Already_Booked_Seat()
    {
        var session = Guid.NewGuid();
        var dto = new BookingDirectSeatsDto
        {
            FlightId = 1001,
            FlightSource = "Delta",
            SeatNumber = "15B",
            SessionId = session,
            Direction="outbound"
        };

        // First booking
        await _client.PostAsJsonAsync("/api/v1/Booking/selectseat/direct", dto);

        // Attempt to rebook
        var response = await _client.PostAsJsonAsync("/api/v1/Booking/selectseat/direct", dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Seat already booked");
    }

[Fact]
public async Task Should_Book_Connecting_Seats_Successfully()
{
    var dto = new BookingConnectingSeatsDto
    {
        Flight1Id = 2001,
        Flight2Id = 2002,
        Flight1Sournce = "Delta",
        Flight2Source = "Southwest",
        Seat1 = "5A",
        Seat2 = "6B",
        Direction = "outbound",
        SessionId = Guid.NewGuid()
    };

    var response = await _client.PostAsJsonAsync("/api/v1/Booking/selectseat/connecting", dto);

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var content = await response.Content.ReadAsStringAsync();
    content.Should().Contain("Seat booked successfully.");
}
[Fact]
public async Task Should_Reject_Already_Booked_Connecting_Seats()
{
    var session = Guid.NewGuid();

    var dto = new BookingConnectingSeatsDto
    {
        Flight1Id = 2001,
        Flight2Id = 2002,
        Flight1Sournce = "Delta",
        Flight2Source = "Southwest",
        Seat1 = "9C",
        Seat2 = "10D",
        Direction = "outbound",
        SessionId = session
    };

    // First hold
    await _client.PostAsJsonAsync("/api/v1/Booking/selectseat/connecting", dto);

    // Second hold (should fail)
    var response = await _client.PostAsJsonAsync("/api/v1/Booking/selectseat/connecting", dto);
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var content = await response.Content.ReadAsStringAsync();
    content.Should().Contain("Seat already booked");
}



}
