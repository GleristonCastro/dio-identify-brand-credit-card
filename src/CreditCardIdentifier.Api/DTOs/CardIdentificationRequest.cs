using System.ComponentModel.DataAnnotations;

namespace CreditCardIdentifier.Api.DTOs;

/// <summary>
/// Request para identificação de bandeira de cartão de crédito
/// </summary>
public class CardIdentificationRequest
{
    /// <summary>
    /// Número do cartão de crédito (apenas dígitos, sem espaços ou hífens)
    /// </summary>
    /// <example>4111111111111111</example>
    [Required(ErrorMessage = "O número do cartão é obrigatório")]
    [RegularExpression(@"^\d{12,19}$", ErrorMessage = "O número do cartão deve conter apenas dígitos e ter entre 12 e 19 caracteres")]
    public string CardNumber { get; set; } = string.Empty;
}
