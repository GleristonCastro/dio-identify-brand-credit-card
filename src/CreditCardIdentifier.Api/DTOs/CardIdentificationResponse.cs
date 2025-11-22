using CreditCardIdentifier.Api.Models;

namespace CreditCardIdentifier.Api.DTOs;

/// <summary>
/// Response com informações sobre a identificação da bandeira do cartão
/// </summary>
public class CardIdentificationResponse
{
    /// <summary>
    /// Bandeira do cartão identificada
    /// </summary>
    /// <example>Visa</example>
    public string Brand { get; set; } = string.Empty;
    
    /// <summary>
    /// Indica se o cartão passou na validação do algoritmo de Luhn
    /// </summary>
    /// <example>true</example>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// Número do cartão mascarado (mostra apenas os 4 primeiros e 4 últimos dígitos)
    /// </summary>
    /// <example>4111********1111</example>
    public string MaskedCardNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Quantidade de dígitos do cartão
    /// </summary>
    /// <example>16</example>
    public int CardLength { get; set; }
    
    /// <summary>
    /// Mensagem adicional sobre a validação
    /// </summary>
    /// <example>Cartão válido</example>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Código de status HTTP da resposta
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; set; }
}
