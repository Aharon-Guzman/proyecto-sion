using Sion.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Interfaces
{
    public interface IConfiguracionSitioService
    {
        Task<IEnumerable<ConfiguracionSitioViewModel>> GetAllAsync();
        Task<ConfiguracionSitioViewModel?> GetByClaveAsync(string clave);
        Task UpdateAsync(ConfiguracionSitioViewModel viewModel, string usuarioEmail);
    }
}
