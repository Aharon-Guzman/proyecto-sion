using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Sion.PL.Pages.Admin.Configuracion
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguracionSitioService _config;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<IndexModel> _logger;

        // ── Hero ─────────────────────────────────────────────────────────────
        [BindProperty] public string HeroTitulo    { get; set; } = string.Empty;
        [BindProperty] public string HeroSubtitulo { get; set; } = string.Empty;
        [BindProperty] public IFormFile? HeroImagen { get; set; }
        public string HeroImagenActual { get; set; } = string.Empty;

        // ── Contadores ───────────────────────────────────────────────────────
        [BindProperty] public string ContadorBeneficiados { get; set; } = string.Empty;
        [BindProperty] public string ContadorDirectos     { get; set; } = string.Empty;
        [BindProperty] public string ContadorAnios        { get; set; } = string.Empty;
        [BindProperty] public string ContadorProgramas    { get; set; } = string.Empty;

        // ── CTA ──────────────────────────────────────────────────────────────
        [BindProperty] public string CtaTitulo    { get; set; } = string.Empty;
        [BindProperty] public string CtaSubtitulo { get; set; } = string.Empty;

        [TempData] public string? ToastMensaje { get; set; }
        [TempData] public string? ToastTipo    { get; set; }

        public IndexModel(
            IConfiguracionSitioService config,
            IWebHostEnvironment env,
            ILogger<IndexModel> logger)
        {
            _config = config;
            _env    = env;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            ViewData["ActivePage"] = "Configuracion";
            ViewData["Title"]      = "Contenido Fijo";
            ViewData["TopbarSub"]  = "Hero, contadores y CTA de donación — secciones siempre visibles";

            await CargarValoresAsync();
        }

        // ── Guardar Hero ─────────────────────────────────────────────────────
        public async Task<IActionResult> OnPostHeroAsync()
        {
            var email = User.Identity?.Name ?? "admin";

            await _config.UpdateAsync(new ConfiguracionSitioViewModel
            {
                Id    = await GetIdAsync("Hero:Titulo"),
                Clave = "Hero:Titulo",
                Valor = HeroTitulo
            }, email);

            await _config.UpdateAsync(new ConfiguracionSitioViewModel
            {
                Id    = await GetIdAsync("Hero:Subtitulo"),
                Clave = "Hero:Subtitulo",
                Valor = HeroSubtitulo
            }, email);

            // Imagen opcional
            if (HeroImagen != null && HeroImagen.Length > 0)
            {
                var ruta = await GuardarImagenHeroAsync(HeroImagen);
                if (ruta != null)
                {
                    await _config.UpdateAsync(new ConfiguracionSitioViewModel
                    {
                        Id    = await GetIdAsync("Hero:ImagenFondo"),
                        Clave = "Hero:ImagenFondo",
                        Valor = ruta
                    }, email);
                }
            }

            ToastMensaje = "Hero actualizado correctamente.";
            ToastTipo    = "ok";
            return RedirectToPage("/Admin/Configuracion/Index");
        }

        // ── Guardar Contadores ───────────────────────────────────────────────
        public async Task<IActionResult> OnPostContadoresAsync()
        {
            var email = User.Identity?.Name ?? "admin";

            await _config.UpdateAsync(new ConfiguracionSitioViewModel { Id = await GetIdAsync("Contador:Beneficiados"), Clave = "Contador:Beneficiados", Valor = ContadorBeneficiados }, email);
            await _config.UpdateAsync(new ConfiguracionSitioViewModel { Id = await GetIdAsync("Contador:Directos"),     Clave = "Contador:Directos",     Valor = ContadorDirectos     }, email);
            await _config.UpdateAsync(new ConfiguracionSitioViewModel { Id = await GetIdAsync("Contador:Anios"),        Clave = "Contador:Anios",        Valor = ContadorAnios        }, email);
            await _config.UpdateAsync(new ConfiguracionSitioViewModel { Id = await GetIdAsync("Contador:Programas"),    Clave = "Contador:Programas",    Valor = ContadorProgramas    }, email);

            ToastMensaje = "Contadores actualizados correctamente.";
            ToastTipo    = "ok";
            return RedirectToPage("/Admin/Configuracion/Index");
        }

        // ── Guardar CTA ──────────────────────────────────────────────────────
        public async Task<IActionResult> OnPostCtaAsync()
        {
            var email = User.Identity?.Name ?? "admin";

            await _config.UpdateAsync(new ConfiguracionSitioViewModel { Id = await GetIdAsync("CTA:Titulo"),    Clave = "CTA:Titulo",    Valor = CtaTitulo    }, email);
            await _config.UpdateAsync(new ConfiguracionSitioViewModel { Id = await GetIdAsync("CTA:Subtitulo"), Clave = "CTA:Subtitulo", Valor = CtaSubtitulo }, email);

            ToastMensaje = "CTA actualizado correctamente.";
            ToastTipo    = "ok";
            return RedirectToPage("/Admin/Configuracion/Index");
        }

        // ── Helpers ──────────────────────────────────────────────────────────
        private async Task CargarValoresAsync()
        {
            HeroTitulo            = (await _config.GetByClaveAsync("Hero:Titulo"))?.Valor        ?? string.Empty;
            HeroSubtitulo         = (await _config.GetByClaveAsync("Hero:Subtitulo"))?.Valor     ?? string.Empty;
            HeroImagenActual      = (await _config.GetByClaveAsync("Hero:ImagenFondo"))?.Valor   ?? string.Empty;
            ContadorBeneficiados  = (await _config.GetByClaveAsync("Contador:Beneficiados"))?.Valor ?? "700";
            ContadorDirectos      = (await _config.GetByClaveAsync("Contador:Directos"))?.Valor     ?? "200";
            ContadorAnios         = (await _config.GetByClaveAsync("Contador:Anios"))?.Valor        ?? "17";
            ContadorProgramas     = (await _config.GetByClaveAsync("Contador:Programas"))?.Valor    ?? "10";
            CtaTitulo             = (await _config.GetByClaveAsync("CTA:Titulo"))?.Valor    ?? string.Empty;
            CtaSubtitulo          = (await _config.GetByClaveAsync("CTA:Subtitulo"))?.Valor ?? string.Empty;
        }

        private async Task<int> GetIdAsync(string clave)
        {
            var vm = await _config.GetByClaveAsync(clave);
            return vm?.Id ?? 0;
        }

        private async Task<string?> GuardarImagenHeroAsync(IFormFile archivo)
        {
            try
            {
                var ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
                var permitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (!permitidas.Contains(ext) || archivo.Length > 5 * 1024 * 1024) return null;

                var carpeta = Path.Combine(_env.WebRootPath, "uploads", "hero");
                Directory.CreateDirectory(carpeta);
                var ruta = Path.Combine(carpeta, "hero.webp");

                using var stream = archivo.OpenReadStream();
                using var image  = await SixLabors.ImageSharp.Image.LoadAsync(stream);
                if (image.Width > 1920)
                {
                    var ratio = 1920.0 / image.Width;
                    image.Mutate(x => x.Resize(1920, (int)(image.Height * ratio)));
                }
                await image.SaveAsWebpAsync(ruta);

                return "/uploads/hero/hero.webp";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando imagen hero");
                return null;
            }
        }
    }
}
