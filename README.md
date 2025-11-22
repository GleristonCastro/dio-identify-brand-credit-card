# 💳 API de Identificação de Bandeiras de Cartão de Crédito

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-10.0-239120?logo=csharp)
![License](https://img.shields.io/badge/license-MIT-green)

API RESTful desenvolvida em **.NET 9** para identificação automática de bandeiras de cartões de crédito baseada no número do cartão (BIN - Bank Identification Number). Suporta 10 bandeiras principais com validação completa via algoritmo de Luhn.

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Bandeiras Suportadas](#-bandeiras-suportadas)
- [Arquitetura](#-arquitetura)
- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Recursos de Segurança](#-recursos-de-segurança)
- [Instalação e Configuração](#-instalação-e-configuração)
- [Uso da API](#-uso-da-api)
- [Documentação Swagger](#-documentação-swagger)
- [Testes](#-testes)
- [Exemplos](#-exemplos)
- [Contribuindo](#-contribuindo)

---

## 🎯 Visão Geral

Esta API permite identificar a bandeira de um cartão de crédito analisando os primeiros dígitos (BIN - Bank Identification Number) e validando o número completo usando o **algoritmo de Luhn (Mod 10)**. Ideal para sistemas de e-commerce, gateways de pagamento e aplicações que precisam validar e identificar cartões antes do processamento.

### Principais Funcionalidades

✅ Identificação de 10 bandeiras de cartão  
✅ Validação via algoritmo de Luhn  
✅ Mascaramento de números de cartão para segurança  
✅ Rate limiting (100 req/min por IP)  
✅ Documentação Swagger interativa  
✅ Respostas JSON padronizadas  
✅ Headers de segurança configurados  
✅ Tratamento global de erros  
✅ Testes unitários abrangentes (95 testes - 100% aprovados)

---

## 🎴 Bandeiras Suportadas

| Bandeira             | Padrão BIN                               | Dígitos | Exemplo          |
| -------------------- | ---------------------------------------- | ------- | ---------------- |
| **Visa**             | Começa com `4`                           | 13-19   | 4111111111111111 |
| **MasterCard**       | `51-55`, `2221-2720`                     | 16      | 5105105105105100 |
| **American Express** | `34`, `37`                               | 15      | 378282246310005  |
| **Diners Club**      | `300-305`, `36`, `38`                    | 14-16   | 30000000000004   |
| **Discover**         | `6011`, `622126-622925`, `644-649`, `65` | 16-19   | 6011000000000004 |
| **JCB**              | `3528-3589`                              | 16-19   | 3530111333300000 |
| **enRoute**          | `2014-2149`                              | 15      | 201400000000009  |
| **Voyager**          | `8699`                                   | 15      | 869900000000005  |
| **HiperCard**        | `606282`, `384100`, `384140`, `384160`   | 16      | 6062820000000005 |
| **Aura**             | `50` (exceto 51-55)                      | 16      | 5000000000000009 |

---

## 🏗️ Arquitetura

A API segue uma arquitetura em camadas com separação de responsabilidades:

```
src/CreditCardIdentifier.Api/
├── Controllers/           # Endpoints da API
│   └── CardIdentificationController.cs
├── Services/             # Lógica de negócio
│   └── CardBrandIdentifierService.cs
├── Validators/           # Validação de cartões (Luhn)
│   └── CardValidator.cs
├── DTOs/                 # Data Transfer Objects
│   ├── CardIdentificationRequest.cs
│   ├── CardIdentificationResponse.cs
│   └── ErrorResponse.cs
├── Models/               # Entidades de domínio
│   ├── CardBrand.cs
│   └── CardInfo.cs
├── Middleware/           # Middlewares personalizados
│   └── GlobalExceptionHandlerMiddleware.cs
└── Program.cs            # Configuração da aplicação

tests/CreditCardIdentifier.Tests/
├── Services/             # Testes de serviços
│   ├── MasterCardIdentificationTests.cs
│   ├── VisaIdentificationTests.cs
│   ├── AmericanExpressIdentificationTests.cs
│   ├── DinersClubIdentificationTests.cs
│   ├── DiscoverIdentificationTests.cs
│   ├── JCBIdentificationTests.cs
│   ├── EnRouteIdentificationTests.cs
│   ├── VoyagerIdentificationTests.cs
│   ├── HiperCardIdentificationTests.cs
│   └── AuraIdentificationTests.cs
└── Validators/           # Testes de validadores
    └── CardValidatorTests.cs
```

---

## 🛠️ Tecnologias Utilizadas

- **.NET 9.0** - Framework principal (SDK estável 9.0.300, não preview)
- **ASP.NET Core 9** - Web API
- **Swashbuckle.AspNetCore 7.2.0** - Documentação Swagger/OpenAPI (compatível com .NET 9)
- **Microsoft.OpenApi 1.6.22** - Modelos OpenAPI
- **xUnit 2.9.2** - Framework de testes
- **FluentAssertions 8.8.0** - Asserções fluentes para testes
- **Moq 4.20.72** - Framework de mock para testes

> **Nota**: Utilizamos Swashbuckle.AspNetCore 7.2.0 com Microsoft.OpenApi 1.6.22 pois a versão 10.x do Swashbuckle requer Microsoft.OpenApi 2.3.0 que possui mudanças incompatíveis (breaking changes) no namespace `Microsoft.OpenApi.Models`.

---

## 🔒 Recursos de Segurança

### Rate Limiting

- **Fixed Window Limiter**: 100 requisições por minuto por endereço IP
- Fila com até 10 requisições pendentes
- Resposta HTTP 429 quando limite excedido
- Header `Retry-After` com tempo de espera

### Security Headers

```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Referrer-Policy: no-referrer
Content-Security-Policy: default-src 'self'
```

### Outras Proteções

- **HTTPS obrigatório** em produção
- **HSTS** (HTTP Strict Transport Security)
- **CORS** configurável
- **Validação de entrada** com Data Annotations
- **Tratamento global de exceções**
- **Mascaramento de números de cartão** (mostra apenas primeiros 4 e últimos 4 dígitos)

---

## 📦 Instalação e Configuração

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Editor de código (VS Code, Visual Studio, Rider)

### Passos para Instalação

1. **Clone o repositório**

```bash
git clone https://github.com/seu-usuario/dio-identify-brand-cred-card.git
cd dio-identify-brand-cred-card
```

2. **Restaure as dependências**

```bash
dotnet restore
```

3. **Compile o projeto**

```bash
dotnet build
```

4. **Execute a aplicação**

```bash
dotnet run --project src/CreditCardIdentifier.Api
```

A API estará disponível em:

- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:7000` (na raiz)

### Configuração (appsettings.json)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "CreditCardIdentifier": "Information"
    }
  },
  "RateLimiting": {
    "PermitLimit": 100,
    "WindowMinutes": 1,
    "QueueLimit": 10
  }
}
```

---

## 🚀 Uso da API

### Endpoint Principal

**POST** `/api/card/identify`

Identifica a bandeira de um cartão de crédito e valida o número.

#### Request

```json
{
  "cardNumber": "4111111111111111"
}
```

#### Response 200 (Sucesso)

```json
{
  "brand": "Visa",
  "isValid": true,
  "maskedCardNumber": "4111********1111",
  "cardLength": 16,
  "message": "Cartão Visa válido",
  "statusCode": 200
}
```

#### Response 422 (Cartão com falha na validação de Luhn)

```json
{
  "brand": "Visa",
  "isValid": false,
  "maskedCardNumber": "4111********1112",
  "cardLength": 16,
  "message": "Bandeira identificada: Visa, mas o cartão falhou na validação de Luhn",
  "statusCode": 422
}
```

#### Response 400 (Requisição inválida)

```json
{
  "message": "Requisição inválida",
  "statusCode": 400,
  "errors": {
    "CardNumber": [
      "O número do cartão deve conter apenas dígitos e ter entre 12 e 19 caracteres"
    ]
  },
  "timestamp": "2025-11-21T10:30:00Z"
}
```

#### Response 429 (Rate Limit)

```json
{
  "message": "Limite de requisições excedido. Por favor, tente novamente mais tarde.",
  "statusCode": 429,
  "timestamp": "2025-11-21T10:30:00Z"
}
```

### Health Check

**GET** `/api/card/health`

Verifica se a API está funcionando.

```json
{
  "status": "healthy",
  "timestamp": "2025-11-21T10:30:00Z",
  "version": "1.0.0"
}
```

---

## 📖 Documentação Swagger

Acesse a documentação interativa em: **`https://localhost:7000`**

A interface Swagger permite:

- Visualizar todos os endpoints
- Testar requisições diretamente no navegador
- Ver exemplos de request/response
- Entender os códigos de status HTTP
- Explorar os modelos de dados

---

## 🧪 Testes

O projeto possui **95 testes unitários (100% aprovados)** cobrindo todas as bandeiras e cenários.

### Executar todos os testes

```bash
dotnet test
```

### Executar testes com cobertura detalhada

```bash
dotnet test --verbosity detailed
```

### Estrutura de Testes

- **95 testes unitários (100% aprovados)**
- **5-7 testes por bandeira** (total de 60+ testes de identificação)
- **Validação de Luhn** para números válidos e inválidos
- **Testes de formato** (espaços, hífens, caracteres inválidos)
- **Mascaramento de cartões**
- **Edge cases** (comprimentos diferentes, BINs limítrofes)

Exemplo de casos de teste:

- ✅ Cartão válido com BIN correto
- ✅ Cartão com falha na validação de Luhn
- ✅ Diferentes ranges de BIN para mesma bandeira
- ✅ Formatos com espaços e hífens
- ✅ Números com caracteres inválidos
- ✅ Comprimentos variados (13, 15, 16, 19 dígitos)

---

## 💡 Exemplos

### cURL

```bash
curl -X POST "https://localhost:7000/api/card/identify" \
  -H "Content-Type: application/json" \
  -d '{"cardNumber":"5105105105105100"}'
```

### PowerShell

```powershell
$body = @{
    cardNumber = "378282246310005"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7000/api/card/identify" `
  -Method Post `
  -ContentType "application/json" `
  -Body $body
```

### C# (HttpClient)

```csharp
using var client = new HttpClient();
var request = new
{
    cardNumber = "6011000000000004"
};

var response = await client.PostAsJsonAsync(
    "https://localhost:7000/api/card/identify",
    request
);

var result = await response.Content.ReadFromJsonAsync<CardIdentificationResponse>();
Console.WriteLine($"Bandeira: {result.Brand}, Válido: {result.IsValid}");
```

### JavaScript (Fetch)

```javascript
const response = await fetch("https://localhost:7000/api/card/identify", {
  method: "POST",
  headers: {
    "Content-Type": "application/json",
  },
  body: JSON.stringify({
    cardNumber: "4111111111111111",
  }),
});

const data = await response.json();
console.log(`Bandeira: ${data.brand}, Válido: ${data.isValid}`);
```

---

## 📊 Algoritmo de Luhn

O **algoritmo de Luhn** (também conhecido como módulo 10) é usado para validar números de cartão de crédito:

1. A partir do último dígito, percorra os dígitos de trás para frente
2. Duplique o valor de cada segundo dígito
3. Se o resultado for maior que 9, subtraia 9
4. Some todos os dígitos
5. Se a soma for divisível por 10, o número é válido

### Exemplo: Validação do cartão 4111111111111111

```
Dígitos:    4  1  1  1  1  1  1  1  1  1  1  1  1  1  1  1
Alternar:   -  2x -  2x -  2x -  2x -  2x -  2x -  2x -  2x
Dobrados:   4  2  1  2  1  2  1  2  1  2  1  2  1  2  1  2
Soma: 4+2+1+2+1+2+1+2+1+2+1+2+1+2+1+2 = 30
30 % 10 = 0 ✅ VÁLIDO
```

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

### Diretrizes

- Mantenha a cobertura de testes alta
- Siga os padrões de código C#
- Documente novas funcionalidades
- Adicione testes para novos códigos

---

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 👨‍💻 Autor

Gleriston Castro - Desenvolvido como parte do bootcamp **DIO - Digital Innovation One**

---

## 📞 Suporte

Para questões e suporte:

- Abra uma [issue](https://github.com/seu-usuario/dio-identify-brand-cred-card/issues)
- Entre em contato através do [DIO](https://www.dio.me)

---

**⭐ Se este projeto foi útil, considere dar uma estrela no GitHub!**
