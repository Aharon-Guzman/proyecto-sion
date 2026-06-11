using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;

namespace Sion.PL.Pages
{
    public class GaleriaModel : PageModel
    {
        private readonly IImagenGaleriaService _imagenService;
        private readonly ILogger<GaleriaModel> _logger;

        public IEnumerable<ImagenGaleriaViewModel> Imagenes { get; set; } = [];

        public GaleriaModel(IImagenGaleriaService imagenService, ILogger<GaleriaModel> logger)
        {
            _imagenService = imagenService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Imagenes = await _imagenService.GetActivasAsync();
            _logger.LogInformation("Galería cargada con {Total} imágenes.", Imagenes.Count());
        }
    }
}