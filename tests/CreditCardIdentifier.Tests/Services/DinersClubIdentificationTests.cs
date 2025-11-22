using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Services;
using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Tests.Services;

/// <summary>
/// Testes para identificação de cartões Diners Club
/// </summary>
public class DinersClubIdentificationTests
{
    private readonly ICardBrandIdentifierService _service;

    public DinersClubIdentificationTests()
    {
        _service = new CardBrandIdentifierService();
    }

    [Fact]
    public void Should_Identify_DinersClub_Starting_With_300()
    {
        // Arrange - Número válido de Diners começando com 300
        var cardNumber = "30299547089187";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.DinersClub);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_DinersClub_Starting_With_305()
    {
        // Arrange - Número válido de Diners começando com 305
        var cardNumber = "30399300260544";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.DinersClub);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_DinersClub_Starting_With_36()
    {
        // Arrange - Número válido de Diners começando com 36
        var cardNumber = "30310636567320";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.DinersClub);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Identify_DinersClub_Starting_With_38()
    {
        // Arrange - Número válido de Diners começando com 38
        var cardNumber = "38376977525500";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.DinersClub);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Invalid_DinersClub_Luhn()
    {
        // Arrange - Número de Diners Club com falha na validação de Luhn
        var cardNumber = "30299547089180"; // Último dígito incorreto

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.DinersClub);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Handle_DinersClub_14_Digits()
    {
        // Arrange - Diners Club pode ter 14 dígitos
        var cardNumber = "38501173743793";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.Brand.Should().Be(CardBrand.DinersClub);
        result.CardLength.Should().Be(14);
    }

    [Fact]
    public void Should_Mask_DinersClub_Number()
    {
        // Arrange
        var cardNumber = "38501173743793";

        // Act
        var result = _service.IdentifyCard(cardNumber);

        // Assert
        result.MaskedCardNumber.Should().Be("3850******3793");
    }
}
