using Sion.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sion.BLL.Interfaces
{

    public interface IImagenGaleriaService
    {
        Task<IEnumerable<ImagenGaleriaViewModel>> GetAllAsync();
        Task<IEnumerable<ImagenGaleriaViewModel>> GetActivasAsync();
        Task<ImagenGaleriaViewModel?> GetByIdAsync(int id);
        Task SubirAsync(IFormFile archivo, string titulo, string usuarioEmail);
        Task UpdateAsync(ImagenGaleriaViewModel viewModel, string usuarioEmail);
        Task DeleteAsync(int id, string usuarioEmail);
        Task ToggleActivaAsync(int id, string usuarioEmail);
    }
}
