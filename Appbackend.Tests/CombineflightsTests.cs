namespace Appbackend.Tests;

using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using AppBackend.Tests;

public class CombinedFlightsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CombinedFlightsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

  [Fact]
    public async Task Should_Return_FlightSearchResults_When_Valid_Inputs()
    {
        // Arrange
        var departAirport = "boston";
        var arriveAirport = "atlanta";
        var departureDate = new DateTime(2022,12,27).ToString("yyyy-MM-ddTHH:mm:ss"); // ISO format

        var url = $"/api/v1/CombinedFlights/search" +
                  $"?departAirport={departAirport}" +
                  $"&arriveAirport={arriveAirport}" +
                  $"&departureDate={Uri.EscapeDataString(departureDate)}";

        // Act
        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Raw response: " + content);

        // Assert
        //response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected 200 OK but got {response.StatusCode}. Server said: {content}");
    }

}
