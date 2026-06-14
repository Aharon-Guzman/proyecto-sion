using Sion.BLL.ViewModels;

namespace Sion.BLL.Interfaces
{
    public interface ILogAuditoriaService
    {
        Task RegistrarAsync(string accion, string entidadAfectada, string usuarioEmail, string detalle);
        Task<IEnumerable<LogAuditoriaViewModel>> GetAllAsync();
    }
}
