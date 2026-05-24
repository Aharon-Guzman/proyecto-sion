using Sion.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Interfaces
{
    public interface IDonacionService
    {
        Task<IEnumerable<DonacionViewModel>> GetAllAsync();
        Task<IEnumerable<DonacionViewModel>> GetByFechasAsync(DateTime desde, DateTime hasta);
        Task RegistrarAsync(DonacionViewModel viewModel);
    }
}
