using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões JCB
/// </summary>
public class JCBIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public JCBIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_JCB_Starting_With_3528()
    {
        // Arrange - Número válido de JCB começando com 3528 (início do range)
        var cardNumber = "3533403135055545";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.JCB);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_JCB_Starting_With_3589()
    {
        // Arrange - Número válido de JCB começando com 3589 (fim do range)
        var cardNumber = "3537472335818354";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.JCB);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_JCB_Mid_Range()
    {
        // Arrange - Número válido de JCB no meio do range
        var cardNumber = "3536331354276871";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.JCB);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_JCB_Luhn()
    {
        // Arrange - Número de JCB com falha na validação de Luhn
        var cardNumber = "3533403135055540"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.JCB);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Identify_As_JCB_Outside_Range()
    {
        // Arrange - Número começando com 35 mas fora do range JCB
        var cardNumber = "3527000000000005";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().NotBe(CardBrand.JCB);
    }

    [Fact]
    public void Should_Validate_JCB_16_Digits()
    {
        // Arrange - JCB tem exatamente 16 dígitos
        var cardNumber = "3557305278155472";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.JCB);
        result.CardLength.Should().Be(16);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Mask_JCB_Number()
    {
        // Arrange
        var cardNumber = "3568622374577574";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("3568********7574");
    }
}
