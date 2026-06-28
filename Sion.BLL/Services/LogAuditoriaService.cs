using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;

namespace Sion.BLL.Services
{
    public class LogAuditoriaService : ILogAuditoriaService
    {
        private readonly ILogAuditoriaRepository _repository;

        public LogAuditoriaService(ILogAuditoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task RegistrarAsync(string accion, string entidadAfectada, string usuarioEmail, string detalle)
        {
            var log = new LogAuditoria
            {
                Accion = accion,
                EntidadAfectada = entidadAfectada,
                UsuarioEmail = usuarioEmail,
                FechaHora = DateTime.Now,
                Detalle = detalle
            };

            await _repository.AddAsync(log);
        }
        public async Task<IEnumerable<LogAuditoriaViewModel>> GetAllAsync()
        {
            var entidades = await _repository.GetAllAsync();
            return entidades.Select(MapToViewModel);
        }

        public async Task<IEnumerable<LogAuditoriaViewModel>> GetByFechasAsync(DateTime desde, DateTime hasta)
        {
            var entidades = await _repository.GetByFechasAsync(desde, hasta);
            return entidades.Select(MapToViewModel);
        }

        private static LogAuditoriaViewModel MapToViewModel(DAL.Entities.LogAuditoria e) => new()
        {
            Id           = e.Id,
            Accion       = e.Accion,
            Entidad      = e.EntidadAfectada,
            UsuarioEmail = e.UsuarioEmail,
            Detalle      = e.Detalle,
            FechaHora    = e.FechaHora
        };
    }
}
