using Sion.BLL.Interfaces;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                FechaHora = DateTime.UtcNow,
                Detalle = detalle
            };

            await _repository.AddAsync(log);
        }
    }
}
