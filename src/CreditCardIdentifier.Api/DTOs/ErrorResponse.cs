namespace CreditCardIdentifier.Api.DTOs;

/// <summary>
/// Response padronizado para erros
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Mensagem de erro
    /// </summary>
    /// <example>Ocorreu um erro ao processar a requisição</example>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Código de status HTTP
    /// </summary>
    /// <example>400</example>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// Detalhes adicionais do erro
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }
    
    /// <summary>
    /// Timestamp do erro
    /// </summary>
    /// <example>2025-11-21T10:30:00Z</example>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
