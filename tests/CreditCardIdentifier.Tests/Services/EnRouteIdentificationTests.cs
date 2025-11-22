using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões enRoute
/// </summary>
public class EnRouteIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public EnRouteIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_EnRoute_Starting_With_2014()
    {
        // Arrange - Número válido de enRoute começando com 2014 (início do range)
        var cardNumber = "2014000000000000";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.EnRoute);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_EnRoute_Starting_With_2149()
    {
        // Arrange - Número válido de enRoute começando com 2149 (fim do range)
        var cardNumber = "2149000000000008";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.EnRoute);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_EnRoute_Mid_Range()
    {
        // Arrange - Número válido de enRoute no meio do range
        var cardNumber = "214914998564342";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.EnRoute);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_EnRoute_Luhn()
    {
        // Arrange - Número de enRoute com falha na validação de Luhn
        var cardNumber = "214911235014500"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.EnRoute);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Identify_As_EnRoute_Outside_Range()
    {
        // Arrange - Número começando com 20 mas fora do range enRoute
        var cardNumber = "2013000000000002";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().NotBe(CardBrand.EnRoute);
    }

    [Fact]
    public void Should_Validate_EnRoute_15_Digits()
    {
        // Arrange - enRoute tem exatamente 15 dígitos
        var cardNumber = "214963786472671";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.EnRoute);
        result.CardLength.Should().Be(15);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Mask_EnRoute_Number()
    {
        // Arrange
        var cardNumber = "201401603767259";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("2014*******7259");
    }
}
