using Sion.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Interfaces
{
    public interface ISeccionHomeService
    {
        Task<IEnumerable<SeccionHomeViewModel>> GetAllAsync();
        Task<IEnumerable<SeccionHomeViewModel>> GetActivasAsync();
        Task<SeccionHomeViewModel?> GetByIdAsync(int id);
        Task CreateAsync(SeccionHomeViewModel viewModel, string usuarioEmail);
        Task UpdateAsync(SeccionHomeViewModel viewModel, string usuarioEmail);
        Task DeleteAsync(int id, string usuarioEmail);
        Task ToggleActivaAsync(int id, string usuarioEmail);
    }
}
