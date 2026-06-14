using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Interfaces
{
    public interface ICorreoService
    {
        Task EnviarContactoAsync(string nombre, string email, string? telefono, string mensaje);
    }
}
