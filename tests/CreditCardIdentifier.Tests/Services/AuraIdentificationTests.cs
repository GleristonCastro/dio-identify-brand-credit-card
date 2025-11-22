using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões Aura
/// </summary>
public class AuraIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public AuraIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_Aura_Starting_With_50()
    {
        // Arrange - Número válido de Aura começando com 50
        var cardNumber = "5078813414903288";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Aura);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_Aura_Luhn()
    {
        // Arrange - Número de Aura com falha na validação de Luhn
        var cardNumber = "5078813414903280"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Aura);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Prioritize_MasterCard_Over_Aura_When_Starting_With_51_To_55()
    {
        // Arrange - Números começando com 51-55 devem ser MasterCard, não Aura
        var cardNumber = "5105105105105100";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.MasterCard, "porque MasterCard tem prioridade sobre Aura para prefixos 51-55");
    }

    [Fact]
    public void Should_Handle_Aura_16_Digits()
    {
        // Arrange - Aura tem 16 dígitos
        var cardNumber = "5041610417930589";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Aura);
        result.CardLength.Should().Be(16);
    }

    [Fact]
    public void Should_Mask_Aura_Number()
    {
        // Arrange
        var cardNumber = "5016116684859833";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("5016********9833");
    }

    [Fact]
    public void Should_Validate_Aura_Format()
    {
        // Arrange
        var cardNumber = "5095367904702741";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Aura);
        result.IsFormatValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Return_Correct_Length_For_Aura()
    {
        // Arrange
        var cardNumber = "5005105115293814";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.CardLength.Should().Be(16);
    }
}
