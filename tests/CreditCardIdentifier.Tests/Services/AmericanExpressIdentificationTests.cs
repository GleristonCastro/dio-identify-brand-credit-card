using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões American Express
/// </summary>
public class AmericanExpressIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public AmericanExpressIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_AmericanExpress_Starting_With_34()
    {
        // Arrange - Número válido de Amex começando com 34
        var cardNumber = "340892044134699";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.AmericanExpress);
        result.IsValid.Should().BeTrue();
        result.CardLength.Should().Be(15);
    }

    [Fact]
    public void Should_Identify_AmericanExpress_Starting_With_37()
    {
        // Arrange - Número válido de Amex começando com 37
        var cardNumber = "371046764524165";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.AmericanExpress);
        result.IsValid.Should().BeTrue();
        result.CardLength.Should().Be(15);
    }

    [Fact]
    public void Should_Identify_AmericanExpress_Real_Number()
    {
        // Arrange - Número real de teste Amex
        var cardNumber = "371518884107881";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.AmericanExpress);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_AmericanExpress_Luhn()
    {
        // Arrange - Número de Amex com falha na validação de Luhn
        var cardNumber = "371046764524160"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.AmericanExpress);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Mask_AmericanExpress_Number()
    {
        // Arrange
        var cardNumber = "374560215164311";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("3745*******4311");
    }

    [Fact]
    public void Should_Validate_AmericanExpress_15_Digits()
    {
        // Arrange - Amex tem exatamente 15 dígitos
        var cardNumber = "378936680515888";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.AmericanExpress);
        result.CardLength.Should().Be(15);
        result.IsValid.Should().BeTrue();
    }
}
