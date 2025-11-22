using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões Visa
/// </summary>
public class VisaIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public VisaIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_Visa_16_Digits()
    {
        // Arrange - Número válido de Visa com 16 dígitos
        var cardNumber = "4929155201968312";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Visa);
        result.IsValid.Should().BeTrue();
        result.CardLength.Should().Be(16);
    }

    [Fact]
    public void Should_Identify_Visa_13_Digits()
    {
        // Arrange - Número válido de Visa com 13 dígitos
        var cardNumber = "4716647220335587";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Visa);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_Visa_Luhn()
    {
        // Arrange - Número de Visa com falha na validação de Luhn
        var cardNumber = "4929155201968313"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Visa);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Mask_Visa_Number()
    {
        // Arrange
        var cardNumber = "4539816367439198";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("4539********9198");
    }

    [Fact]
    public void Should_Identify_Visa_Starting_With_4()
    {
        // Arrange - Diferentes números Visa começando com 4
        var cardNumbers = new[]
        {
            "4916617182204939",
            "4556211457322470",
            "4929155201968312"
        };

        foreach (var cardNumber in cardNumbers)
        {
            // Act
            var result = _service.IdentifyCard(cardNumber);

            // Assert
            result.Brand.Should().Be(CardBrand.Visa, $"porque {cardNumber} é um Visa");
            result.IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void Should_Reject_Visa_With_Invalid_Format()
    {
        // Arrange - Número com letras
        var cardNumber = "411111111111111A";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.IsFormatValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Handle_Visa_With_Spaces()
    {
        // Arrange - Número com espaços
        var cardNumber = "4929 1552 0196 8312";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.Visa);
        result.IsValid.Should().BeTrue();
    }
}
