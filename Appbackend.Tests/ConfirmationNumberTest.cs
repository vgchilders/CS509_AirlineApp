namespace Appbackend.Tests;
using AppBackend.util;
using FluentAssertions;
using Xunit;

public class ConfirmationNumberGeneratorTests
{
    [Fact]
    public void Should_Generate_Confirmation_Number_With_FL_Prefix()
    {
        var confirmation = ConfirmationNumberGenerator.Generate();

        //Assert
        confirmation.Should().StartWith("FL-");
        confirmation.Length.Should().Be(11); // "FL-" + 8 chars
    }

    [Fact]
    public void Should_Generate_Unique_Confirmation_Numbers()
    {
        var conf1 = ConfirmationNumberGenerator.Generate();
        var conf2 = ConfirmationNumberGenerator.Generate();

        //Assert
        conf1.Should().NotBe(conf2);
    }
}
