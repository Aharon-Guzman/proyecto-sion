using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<string?> GetEmailAsync(string userId);
        Task<bool> EsAdminAsync(string userId);
    }
}
