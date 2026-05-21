using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Entities
{
    public class SeccionHome
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? RutaImagen { get; set; }
        public string Estilo { get; set; } = string.Empty;
        public int Orden { get; set; }
        public bool EstaActiva { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
