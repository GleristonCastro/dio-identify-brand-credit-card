namespace CreditCardIdentifier.Api.Models;

/// <summary>
/// Informações sobre um cartão de crédito identificado
/// </summary>
public class CardInfo
{
    /// <summary>
    /// Bandeira do cartão identificada
    /// </summary>
    public CardBrand Brand { get; set; }
    
    /// <summary>
    /// Indica se o cartão passou na validação do algoritmo de Luhn
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// Número do cartão (mascarado para segurança)
    /// </summary>
    public string MaskedCardNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Quantidade de dígitos do cartão
    /// </summary>
    public int CardLength { get; set; }
    
    /// <summary>
    /// Indica se o formato do número do cartão é válido
    /// </summary>
    public bool IsFormatValid { get; set; }
    
    /// <summary>
    /// Mensagem adicional sobre a validação
    /// </summary>
    public string? Message { get; set; }
}
