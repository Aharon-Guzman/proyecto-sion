using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sion.PL.Pages
{
    public class DonacionesModel : PageModel
    {
        private readonly ILogger<DonacionesModel> _logger;

        public int? Monto { get; set; }

        public DonacionesModel(ILogger<DonacionesModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromQuery] int? monto)
        {
            Monto = monto;
            _logger.LogInformation("Página de donaciones cargada. Monto sugerido: {Monto}", monto);
        }
    }
}