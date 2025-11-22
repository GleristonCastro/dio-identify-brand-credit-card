using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões Voyager
/// </summary>
public class VoyagerIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public VoyagerIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_Voyager_Starting_With_8699()
    {
        // Arrange - Número válido de Voyager começando com 8699
        var cardNumber = "869915216183227";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Voyager);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_Voyager_Luhn()
    {
        // Arrange - Número de Voyager com falha na validação de Luhn
        var cardNumber = "869900000000000"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Voyager);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Identify_As_Voyager_Different_BIN()
    {
        // Arrange - Número começando com 86 mas não 8699
        var cardNumber = "8698000000000009";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().NotBe(CardBrand.Voyager);
    }

    [Fact]
    public void Should_Validate_Voyager_15_Digits()
    {
        // Arrange - Voyager tem exatamente 15 dígitos
        var cardNumber = "869957150361310";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Voyager);
        result.CardLength.Should().Be(15);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Mask_Voyager_Number()
    {
        // Arrange
        var cardNumber = "869988888888880";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("8699*******8880");
    }

    [Fact]
    public void Should_Validate_Voyager_Format()
    {
        // Arrange
        var cardNumber = "869912345678908";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Voyager);
        result.IsFormatValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Return_Correct_Length_For_Voyager()
    {
        // Arrange
        var cardNumber = "869944444444445";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.CardLength.Should().Be(15);
    }
}
