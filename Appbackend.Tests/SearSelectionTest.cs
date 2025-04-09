using System.Net;
using System.Net.Http.Json;
using AppBackend.DTOs;
using AppBackend.Models;
using FluentAssertions;
using Xunit;

public class SeatSelectionTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SeatSelectionTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Hold_Seat_Successfully()
    {
        var dto = new BookeSeatRequestDto
        {
            FlightType = FlightTypes.Direct,
            FlightId = 1001,
            FlightSource = "Delta",
            SeatNumber = "12A",
            SessionId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/SeatBooking/select", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Seat held successfully");
    }

    [Fact]
    public async Task Should_Reject_Already_Held_Seat()
    {
        var session = Guid.NewGuid();
        var dto = new BookeSeatRequestDto
        {
            FlightType = FlightTypes.Direct,
            FlightId = 1001,
            FlightSource = "Delta",
            SeatNumber = "15B",
            SessionId = session
        };

        await _client.PostAsJsonAsync("/api/v1/SeatSelection/select", dto); // first hold

        var response = await _client.PostAsJsonAsync("/api/v1/SeatBooking/select", dto); // second attempt
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
