using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using AppBackend.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Appbackend.Tests
{
    public class PaymentControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly Mock<IPaymentService> _paymentServiceMock = new();
        private readonly Mock<ITicketBookingRepository> _ticketRepoMock = new();

        public PaymentControllerTests(CustomWebApplicationFactory factory)
        {
            var appFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_paymentServiceMock.Object);
                    services.AddSingleton(_ticketRepoMock.Object);
                });
            });
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_SessionId_Is_Invalid()
        {
            var request = new CreateStripeSessionRequestDto
            {
                SessionId = "invalid-guid",
                BookingReference = "BR-TEST"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Payment/create-session", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_NotFound_When_Booking_Not_Exists()
        {
            var sessionId = Guid.NewGuid().ToString();
            _ticketRepoMock.Setup(r => r.GetBookingBySessionAndReferenceAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                            .ReturnsAsync((TicketBooking)null);

            var request = new CreateStripeSessionRequestDto
            {
                SessionId = sessionId,
                BookingReference = "BR-MISSING"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Payment/create-session", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Return_Url_When_Session_Creation_Is_Successful()
        {
            var sessionId = Guid.NewGuid();
            var booking = new TicketBooking
            {
                Id = 1,
                BookingReference = "BR-SUCCESS",
                Price = 299.9,
                SessionId = sessionId
            };

            _ticketRepoMock.Setup(r => r.GetBookingBySessionAndReferenceAsync(sessionId, booking.BookingReference))
                            .ReturnsAsync(booking);

            _paymentServiceMock.Setup(p => p.CreateCheckoutSessionAsync(
                booking.Price,
                sessionId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                booking.BookingReference))
                .ReturnsAsync("https://mocked-stripe-session.url");

            var request = new CreateStripeSessionRequestDto
            {
                SessionId = sessionId.ToString(),
                BookingReference = booking.BookingReference
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Payment/create-session", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<SessionUrlResponse>();
            result.Should().NotBeNull();
            result!.Url.Should().Be("https://mocked-stripe-session.url");
        }

        [Fact]
        public async Task Should_Return_InternalServerError_On_Exception()
        {
            var sessionId = Guid.NewGuid().ToString();

            _ticketRepoMock.Setup(r => r.GetBookingBySessionAndReferenceAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                            .ThrowsAsync(new Exception("DB error"));

            var request = new CreateStripeSessionRequestDto
            {
                SessionId = sessionId,
                BookingReference = "BR-FAIL"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Payment/create-session", request);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        private class SessionUrlResponse
        {
            public string Url { get; set; } = string.Empty;
        }
    }
}
