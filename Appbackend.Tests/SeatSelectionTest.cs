namespace Appbackend.Tests;
using System.Net;
using System.Net.Http.Json;
using AppBackend.DTOs;
using AppBackend.Models;
using AppBackend.Tests;
using FluentAssertions;
using Xunit;

public class SeatBookingTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SeatBookingTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Book_Seat_Successfully()
    {
        var dto = new
        {
            FlightType = FlightTypes.Direct,
            FlightId = 1001,
            FlightSource = "Delta",
            SeatNumber = "12A",
            SessionId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/SeatBooking/select", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Seat booked successfully.");
    }

    [Fact]
    public async Task Should_Reject_Already_Booked_Seat()
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

        // First booking
        await _client.PostAsJsonAsync("/api/v1/SeatBooking/select", dto);

        // Attempt to rebook
        var response = await _client.PostAsJsonAsync("/api/v1/SeatBooking/select", dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Seat already booked");
    }
}
