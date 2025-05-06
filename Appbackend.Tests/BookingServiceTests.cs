namespace AppBackend.Tests;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using AppBackend.Services;
using AppBackend.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

public class BookingServiceTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly AppDbContext _context;
    private readonly Mock<IEMailService> _mockEmailService = new();
    private readonly Mock<IPDFService> _mockPdfService = new();
    private readonly BookingService _bookingService;

    public BookingServiceTests(CustomWebApplicationFactory factory)
    {
        _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        _context.Database.EnsureDeleted(); // Clean state before each test class run
        _context.Database.EnsureCreated();

        _bookingService = new BookingService(_mockEmailService.Object, _mockPdfService.Object, _context);
    }

    [Fact]
    public async Task Should_Return_Enriched_Seats_For_BookingId()
    {
        var bookingId = 123;
        _context.BookedSeats.Add(new BookedSeat
        {
            TicketBookingId = bookingId,
            SeatNumber = "1A",
            IsConfirmed = true,
            FlightSource = "Delta",
            FlightId = 1001,
            Leg = 1,
            Direction = "North"
        });

        _context.Deltas.Add(new Deltas {
            Id = 1001,
            FlightNumber = "DL123",
            DepartAirport = "JFK",
            ArriveAirport = "LAX",
        });
        await _context.SaveChangesAsync();

        var result = await _bookingService.GetEnrichedSeatAsync(bookingId);

        //Assert
        result.Should().HaveCount(1);
        result[0].SeatNumber.Should().Be("1A");
        result[0].FlightSource.Should().Be("Delta");
        result[0].Gate.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Should_Send_Email_With_Confirmed_Seats()
    {
        var booking = new TicketBooking
        {
            FirstName = "Valerie",
            LastName = "Childers",
            Email = "vgchilders@wpi.edu",
            Price = 300,
            ConfirmationCode = "CONF321",
            BookedSeats = new List<BookedSeat>
            {
                new BookedSeat { SeatNumber = "22C", IsConfirmed = true },
                new BookedSeat { SeatNumber = "22D", IsConfirmed = false }
            }
        };

        await _bookingService.SendConfirmationEmailAsync(booking);

        //Assert
        _mockEmailService.Verify(es =>
            es.SendEmail(
                "vgchilders@wpi.edu",
                "Your Flight Ticket and Confirmation", // match actual subject
                It.Is<string>(b =>
                    b.Contains("Valerie Childers") &&
                    b.Contains("300") &&
                    b.Contains("CONF321")
                ),
                It.IsAny<byte[]>(), // allow for any PDF attachment
                "Ticket_.pdf"       // match actual attachment name
            ),
            Times.Once
        );
    }
}
