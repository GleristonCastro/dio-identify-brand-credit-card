using Xunit;
using FluentAssertions;
using CreditCardIdentifier.Api.Validators;

namespace CreditCardIdentifier.Tests.Validators;

/// <summary>
/// Testes para o validador de cartões (Algoritmo de Luhn e formatação)
/// </summary>
public class CardValidatorTests
{
    [Theory]
    [InlineData("4111111111111111", true)] // Visa válido
    [InlineData("5105105105105100", true)] // MasterCard válido
    [InlineData("378282246310005", true)] // Amex válido
    [InlineData("6011000000000004", true)] // Discover válido
    [InlineData("4111111111111112", false)] // Visa inválido (Luhn)
    [InlineData("5105105105105101", false)] // MasterCard inválido (Luhn)
    [InlineData("378282246310006", false)] // Amex inválido (Luhn)
    public void ValidateLuhn_Should_Return_Correct_Result(string cardNumber, bool expectedResult)
    {
        // Act
        var result = CardValidator.ValidateLuhn(cardNumber);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("4111111111111111", true)]
    [InlineData("411111111111", true)] // Mínimo 12 dígitos
    [InlineData("4111111111111111111", true)] // Máximo 19 dígitos
    [InlineData("41111111111", false)] // Menos que 12 dígitos
    [InlineData("41111111111111111111", false)] // Mais que 19 dígitos
    [InlineData("411111111111111A", false)] // Contém letra
    [InlineData("", false)] // Vazio
    public void ValidateFormat_Should_Return_Correct_Result(string cardNumber, bool expectedResult)
    {
        // Act
        var result = CardValidator.ValidateFormat(cardNumber);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("4111111111111111", "4111********1111")]
    [InlineData("378282246310005", "3782*******0005")]
    [InlineData("5105105105105100", "5105********5100")]
    [InlineData("123456", "******")] // Menos de 8 dígitos
    public void MaskCardNumber_Should_Mask_Correctly(string cardNumber, string expectedMasked)
    {
        // Act
        var result = CardValidator.MaskCardNumber(cardNumber);

        // Assert
        result.Should().Be(expectedMasked);
    }

    [Theory]
    [InlineData("4111 1111 1111 1111", "4111111111111111")]
    [InlineData("4111-1111-1111-1111", "4111111111111111")]
    [InlineData("4111  1111  1111  1111", "4111111111111111")]
    public void SanitizeCardNumber_Should_Remove_Spaces_And_Hyphens(string cardNumber, string expected)
    {
        // Act
        var result = CardValidator.SanitizeCardNumber(cardNumber);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ValidateLuhn_Should_Handle_Spaces_In_CardNumber()
    {
        // Arrange
        var cardNumber = "4111 1111 1111 1111";

        // Act
        var result = CardValidator.ValidateLuhn(cardNumber);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateLuhn_Should_Handle_Hyphens_In_CardNumber()
    {
        // Arrange
        var cardNumber = "4111-1111-1111-1111";

        // Act
        var result = CardValidator.ValidateLuhn(cardNumber);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateFormat_Should_Accept_Card_With_Spaces()
    {
        // Arrange
        var cardNumber = "4111 1111 1111 1111";

        // Act
        var result = CardValidator.ValidateFormat(cardNumber);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MaskCardNumber_Should_Handle_Empty_String()
    {
        // Arrange
        var cardNumber = "";

        // Act
        var result = CardValidator.MaskCardNumber(cardNumber);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void SanitizeCardNumber_Should_Handle_Null()
    {
        // Arrange
        string? cardNumber = null;

        // Act
        var result = CardValidator.SanitizeCardNumber(cardNumber!);

        // Assert
        result.Should().Be(string.Empty);
    }
}
