using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using System.Globalization;
using System.Text;

namespace Sion.PL.Pages.Admin.Donaciones
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IDonacionService _donaciones;
        private readonly ILogger<IndexModel> _logger;

        private const int PageSize = 50;

        public IEnumerable<DonacionViewModel> Donaciones { get; set; } = [];
        public int     TotalDonaciones { get; set; }
        public decimal MontoTotal      { get; set; }
        public int     TotalPaginas    { get; set; }

        [BindProperty(SupportsGet = true)] public DateTime? Desde  { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? Hasta  { get; set; }
        [BindProperty(SupportsGet = true)] public int       Pagina { get; set; } = 1;

        public IndexModel(IDonacionService donaciones, ILogger<IndexModel> logger)
        {
            _donaciones = donaciones;
            _logger     = logger;
        }

        public async Task OnGetAsync()
        {
            ModelState.Clear();
            ViewData["ActivePage"] = "Donaciones";
            ViewData["Title"]      = "Donaciones";
            ViewData["TopbarSub"]  = "Historial de donaciones recibidas vía PayPal";

            var desde = Desde ?? DateTime.UtcNow.AddDays(-30).Date;
            var hasta = (Hasta ?? DateTime.UtcNow.Date).AddDays(1).AddTicks(-1);

            var todas = (await _donaciones.GetByFechasAsync(desde, hasta))
                            .OrderByDescending(d => d.FechaRegistro)
                            .ToList();

            TotalDonaciones = todas.Count;
            MontoTotal      = todas.Sum(d => d.Monto);
            TotalPaginas    = (int)Math.Ceiling(TotalDonaciones / (double)PageSize);
            Pagina          = Math.Clamp(Pagina, 1, Math.Max(1, TotalPaginas));

            Donaciones = todas.Skip((Pagina - 1) * PageSize).Take(PageSize).ToList();

            _logger.LogInformation("Dashboard de donaciones consultado por {Usuario}.", User.Identity?.Name);
        }

        // Handler GET para exportar CSV — ?handler=ExportarCsv&Desde=...&Hasta=...
        public async Task<IActionResult> OnGetExportarCsvAsync()
        {
            var desde = Desde ?? DateTime.UtcNow.AddDays(-30).Date;
            var hasta = (Hasta ?? DateTime.UtcNow.Date).AddDays(1).AddTicks(-1);

            var lista = (await _donaciones.GetByFechasAsync(desde, hasta))
                            .OrderByDescending(d => d.FechaRegistro)
                            .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("TransaccionPayPal,Donante,Email,Monto,Moneda,Tipo,Fecha,Estado");

            foreach (var d in lista)
            {
                sb.AppendLine(string.Join(",",
                    $"\"{d.TransaccionPaypalId}\"",
                    $"\"{(d.NombreDonante ?? "Anónimo").Replace("\"", "\"\"")}\"",
                    $"\"{d.EmailDonante ?? "—"}\"",
                    d.Monto.ToString("F2", CultureInfo.InvariantCulture),
                    $"\"{d.Moneda}\"",
                    $"\"{(d.EsRecurrente ? "Mensual" : "Única")}\"",
                    $"\"{d.FechaRegistro.ToLocalTime():dd/MM/yyyy HH:mm}\"",
                    $"\"{d.Estado}\""
                ));
            }

            var bytes    = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
            var filename = $"donaciones_{DateTime.Now:yyyyMMdd}.csv";
            return File(bytes, "text/csv", filename);
        }
    }
}
