using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;

namespace Sion.PL.Pages.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IImagenGaleriaService _imagenes;
        private readonly IDonacionService _donaciones;
        private readonly ISeccionHomeService _secciones;
        private readonly ILogger<IndexModel> _logger;

        public int TotalImagenes { get; set; }
        public int TotalSecciones { get; set; }
        public decimal MontoMes { get; set; }
        public int TotalDonacionesMes { get; set; }
        public decimal MontoTotal { get; set; }
        public IEnumerable<DonacionViewModel> UltimasDonaciones { get; set; } = [];

        public IndexModel(
            IImagenGaleriaService imagenes,
            IDonacionService donaciones,
            ISeccionHomeService secciones,
            ILogger<IndexModel> logger)
        {
            _imagenes = imagenes;
            _donaciones = donaciones;
            _secciones = secciones;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var imagenes   = await _imagenes.GetActivasAsync();
            var secciones  = await _secciones.GetActivasAsync();
            var donaciones = await _donaciones.GetAllAsync();

            TotalImagenes  = imagenes.Count();
            TotalSecciones = secciones.Count();

            var donaList   = donaciones.ToList();
            var inicioMes  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var estesMes        = donaList.Where(d => d.FechaRegistro >= inicioMes).ToList();
            MontoMes            = estesMes.Sum(d => d.Monto);
            TotalDonacionesMes  = estesMes.Count;
            MontoTotal          = donaList.Sum(d => d.Monto);
            UltimasDonaciones   = donaList.OrderByDescending(d => d.FechaRegistro).Take(5);

            _logger.LogInformation("Dashboard admin cargado por {Usuario}.", User.Identity?.Name);
        }
    }
}
