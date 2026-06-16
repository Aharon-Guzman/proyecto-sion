using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Sion.PL.Pages.Admin.Secciones
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ISeccionHomeService _secciones;
        private readonly IWebHostEnvironment _env;

        public IEnumerable<SeccionHomeViewModel> Secciones { get; set; } = [];
        [BindProperty] public SeccionHomeViewModel Seccion { get; set; } = new();
        [BindProperty] public IFormFile? ImagenFile { get; set; }

        [TempData] public string? ToastMensaje { get; set; }
        [TempData] public string? ToastTipo    { get; set; }

        public IndexModel(ISeccionHomeService secciones, IWebHostEnvironment env)
        {
            _secciones = secciones;
            _env = env;
        }

        public async Task OnGetAsync()
        {
            ViewData["ActivePage"] = "Secciones";
            ViewData["Title"] = "Secciones Home";
            ViewData["TopbarSub"] = "Administra las secciones de contenido del sitio publico";

            var todas = (await _secciones.GetAllAsync())
                .OrderBy(s => s.Orden).ThenBy(s => s.Id).ToList();

            // Normalizar ordenes si hay duplicados (limpia estado inconsistente en BD)
            var ordenes = todas.Select(s => s.Orden).ToList();
            if (ordenes.Distinct().Count() != ordenes.Count)
            {
                var ids = todas.Select(s => s.Id).ToList();
                await _secciones.ReordenarAsync(ids, "sistema");
                todas = (await _secciones.GetAllAsync()).OrderBy(s => s.Orden).ToList();
            }

            Secciones = todas;
        }

        // Crear o actualizar — centinela Id < 0 = nueva, Id >= 0 = editar
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (Seccion.Estilo == "TextoSolo")
                ModelState.Remove(nameof(ImagenFile));

            if (!ModelState.IsValid)
            {
                Secciones = (await _secciones.GetAllAsync()).OrderBy(s => s.Orden);
                ViewData["ActivePage"] = "Secciones";
                ViewData["Title"] = "Secciones Home";
                return Page();
            }

            if (ImagenFile != null)
            {
                var ruta = await GuardarImagenAsync(ImagenFile);
                if (ruta == null)
                {
                    ModelState.AddModelError("ImagenFile", "Formato invalido o archivo mayor a 5 MB.");
                    Secciones = (await _secciones.GetAllAsync()).OrderBy(s => s.Orden);
                    ViewData["ActivePage"] = "Secciones";
                    ViewData["Title"] = "Secciones Home";
                    return Page();
                }

                // Si es editar (Id >= 0), borrar imagen anterior del disco
                if (Seccion.Id >= 0 && !string.IsNullOrEmpty(Seccion.RutaImagen))
                {
                    var old = Path.Combine(_env.WebRootPath, Seccion.RutaImagen.TrimStart('/'));
                    if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                }

                Seccion.RutaImagen = ruta;
            }

            if (Seccion.Id < 0)   // -1 es el centinela para "nueva seccion"
            {
                var todas = await _secciones.GetAllAsync();
                Seccion.Orden      = todas.Any() ? todas.Max(s => s.Orden) + 1 : 1;
                Seccion.EstaActiva = true;
                await _secciones.CreateAsync(Seccion, User.Identity!.Name!);
                ToastMensaje = $"Seccion '{Seccion.Titulo}' creada correctamente.";
            }
            else
            {
                await _secciones.UpdateAsync(Seccion, User.Identity!.Name!);
                ToastMensaje = $"Seccion '{Seccion.Titulo}' actualizada.";
            }

            ToastTipo = "ok";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleAsync(int id)
        {
            var sec = await _secciones.GetByIdAsync(id);
            await _secciones.ToggleActivaAsync(id, User.Identity!.Name!);
            if (sec != null)
                ToastMensaje = $"'{sec.Titulo}' {(sec.EstaActiva ? "desactivada" : "activada")}.";
            ToastTipo = "ok";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var sec = await _secciones.GetByIdAsync(id);
            await _secciones.DeleteAsync(id, User.Identity!.Name!);
            ToastMensaje = sec != null ? $"Seccion '{sec.Titulo}' eliminada." : "Seccion eliminada.";
            ToastTipo    = "ok";
            return RedirectToPage();
        }

        // Llamado por AJAX — devuelve 200 OK sin redirect
        public async Task<IActionResult> OnPostReordenarAsync([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest();

            await _secciones.ReordenarAsync(ids, User.Identity!.Name!);
            return new OkResult();
        }

        private async Task<string?> GuardarImagenAsync(IFormFile archivo)
        {
            var ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            var permitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!permitidas.Contains(ext) || archivo.Length > 5 * 1024 * 1024)
                return null;

            var carpeta = Path.Combine(_env.WebRootPath, "uploads", "secciones");
            Directory.CreateDirectory(carpeta);

            // Siempre guardamos como WebP para optimizar peso en disco y red
            var nombre = $"{Guid.NewGuid()}.webp";
            var ruta   = Path.Combine(carpeta, nombre);

            using var stream = archivo.OpenReadStream();
            using var image  = await Image.LoadAsync(stream);

            // Redimensionar si supera 1200 px de ancho (mantiene relación de aspecto)
            if (image.Width > 1200)
            {
                var ratio = 1200.0 / image.Width;
                var newH  = (int)(image.Height * ratio);
                image.Mutate(x => x.Resize(1200, newH));
            }

            await image.SaveAsWebpAsync(ruta);
            return $"/uploads/secciones/{nombre}";
        }
    }
}
