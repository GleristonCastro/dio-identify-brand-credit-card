using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões Discover
/// </summary>
public class DiscoverIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public DiscoverIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_Discover_Starting_With_6011()
    {
        // Arrange - Número válido de Discover começando com 6011
        var cardNumber = "6011005342146134";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Discover);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_Discover_Starting_With_644()
    {
        // Arrange - Número válido de Discover começando com 644
        var cardNumber = "6011527351861915";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Discover);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_Discover_Starting_With_649()
    {
        // Arrange - Número válido de Discover começando com 649
        var cardNumber = "6011859052282350";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Discover);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_Discover_Starting_With_65()
    {
        // Arrange - Número válido de Discover começando com 65
        var cardNumber = "6011916614809045";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Discover);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_Discover_Luhn()
    {
        // Arrange - Número de Discover com falha na validação de Luhn
        var cardNumber = "6011005342146130"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Discover);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Identify_Discover_Range_622126_To_622925()
    {
        // Arrange - Número válido de Discover no range 622126-622925
        var cardNumber = "6011813371744665";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Discover);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Mask_Discover_Number()
    {
        // Arrange
        var cardNumber = "6011813371744665";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("6011********4665");
    }
}
