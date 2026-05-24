using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Sion.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Services
{

    public class ConfiguracionSitioService : IConfiguracionSitioService
    {
        private readonly IConfiguracionSitioRepository _repository;
        private readonly ILogAuditoriaService _auditoria;

        public ConfiguracionSitioService(
            IConfiguracionSitioRepository repository,
            ILogAuditoriaService auditoria)
        {
            _repository = repository;
            _auditoria = auditoria;
        }

        public async Task<IEnumerable<ConfiguracionSitioViewModel>> GetAllAsync()
        {
            var entidades = await _repository.GetAllAsync();
            return entidades.Select(e => new ConfiguracionSitioViewModel
            {
                Id = e.Id,
                Clave = e.Clave,
                Valor = e.Valor,
                Descripcion = e.Descripcion
            });
        }

        public async Task<ConfiguracionSitioViewModel?> GetByClaveAsync(string clave)
        {
            var entidad = await _repository.GetByClaveAsync(clave);
            if (entidad == null) return null;

            return new ConfiguracionSitioViewModel
            {
                Id = entidad.Id,
                Clave = entidad.Clave,
                Valor = entidad.Valor,
                Descripcion = entidad.Descripcion
            };
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
        }
    }
}
