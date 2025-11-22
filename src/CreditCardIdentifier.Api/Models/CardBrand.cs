namespace CreditCardIdentifier.Api.Models;

/// <summary>
/// Representa as bandeiras de cartão de crédito suportadas pelo sistema
/// </summary>
public enum CardBrand
{
    /// <summary>
    /// Bandeira desconhecida ou não identificada
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Visa - BIN começa com 4
    /// </summary>
    Visa = 1,
    
    /// <summary>
    /// MasterCard - BIN começa com 51-55 ou 2221-2720
    /// </summary>
    MasterCard = 2,
    
    /// <summary>
    /// American Express - BIN começa com 34 ou 37
    /// </summary>
    AmericanExpress = 3,
    
    /// <summary>
    /// Diners Club - BIN começa com 300-305, 36 ou 38
    /// </summary>
    DinersClub = 4,
    
    /// <summary>
    /// Discover - BIN começa com 6011, 622126-622925, 644-649, 65
    /// </summary>
    Discover = 5,
    
    /// <summary>
    /// JCB - BIN começa com 3528-3589
    /// </summary>
    JCB = 6,
    
    /// <summary>
    /// enRoute - BIN começa com 2014-2149
    /// </summary>
    EnRoute = 7,
    
    /// <summary>
    /// Voyager - BIN começa com 8699
    /// </summary>
    Voyager = 8,
    
    /// <summary>
    /// HiperCard - BIN começa com 606282, 384100, 384140, 384160
    /// </summary>
    HiperCard = 9,
    
    /// <summary>
    /// Aura - BIN começa com 50
    /// </summary>
    Aura = 10
}
