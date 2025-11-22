using Microsoft.AspNetCore.Mvc;
using CreditCardIdentifier.Api.DTOs;
using CreditCardIdentifier.Api.Services;
using System.Net;

namespace CreditCardIdentifier.Api.Controllers;

/// <summary>
/// Controller responsável pela identificação de bandeiras de cartões de crédito
/// </summary>
[ApiController]
[Route("api/card")]
[Produces("application/json")]
public class CardIdentificationController : ControllerBase
{
    private readonly ICardBrandIdentifierService _cardBrandIdentifierService;
    private readonly ILogger<CardIdentificationController> _logger;

    public CardIdentificationController(
        ICardBrandIdentifierService cardBrandIdentifierService,
        ILogger<CardIdentificationController> logger)
    {
        _cardBrandIdentifierService = cardBrandIdentifierService;
        _logger = logger;
    }

    /// <summary>
    /// Identifica a bandeira de um cartão de crédito baseado no número fornecido
    /// </summary>
    /// <param name="request">Request contendo o número do cartão</param>
    /// <returns>Informações sobre a bandeira identificada e validação do cartão</returns>
    /// <response code="200">Cartão identificado com sucesso</response>
    /// <response code="400">Requisição inválida - número do cartão malformado</response>
    /// <response code="422">Cartão com formato válido mas não passou na validação de Luhn</response>
    /// <response code="429">Muitas requisições - limite de taxa excedido</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("identify")]
    [ProducesResponseType(typeof(CardIdentificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CardIdentificationResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<CardIdentificationResponse> IdentifyCard([FromBody] CardIdentificationRequest request)
    {
        try
        {
            _logger.LogInformation("Recebida requisição para identificar cartão");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Requisição inválida: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
                
                var errorResponse = new ErrorResponse
                {
                    Message = "Requisição inválida",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                    )
                };
                
                return BadRequest(errorResponse);
            }

            var cardInfo = _cardBrandIdentifierService.IdentifyCard(request.CardNumber);

            var response = new CardIdentificationResponse
            {
                Brand = cardInfo.Brand.ToString(),
                IsValid = cardInfo.IsValid,
                MaskedCardNumber = cardInfo.MaskedCardNumber,
                CardLength = cardInfo.CardLength,
                Message = cardInfo.Message ?? "Processado com sucesso",
                StatusCode = cardInfo.IsValid ? (int)HttpStatusCode.OK : (int)HttpStatusCode.UnprocessableEntity
            };

            if (!cardInfo.IsValid && cardInfo.IsFormatValid)
            {
                _logger.LogWarning("Cartão com formato válido mas falhou na validação de Luhn");
                return UnprocessableEntity(response);
            }

            _logger.LogInformation("Cartão identificado como {Brand}", cardInfo.Brand);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar identificação de cartão");
            
            var errorResponse = new ErrorResponse
            {
                Message = "Erro ao processar a requisição",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            
            return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
        }
    }

    /// <summary>
    /// Endpoint de health check
    /// </summary>
    /// <returns>Status da API</returns>
    /// <response code="200">API funcionando corretamente</response>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> HealthCheck()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }
}
