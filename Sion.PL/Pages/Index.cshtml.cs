using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;

namespace Sion.PL.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracionSitioService _config;  
        private readonly ISeccionHomeService _seccionHomeService;
        private readonly ILogger<IndexModel> _logger;
        public string HeroTitulo { get; set; } = string.Empty;
        public string HeroSubtitulo { get; set; } = string.Empty;
        public int ContadorBeneficiados { get; set; }
        public int ContadorDirectos { get; set; }
        public int ContadorAnios { get; set; }
        public int ContadorProgramas { get; set; }
        public IEnumerable<SeccionHomeViewModel> Secciones { get; set; } = [];
        public string CtaTitulo { get; set; } = string.Empty;
        public string CtaSubtitulo { get; set; } = string.Empty;

        public IndexModel(IConfiguracionSitioService config, ISeccionHomeService seccionHomeService, ILogger<IndexModel> logger)
        {
            _config = config;
            _seccionHomeService = seccionHomeService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var titulo = await _config.GetByClaveAsync("Hero:Titulo");
            var subtitulo = await _config.GetByClaveAsync("Hero:Subtitulo");
            var beneficiados = await _config.GetByClaveAsync("Contador:Beneficiados");
            var directos = await _config.GetByClaveAsync("Contador:Directos");
            var anios = await _config.GetByClaveAsync("Contador:Anios");
            var programas = await _config.GetByClaveAsync("Contador:Programas");
            var ctaTitulo = await _config.GetByClaveAsync("CTA:Titulo");
            var ctaSubtitulo = await _config.GetByClaveAsync("CTA:Subtitulo");

            HeroTitulo = titulo?.Valor ?? "Transformando vidas en Tres Ríos desde 2008";
            HeroSubtitulo = subtitulo?.Valor ?? "Somos una organización cristiana comprometida con el desarrollo integral de familias vulnerables en Costa Rica.";
            CtaTitulo = ctaTitulo?.Valor ?? "Tu donación cambia vidas";
            CtaSubtitulo = ctaSubtitulo?.Valor ?? "Cada aporte nos permite mantener nuestros programas y llegar a más familias en Concepción de Tres Ríos.";

            int.TryParse(beneficiados?.Valor, out int b);ContadorBeneficiados = b == 0 ? 700 : b;
            int.TryParse(directos?.Valor, out int d);ContadorDirectos = d == 0 ? 200 : d;
            int.TryParse(anios?.Valor, out int a);ContadorAnios = a == 0 ? 17 : a;
            int.TryParse(programas?.Valor, out int p);ContadorProgramas = p == 0 ? 10 : p;

            Secciones = await _seccionHomeService.GetActivasAsync();

            _logger.LogInformation("Página de inicio cargada con título: {HeroTitulo} y subtítulo: {HeroSubtitulo}", HeroTitulo, HeroSubtitulo);
        }
    }
}
