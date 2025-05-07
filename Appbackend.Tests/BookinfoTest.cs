namespace Appbackend.Tests;

using AppBackend.DTOs;
using AppBackend.Tests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

public class BookingInfoTests :  IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BookingInfoTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
public async Task Should_Save_Booking_Info_Successfully()
{
    var dto = new BookingInfoDto
    {
        SessionId = Guid.NewGuid(),
        FirstName = "Jane",
        LastName = "Doe",
        Email = "jane.doe@example.com",
        PhoneNumber = "5551234567",
        Gender = "Female",
        DateOfBirth = new DateTime(1992, 5, 12),
        Price = 299,
        Flights = new List<FlightLegDto>
        {
            new FlightLegDto
            {
                FlightId = 1001,
                FlightSource = "Delta",
                Direction = "outbound"
            }
        }
    };

    var response = await _client.PostAsJsonAsync("/api/v1/Booking/info", dto);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
    content.Should().ContainKey("bookingReference");
    content["message"].Should().Be("Booking info saved. Proceed to payment.");
}
[Fact]
public async Task Should_Return_Validation_Error_When_BookingInfo_Is_Invalid()
{
    // Arrange: Create an invalid booking info DTO (missing required fields)
    var invalidDto = new
    {
        sessionId = Guid.NewGuid(),
        firstName = "", // Invalid: required
        lastName = "Smith",
        email = "",     // Invalid: required
        phoneNumber = "123", // Invalid: optional but poorly formatted
        dateOfBirth = "2000-01-01",
        gender = "Male",
        price = -50, // Invalid: assuming price must be > 0
        flights = new[] {
            new {
                flightId = 123,
                flightSource = "Delta",
                direction = "outbound"
            }
        }
    };

    // Act
    var response = await _client.PostAsJsonAsync("/api/v1/Booking/info", invalidDto);
    var content = await response.Content.ReadAsStringAsync();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Expected validation failure. Server said: {content}");
    content.Should().Contain("Validation failed");
    content.Should().Contain("FirstName");
    content.Should().Contain("Email");
    content.Should().Contain("Price");
}


}
