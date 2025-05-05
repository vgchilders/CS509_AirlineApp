namespace Appbackend.Tests;
using AppBackend.Services;
using AppBackend.Interfaces;
using AppBackend.Models;
using FluentAssertions;
using Moq;
using Xunit;

public class PriceCalculateTests
{
    [Fact]
    public async Task Should_Calculate_Price_Based_On_Distance()
    {
        var mockRepo = new Mock<ICitiesRepository>();
        mockRepo.Setup(r => r.GetCityByName("New York")).ReturnsAsync(new Cities { Latitude = (float) 40.7128, Longitude = (float) -74.0060 });
        mockRepo.Setup(r => r.GetCityByName("Los Angeles")).ReturnsAsync(new Cities { Latitude = (float) 34.0522, Longitude = (float) -118.2437 });

        var priceCalc = new PriceCalculate(mockRepo.Object);

        var price = await priceCalc.CalaculatePrice("New York", "Los Angeles");

        //Assert
        price.Should().BeGreaterThan(100);
    }
}
