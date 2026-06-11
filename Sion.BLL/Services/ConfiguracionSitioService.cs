using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Sion.DAL.Interfaces;

namespace Sion.BLL.Services
{
    public class ConfiguracionSitioService : IConfiguracionSitioService
    {
        private readonly IConfiguracionSitioRepository _repository;
        private readonly ILogAuditoriaService _auditoria;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ConfiguracionSitioService> _logger;

        private const string CacheKeyAll = "config_sitio_all";
        private const string CacheKeyPrefx = "config_sitio_";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public ConfiguracionSitioService(
            IConfiguracionSitioRepository repository,
            ILogAuditoriaService auditoria,
            IMemoryCache cache,
            ILogger<ConfiguracionSitioService> logger)
        {
            _repository = repository;
            _auditoria = auditoria;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<ConfiguracionSitioViewModel>> GetAllAsync()
        {
            if (_cache.TryGetValue(CacheKeyAll, out IEnumerable<ConfiguracionSitioViewModel>? cached) && cached != null)
                return cached;

            var entidades = await _repository.GetAllAsync();
            var resultado = entidades.Select(MapToViewModel).ToList();

            _cache.Set(CacheKeyAll, resultado, CacheDuration);
            _logger.LogInformation("ConfiguracionSitio cargada desde BD y cacheada por {Min} min.", CacheDuration.TotalMinutes);

            return resultado;
        }

        public async Task<ConfiguracionSitioViewModel?> GetByClaveAsync(string clave)
        {
            var key = CacheKeyPrefx + clave;

            if (_cache.TryGetValue(key, out ConfiguracionSitioViewModel? cached))
                return cached;

            var entidad = await _repository.GetByClaveAsync(clave);
            if (entidad == null) return null;

            var resultado = MapToViewModel(entidad);
            _cache.Set(key, resultado, CacheDuration);

            return resultado;
        }

        public async Task UpdateAsync(ConfiguracionSitioViewModel viewModel, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(viewModel.Id);
            if (entidad == null) return;

            entidad.Valor = viewModel.Valor;

            await _repository.UpdateAsync(entidad);
            await _auditoria.RegistrarAsync(
                "Actualizar",
                $"ConfiguracionSitio:{entidad.Clave}",
                usuarioEmail,
                $"Valor actualizado a '{entidad.Valor}'"
            );

            // Invalida cache para que el cambio sea visible de inmediato
            InvalidarCache();
            _logger.LogInformation("Cache de ConfiguracionSitio invalidado tras actualizar '{Clave}'.", entidad.Clave);
        }

        public void InvalidarCache()
        {
            _cache.Remove(CacheKeyAll);

            // Invalida todas las claves individuales conocidas
            var claves = new[]
            {
                "Hero:Titulo", "Hero:Subtitulo", "Hero:ImagenFondo",
                "Contador:Beneficiados", "Contador:Directos",
                "Contador:Anios", "Contador:Programas",
                "CTA:Titulo", "CTA:Subtitulo"
            };

            foreach (var clave in claves)
                _cache.Remove(CacheKeyPrefx + clave);
        }

        private static ConfiguracionSitioViewModel MapToViewModel(DAL.Entities.ConfiguracionSitio e) => new()
        {
            Id = e.Id,
            Clave = e.Clave,
            Valor = e.Valor,
            Descripcion = e.Descripcion
        };
    }
}