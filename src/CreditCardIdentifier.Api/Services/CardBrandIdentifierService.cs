using CreditCardIdentifier.Api.Models;
using CreditCardIdentifier.Api.Validators;

namespace CreditCardIdentifier.Api.Services;

/// <summary>
/// Interface para o serviço de identificação de bandeiras de cartão
/// </summary>
public interface ICardBrandIdentifierService
{
    /// <summary>
    /// Identifica a bandeira de um cartão de crédito baseado no número
    /// </summary>
    /// <param name="cardNumber">Número do cartão</param>
    /// <returns>Informações sobre o cartão identificado</returns>
    CardInfo IdentifyCard(string cardNumber);
}

/// <summary>
/// Serviço responsável por identificar a bandeira de cartões de crédito
/// baseado nos padrões BIN (Bank Identification Number)
/// </summary>
public class CardBrandIdentifierService : ICardBrandIdentifierService
{
    /// <summary>
    /// Identifica a bandeira de um cartão de crédito baseado no número
    /// </summary>
    /// <param name="cardNumber">Número do cartão</param>
    /// <returns>Informações sobre o cartão identificado</returns>
    public CardInfo IdentifyCard(string cardNumber)
    {
        // Valida se o formato original contém apenas dígitos, espaços e hífens
        var hasOnlyValidChars = !string.IsNullOrWhiteSpace(cardNumber) && 
                                cardNumber.All(c => char.IsDigit(c) || c == ' ' || c == '-');
        
        var sanitizedNumber = CardValidator.SanitizeCardNumber(cardNumber);
        
        var cardInfo = new CardInfo
        {
            CardLength = sanitizedNumber.Length,
            IsFormatValid = hasOnlyValidChars && CardValidator.ValidateFormat(sanitizedNumber),
            IsValid = CardValidator.ValidateLuhn(sanitizedNumber),
            MaskedCardNumber = CardValidator.MaskCardNumber(sanitizedNumber)
        };

        if (!cardInfo.IsFormatValid)
        {
            cardInfo.Brand = CardBrand.Unknown;
            cardInfo.Message = "Formato de cartão inválido";
            return cardInfo;
        }

        // Identifica a bandeira baseado nos padrões BIN
        cardInfo.Brand = IdentifyBrandByBIN(sanitizedNumber);
        
        if (cardInfo.Brand == CardBrand.Unknown)
        {
            cardInfo.Message = "Bandeira não identificada";
        }
        else if (!cardInfo.IsValid)
        {
            cardInfo.Message = $"Bandeira identificada: {cardInfo.Brand}, mas o cartão falhou na validação de Luhn";
        }
        else
        {
            cardInfo.Message = $"Cartão {cardInfo.Brand} válido";
        }

        return cardInfo;
    }

    /// <summary>
    /// Identifica a bandeira baseado nos padrões BIN
    /// </summary>
    private CardBrand IdentifyBrandByBIN(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
            return CardBrand.Unknown;

        // HiperCard: verificar primeiro os BINs de 6 dígitos específicos (384100, 384140, 384160)
        if (cardNumber.Length >= 6)
        {
            var first6 = cardNumber.Substring(0, 6);
            if (int.TryParse(first6, out int prefix6))
            {
                if (prefix6 == 606282 || prefix6 == 384100 || prefix6 == 384140 || prefix6 == 384160)
                    return CardBrand.HiperCard;
                
                // Discover: começa com 622126-622925
                if (prefix6 >= 622126 && prefix6 <= 622925)
                    return CardBrand.Discover;
            }
        }

        // Visa: começa com 4
        if (cardNumber.StartsWith("4"))
            return CardBrand.Visa;

        // American Express: começa com 34 ou 37
        if (cardNumber.StartsWith("34") || cardNumber.StartsWith("37"))
            return CardBrand.AmericanExpress;

        // Diners Club: começa com 300-305, 36 ou 38
        if (cardNumber.StartsWith("36") || cardNumber.StartsWith("38"))
            return CardBrand.DinersClub;
        
        if (cardNumber.Length >= 3)
        {
            var first3 = cardNumber.Substring(0, 3);
            if (int.TryParse(first3, out int prefix3) && prefix3 >= 300 && prefix3 <= 305)
                return CardBrand.DinersClub;
        }

        // JCB: começa com 3528-3589
        if (cardNumber.Length >= 4)
        {
            var first4 = cardNumber.Substring(0, 4);
            if (int.TryParse(first4, out int prefix4))
            {
                if (prefix4 >= 3528 && prefix4 <= 3589)
                    return CardBrand.JCB;
                
                // enRoute: começa com 2014-2149
                if (prefix4 >= 2014 && prefix4 <= 2149)
                    return CardBrand.EnRoute;
                
                // Voyager: começa com 8699
                if (prefix4 == 8699)
                    return CardBrand.Voyager;
                
                // Discover: começa com 6011
                if (prefix4 == 6011)
                    return CardBrand.Discover;
            }
        }

        // MasterCard: começa com 51-55 ou 2221-2720
        if (cardNumber.Length >= 2)
        {
            var first2 = cardNumber.Substring(0, 2);
            if (int.TryParse(first2, out int prefix2))
            {
                if (prefix2 >= 51 && prefix2 <= 55)
                    return CardBrand.MasterCard;
                
                // Aura: começa com 50
                if (prefix2 == 50)
                    return CardBrand.Aura;
                
                // Discover: começa com 65
                if (prefix2 == 65)
                    return CardBrand.Discover;
            }
        }

        // MasterCard: 2221-2720
        if (cardNumber.Length >= 4)
        {
            var first4 = cardNumber.Substring(0, 4);
            if (int.TryParse(first4, out int prefix4) && prefix4 >= 2221 && prefix4 <= 2720)
                return CardBrand.MasterCard;
        }

        // Discover: começa com 644-649
        if (cardNumber.Length >= 3)
        {
            var first3 = cardNumber.Substring(0, 3);
            if (int.TryParse(first3, out int prefix3) && prefix3 >= 644 && prefix3 <= 649)
                return CardBrand.Discover;
        }

        return CardBrand.Unknown;
    }
}
