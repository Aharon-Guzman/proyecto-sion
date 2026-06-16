using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Microsoft.Extensions.Logging;

namespace Sion.PL.Pages.Admin.Galeria
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IImagenGaleriaService _galeria;
        private readonly ILogger<IndexModel> _logger;

        private const int PageSize = 15;

        // Resultados paginados (lo que se muestra en la tabla)
        public IEnumerable<ImagenGaleriaViewModel> Imagenes { get; set; } = [];

        // Contadores para la vista
        public int TotalImagenes { get; set; }
        public int TotalPaginas  { get; set; }

        // Query params — SupportsGet = true para que funcionen en GET
        // IMPORTANTE: no usar "Page" — es route value reservado de Razor Pages (binding lo ignora)
        [BindProperty(SupportsGet = true)] public int    Pagina   { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Busqueda { get; set; } = string.Empty;

        // Campos del formulario de subida
        [BindProperty] public string     NuevoTitulo  { get; set; } = string.Empty;
        [BindProperty] public IFormFile? ArchivoNuevo { get; set; }

        [TempData] public string? ToastMensaje { get; set; }
        [TempData] public string? ToastTipo    { get; set; }

        public IndexModel(IImagenGaleriaService galeria, ILogger<IndexModel> logger)
        {
            _galeria = galeria;
            _logger  = logger;
        }

        public async Task OnGetAsync()
        {
            ModelState.Clear();

            ViewData["ActivePage"] = "Galeria";
            ViewData["Title"]      = "Galería";
            ViewData["TopbarSub"]  = "Administra las imágenes que aparecen en la página pública de galería";

            var todas = await _galeria.GetAllAsync();

            // Filtrar por título si hay búsqueda activa
            if (!string.IsNullOrWhiteSpace(Busqueda))
                todas = todas.Where(i => i.Titulo.Contains(Busqueda.Trim(), StringComparison.OrdinalIgnoreCase));

            var ordenadas = todas.OrderByDescending(i => i.FechaSubida).ToList();

            TotalImagenes = ordenadas.Count;
            TotalPaginas  = (int)Math.Ceiling(TotalImagenes / (double)PageSize);

            if (Pagina < 1) Pagina = 1;
            if (Pagina > TotalPaginas && TotalPaginas > 0) Pagina = TotalPaginas;

            Imagenes = ordenadas.Skip((Pagina - 1) * PageSize).Take(PageSize);
        }

        public async Task<IActionResult> OnPostSubirAsync()
        {
            if (ArchivoNuevo == null || string.IsNullOrWhiteSpace(NuevoTitulo))
            {
                ToastMensaje = "Debes indicar un título y seleccionar una imagen.";
                ToastTipo    = "error";
                return RedirectToPage("/Admin/Galeria/Index");
            }

            try
            {
                await _galeria.SubirAsync(ArchivoNuevo, NuevoTitulo.Trim(), User.Identity!.Name!);
                ToastMensaje = $"Imagen '{NuevoTitulo.Trim()}' subida correctamente.";
                ToastTipo    = "ok";
            }
            catch (InvalidOperationException ex)
            {
                ToastMensaje = ex.Message;
                ToastTipo    = "error";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al subir imagen '{Titulo}'", NuevoTitulo);
                ToastMensaje = "Ocurrió un error al subir la imagen. Intente de nuevo.";
                ToastTipo    = "error";
            }

            return RedirectToPage("/Admin/Galeria/Index", new { Pagina = 1 });
        }

        public async Task<IActionResult> OnPostToggleAsync(int id, int pagina, string? busqueda)
        {
            var img = await _galeria.GetByIdAsync(id);
            await _galeria.ToggleActivaAsync(id, User.Identity!.Name!);
            if (img != null)
                ToastMensaje = $"'{img.Titulo}' {(img.EstaActiva ? "desactivada" : "activada")}.";
            ToastTipo = "ok";
            return RedirectToPage("/Admin/Galeria/Index", new { Pagina = pagina, Busqueda = busqueda });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, int pagina, string? busqueda)
        {
            var img = await _galeria.GetByIdAsync(id);
            await _galeria.DeleteAsync(id, User.Identity!.Name!);
            ToastMensaje = img != null ? $"Imagen '{img.Titulo}' eliminada." : "Imagen eliminada.";
            ToastTipo    = "ok";
            return RedirectToPage("/Admin/Galeria/Index", new { Pagina = pagina, Busqueda = busqueda });
        }
    }
}
