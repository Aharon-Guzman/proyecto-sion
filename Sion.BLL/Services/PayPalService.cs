using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Sion.BLL.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<PayPalService> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _baseUrl;   // sandbox vs live

        public PayPalService(
            IHttpClientFactory httpFactory,
            IConfiguration config,
            ILogger<PayPalService> logger)
        {
            _httpFactory  = httpFactory;
            _logger       = logger;
            _clientId     = config["PayPal:ClientId"]     ?? throw new InvalidOperationException("PayPal:ClientId no configurado");
            _clientSecret = config["PayPal:ClientSecret"] ?? throw new InvalidOperationException("PayPal:ClientSecret no configurado");

            var modo = config["PayPal:Mode"] ?? "sandbox";
            _baseUrl = modo == "live"
                ? "https://api-m.paypal.com"
                : "https://api-m.sandbox.paypal.com";
        }

        // ─────────────────────────────────────────────────────────────────────
        // PÚBLICO
        // ─────────────────────────────────────────────────────────────────────

        public async Task<string> CrearOrdenAsync(decimal monto, string moneda = "USD")
        {
            var token = await ObtenerTokenAsync();
            var client = _httpFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = moneda,
                            value         = monto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                        },
                        description = "Donación Proyecto Sion"
                    }
                }
            };

            var json     = JsonSerializer.Serialize(body);
            var content  = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseUrl}/v2/checkout/orders", content);
            var raw      = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("PayPal CrearOrden falló {Status}: {Body}", response.StatusCode, raw);
                throw new InvalidOperationException("Error al crear la orden en PayPal.");
            }

            using var doc = JsonDocument.Parse(raw);
            return doc.RootElement.GetProperty("id").GetString()
                   ?? throw new InvalidOperationException("PayPal no retornó ID de orden.");
        }

        public async Task<PayPalCaptureResult> CapturarOrdenAsync(string orderId)
        {
            var token = await ObtenerTokenAsync();
            var client = _httpFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Body vacío — PayPal solo necesita el orderId en la URL
            var content  = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(
                $"{_baseUrl}/v2/checkout/orders/{orderId}/capture", content);
            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("PayPal CapturarOrden falló {Status}: {Body}", response.StatusCode, raw);
                return new PayPalCaptureResult
                {
                    Exitoso      = false,
                    OrderId      = orderId,
                    ErrorMensaje = "El pago no pudo ser procesado por PayPal."
                };
            }

            using var doc  = JsonDocument.Parse(raw);
            var root       = doc.RootElement;
            var estado     = root.GetProperty("status").GetString() ?? "";
            var exitoso    = estado == "COMPLETED";

            // Extraer monto y moneda de purchase_units[0].payments.captures[0]
            decimal monto  = 0;
            string  moneda = "USD";
            if (root.TryGetProperty("purchase_units", out var units) && units.GetArrayLength() > 0)
            {
                var unit = units[0];
                if (unit.TryGetProperty("payments", out var pagos) &&
                    pagos.TryGetProperty("captures", out var capturas) &&
                    capturas.GetArrayLength() > 0)
                {
                    var captura = capturas[0];
                    if (captura.TryGetProperty("amount", out var amt))
                    {
                        decimal.TryParse(
                            amt.GetProperty("value").GetString(),
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out monto);
                        moneda = amt.GetProperty("currency_code").GetString() ?? "USD";
                    }
                }
            }

            // Extraer datos del donante desde payer
            string? nombre = null;
            string? email  = null;
            if (root.TryGetProperty("payer", out var payer))
            {
                email = payer.TryGetProperty("email_address", out var em) ? em.GetString() : null;
                if (payer.TryGetProperty("name", out var name))
                {
                    var given   = name.TryGetProperty("given_name", out var gn) ? gn.GetString() : "";
                    var surname = name.TryGetProperty("surname",    out var sn) ? sn.GetString() : "";
                    nombre = $"{given} {surname}".Trim();
                }
            }

            return new PayPalCaptureResult
            {
                Exitoso       = exitoso,
                OrderId       = orderId,
                Monto         = monto,
                Moneda        = moneda,
                NombreDonante = nombre,
                EmailDonante  = email,
                Estado        = estado
            };
        }

        // ─────────────────────────────────────────────────────────────────────
        // PRIVADO
        // ─────────────────────────────────────────────────────────────────────

        private async Task<string> ObtenerTokenAsync()
        {
            var client = _httpFactory.CreateClient();

            // Basic Auth: Base64(ClientId:Secret)
            var credenciales = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credenciales);

            var body     = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            ]);
            var response = await client.PostAsync($"{_baseUrl}/v1/oauth2/token", body);
            var raw      = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("PayPal token falló {Status}: {Body}", response.StatusCode, raw);
                throw new InvalidOperationException("No se pudo autenticar con PayPal.");
            }

            using var doc = JsonDocument.Parse(raw);
            return doc.RootElement.GetProperty("access_token").GetString()
                   ?? throw new InvalidOperationException("PayPal no retornó access_token.");
        }
    }
}
