using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Services
{

    public class DonacionService : IDonacionService
    {
        private readonly IDonacionRepository _repository;
        private readonly ILogAuditoriaService _auditoria;

        public DonacionService(
            IDonacionRepository repository,
            ILogAuditoriaService auditoria)
        {
            _repository = repository;
            _auditoria = auditoria;
        }

        public async Task<IEnumerable<DonacionViewModel>> GetAllAsync()
        {
            var entidades = await _repository.GetAllAsync();
            return entidades.Select(MapToViewModel);
        }

        public async Task<IEnumerable<DonacionViewModel>> GetByFechasAsync(DateTime desde, DateTime hasta)
        {
            var entidades = await _repository.GetByFechasAsync(desde, hasta);
            return entidades.Select(MapToViewModel);
        }

        public async Task RegistrarAsync(DonacionViewModel viewModel)
        {
            var existe = await _repository.GetAllAsync();
            if (existe.Any(d => d.TransaccionPaypalId == viewModel.TransaccionPaypalId))
                return;

            var entidad = new Donacion
            {
                TransaccionPaypalId = viewModel.TransaccionPaypalId,
                Monto = viewModel.Monto,
                Moneda = viewModel.Moneda,
                NombreDonante = viewModel.NombreDonante,
                EmailDonante = viewModel.EmailDonante,
                EsRecurrente = viewModel.EsRecurrente,
                FechaRegistro = DateTime.UtcNow,
                Estado = viewModel.Estado
            };

            await _repository.AddAsync(entidad);
            await _auditoria.RegistrarAsync(
                "Registrar",
                "Donacion",
                viewModel.EmailDonante ?? "anonimo",
                $"Donación ${entidad.Monto} {entidad.Moneda} — PayPal ID: {entidad.TransaccionPaypalId}"
            );
        }

        private static DonacionViewModel MapToViewModel(Donacion e) => new()
        {
            Id = e.Id,
            TransaccionPaypalId = e.TransaccionPaypalId,
            Monto = e.Monto,
            Moneda = e.Moneda,
            NombreDonante = e.NombreDonante,
            EmailDonante = e.EmailDonante,
            EsRecurrente = e.EsRecurrente,
            FechaRegistro = e.FechaRegistro,
            Estado = e.Estado
        };
    }
}
