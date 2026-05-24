using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Interfaces
{
    public interface ILogAuditoriaService
    {
        Task RegistrarAsync(string accion, string entidadAfectada, string usuarioEmail, string detalle);
    }
}
