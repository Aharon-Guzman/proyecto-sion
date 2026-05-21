using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Entities
{
    public class LogAuditoria
    {
        public int Id { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string EntidadAfectada { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }
}
