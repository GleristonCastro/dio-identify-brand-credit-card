namespace CreditCardIdentifier.Api.Validators;

/// <summary>
/// Validador de números de cartão de crédito usando o algoritmo de Luhn
/// </summary>
public class CardValidator
{
    /// <summary>
    /// Valida um número de cartão de crédito usando o algoritmo de Luhn (Mod 10)
    /// </summary>
    /// <param name="cardNumber">Número do cartão a ser validado</param>
    /// <returns>True se o cartão for válido, false caso contrário</returns>
    public static bool ValidateLuhn(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        // Remove espaços e hífens
        cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

        // Verifica se contém apenas dígitos
        if (!cardNumber.All(char.IsDigit))
            return false;

        // Verifica o tamanho (mínimo 12, máximo 19)
        if (cardNumber.Length < 12 || cardNumber.Length > 19)
            return false;

        int sum = 0;
        bool alternate = false;

        // Percorre os dígitos de trás para frente
        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            int digit = cardNumber[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            alternate = !alternate;
        }

        // O número é válido se a soma for divisível por 10
        return sum % 10 == 0;
    }

    /// <summary>
    /// Valida o formato básico de um número de cartão
    /// </summary>
    /// <param name="cardNumber">Número do cartão a ser validado</param>
    /// <returns>True se o formato for válido, false caso contrário</returns>
    public static bool ValidateFormat(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        // Remove espaços e hífens
        cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

        // Verifica se contém apenas dígitos
        if (!cardNumber.All(char.IsDigit))
            return false;

        // Verifica o tamanho (entre 12 e 19 dígitos)
        return cardNumber.Length >= 12 && cardNumber.Length <= 19;
    }

    /// <summary>
    /// Mascara o número do cartão mostrando apenas os 4 primeiros e 4 últimos dígitos
    /// </summary>
    /// <param name="cardNumber">Número do cartão a ser mascarado</param>
    /// <returns>Número do cartão mascarado</returns>
    public static string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        // Remove espaços e hífens
        cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

        if (cardNumber.Length <= 8)
            return new string('*', cardNumber.Length);

        string firstFour = cardNumber.Substring(0, 4);
        string lastFour = cardNumber.Substring(cardNumber.Length - 4);
        int middleLength = cardNumber.Length - 8;

        return $"{firstFour}{new string('*', middleLength)}{lastFour}";
    }

    /// <summary>
    /// Remove caracteres não numéricos do número do cartão
    /// </summary>
    /// <param name="cardNumber">Número do cartão</param>
    /// <returns>Número do cartão apenas com dígitos</returns>
    public static string SanitizeCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        return new string(cardNumber.Where(char.IsDigit).ToArray());
    }
}
