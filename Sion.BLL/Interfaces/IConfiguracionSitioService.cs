using Sion.BLL.ViewModels;

namespace Sion.BLL.Interfaces
{
    public interface IConfiguracionSitioService
    {
        Task<IEnumerable<ConfiguracionSitioViewModel>> GetAllAsync();
        Task<ConfiguracionSitioViewModel?> GetByClaveAsync(string clave);
        Task UpdateAsync(ConfiguracionSitioViewModel viewModel, string usuarioEmail);
        void InvalidarCache();
    }
}