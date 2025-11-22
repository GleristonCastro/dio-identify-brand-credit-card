using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões MasterCard
/// </summary>
public class MasterCardIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public MasterCardIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_MasterCard_Starting_With_51()
    {
        // Arrange - Número válido de MasterCard começando com 51
        var cardNumber = "5121014513470686";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.MasterCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_MasterCard_Starting_With_55()
    {
        // Arrange - Número válido de MasterCard começando com 55
        var cardNumber = "5560606858405462";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.MasterCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_MasterCard_New_Range_2221()
    {
        // Arrange - Número válido de MasterCard novo range (2221-2720)
        var cardNumber = "2221000000000009";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.MasterCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_MasterCard_New_Range_2720()
    {
        // Arrange - Número válido de MasterCard no limite superior do novo range
        var cardNumber = "2720999999999996";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.MasterCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_MasterCard_Luhn()
    {
        // Arrange - Número de MasterCard com falha na validação de Luhn
        var cardNumber = "5121014513470687"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.MasterCard);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Mask_MasterCard_Number()
    {
        // Arrange
        var cardNumber = "5445623218422096";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("5445********2096");
    }

    [Fact]
    public void Should_Return_Correct_Length_For_MasterCard()
    {
        // Arrange
        var cardNumber = "5352403157559799";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.CardLength.Should().Be(16);
    }
}
