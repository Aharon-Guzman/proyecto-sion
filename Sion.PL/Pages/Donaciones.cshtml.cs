using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using System.Text.Json;

namespace Sion.PL.Pages
{
    public class DonacionesModel : PageModel
    {
        private readonly IPayPalService    _paypal;
        private readonly IDonacionService  _donaciones;
        private readonly ILogger<DonacionesModel> _logger;

        // Client ID expuesto a la vista para el script del SDK de PayPal
        public string PayPalClientId { get; private set; } = string.Empty;

        public int? Monto { get; set; }

        public DonacionesModel(
            IPayPalService paypal,
            IDonacionService donaciones,
            IConfiguration config,
            ILogger<DonacionesModel> logger)
        {
            _paypal     = paypal;
            _donaciones = donaciones;
            _logger     = logger;
            PayPalClientId = config["PayPal:ClientId"] ?? string.Empty;
        }

        public void OnGet([FromQuery] int? monto)
        {
            Monto = monto;
        }

        // ── Paso 1: crear orden en PayPal ─────────────────────────────────────
        // El JS SDK llama a este endpoint y espera { orderId: "..." }
        public async Task<IActionResult> OnPostCrearOrdenAsync([FromBody] CrearOrdenRequest request)
        {
            if (request.Monto <= 0 || request.Monto > 10000)
                return new JsonResult(new { error = "Monto inválido." }) { StatusCode = 400 };

            try
            {
                var orderId = await _paypal.CrearOrdenAsync(request.Monto);
                return new JsonResult(new { orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando orden PayPal por ${Monto}", request.Monto);
                return new JsonResult(new { error = "No se pudo iniciar el pago." }) { StatusCode = 500 };
            }
        }

        // ── Paso 2: capturar pago aprobado y guardar en BD ───────────────────
        // El JS SDK llama a este endpoint después de que el usuario aprueba
        public async Task<IActionResult> OnPostCapturarOrdenAsync([FromBody] CapturarOrdenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OrderId))
                return new JsonResult(new { exitoso = false, error = "OrderId requerido." }) { StatusCode = 400 };

            try
            {
                var resultado = await _paypal.CapturarOrdenAsync(request.OrderId);

                if (resultado.Exitoso)
                {
                    await _donaciones.RegistrarAsync(new DonacionViewModel
                    {
                        TransaccionPaypalId = resultado.OrderId,
                        Monto               = resultado.Monto,
                        Moneda              = resultado.Moneda,
                        NombreDonante       = resultado.NombreDonante,
                        EmailDonante        = resultado.EmailDonante,
                        Estado              = resultado.Estado,
                        EsRecurrente        = false
                    });

                    _logger.LogInformation(
                        "Donación registrada: ${Monto} {Moneda} — PayPal {OrderId}",
                        resultado.Monto, resultado.Moneda, resultado.OrderId);
                }

                return new JsonResult(new
                {
                    exitoso       = resultado.Exitoso,
                    monto         = resultado.Monto,
                    moneda        = resultado.Moneda,
                    nombreDonante = resultado.NombreDonante,
                    error         = resultado.ErrorMensaje
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error capturando orden PayPal {OrderId}", request.OrderId);
                return new JsonResult(new { exitoso = false, error = "Error al procesar el pago." }) { StatusCode = 500 };
            }
        }
    }

    // ── DTOs para los requests del JS SDK ────────────────────────────────────
    public record CrearOrdenRequest(decimal Monto);
    public record CapturarOrdenRequest(string OrderId);
}
