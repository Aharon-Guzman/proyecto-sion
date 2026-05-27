using Microsoft.Extensions.Caching.Memory;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sion.BLL.Services
{
    public class SeccionHomeService : ISeccionHomeService
    {
        private readonly ISeccionHomeRepository _repository;
        private readonly ILogAuditoriaService _auditoria;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SeccionHomeService> _logger;
        private const string CacheKey = "secciones_activas";

        public SeccionHomeService(
            ISeccionHomeRepository repository,
            ILogAuditoriaService auditoria,
            IMemoryCache cache,
            ILogger<SeccionHomeService> logger)
        {
            _repository = repository;
            _auditoria = auditoria;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<SeccionHomeViewModel>> GetAllAsync()
        {
            var entidades = await _repository.GetAllAsync();
            return entidades.Select(MapToViewModel);
        }

        public async Task<IEnumerable<SeccionHomeViewModel>> GetActivasAsync()
        {
            if (_cache.TryGetValue(CacheKey, out IEnumerable<SeccionHomeViewModel>? cached) && cached != null)
                return cached;

            var entidades = await _repository.GetActivasAsync();
            var viewModels = entidades.Select(MapToViewModel).ToList();
            _logger.LogInformation("Cache miss - cargando secciones activas desde DB");
            _cache.Set(CacheKey, viewModels, TimeSpan.FromMinutes(30));
            return viewModels;
        }

        public async Task<SeccionHomeViewModel?> GetByIdAsync(int id)
        {
            var entidad = await _repository.GetByIdAsync(id);
            return entidad == null ? null : MapToViewModel(entidad);
        }

        public async Task CreateAsync(SeccionHomeViewModel viewModel, string usuarioEmail)
        {
            var entidad = new SeccionHome
            {
                Titulo = viewModel.Titulo,
                Descripcion = viewModel.Descripcion,
                RutaImagen = viewModel.RutaImagen,
                Estilo = viewModel.Estilo,
                Orden = viewModel.Orden,
                EstaActiva = viewModel.EstaActiva,
                FechaModificacion = DateTime.UtcNow
            };

            await _repository.AddAsync(entidad);
            _logger.LogInformation("Sección '{Titulo}' creada por {Usuario}", entidad.Titulo, usuarioEmail);
            _cache.Remove(CacheKey);
            await _auditoria.RegistrarAsync("Crear", "SeccionHome", usuarioEmail, $"Sección '{entidad.Titulo}' creada");
        }

        public async Task UpdateAsync(SeccionHomeViewModel viewModel, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(viewModel.Id);
            if (entidad == null) return;

            entidad.Titulo = viewModel.Titulo;
            entidad.Descripcion = viewModel.Descripcion;
            entidad.RutaImagen = viewModel.RutaImagen;
            entidad.Estilo = viewModel.Estilo;
            entidad.Orden = viewModel.Orden;
            entidad.EstaActiva = viewModel.EstaActiva;
            entidad.FechaModificacion = DateTime.UtcNow;

            await _repository.UpdateAsync(entidad);
            _cache.Remove(CacheKey);
            await _auditoria.RegistrarAsync("Actualizar", "SeccionHome", usuarioEmail, $"Sección '{entidad.Titulo}' actualizada");
        }

        public async Task DeleteAsync(int id, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(id);
            if (entidad == null) return;

            await _repository.DeleteAsync(id);
            _logger.LogInformation("Sección '{Titulo}' eliminada por {Usuario}", entidad.Titulo, usuarioEmail);
            _cache.Remove(CacheKey);
            await _auditoria.RegistrarAsync("Eliminar", "SeccionHome", usuarioEmail, $"Sección '{entidad.Titulo}' eliminada");
        }

        public async Task ToggleActivaAsync(int id, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(id);
            if (entidad == null) return;

            entidad.EstaActiva = !entidad.EstaActiva;
            entidad.FechaModificacion = DateTime.UtcNow;

            await _repository.UpdateAsync(entidad);
            _cache.Remove(CacheKey);
            await _auditoria.RegistrarAsync("Toggle", "SeccionHome", usuarioEmail, $"Sección '{entidad.Titulo}' → activa: {entidad.EstaActiva}");
        }

        private static SeccionHomeViewModel MapToViewModel(SeccionHome e) => new()
        {
            Id = e.Id,
            Titulo = e.Titulo,
            Descripcion = e.Descripcion,
            RutaImagen = e.RutaImagen,
            Estilo = e.Estilo,
            Orden = e.Orden,
            EstaActiva = e.EstaActiva
        };
    }
}
