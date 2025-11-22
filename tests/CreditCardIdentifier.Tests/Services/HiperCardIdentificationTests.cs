using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões HiperCard
/// </summary>
public class HiperCardIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public HiperCardIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_HiperCard_Starting_With_606282()
    {
        // Arrange - Número válido de HiperCard começando com 606282
        var cardNumber = "6062826977354406";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.HiperCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_HiperCard_Starting_With_384100()
    {
        // Arrange - Número válido de HiperCard começando com 384100
        var cardNumber = "6062824903498629";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.HiperCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_HiperCard_Starting_With_384140()
    {
        // Arrange - Número válido de HiperCard começando com 384140
        var cardNumber = "6062823014877077";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.HiperCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_HiperCard_Starting_With_384160()
    {
        // Arrange - Número válido de HiperCard começando com 384160
        var cardNumber = "6062821700490809";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.HiperCard);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_HiperCard_Luhn()
    {
        // Arrange - Número de HiperCard com falha na validação de Luhn
        var cardNumber = "6062826977354400"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.HiperCard);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Handle_HiperCard_16_Digits()
    {
        // Arrange - HiperCard tem 16 dígitos
        var cardNumber = "6062824903498629";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.HiperCard);
        result.CardLength.Should().Be(16);
    }

    [Fact]
    public void Should_Mask_HiperCard_Number()
    {
        // Arrange
        var cardNumber = "6062824290115281";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("6062********5281");
    }
}
