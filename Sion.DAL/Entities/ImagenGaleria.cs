using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Entities
{
    public class ImagenGaleria
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string RutaOriginal { get; set; } = string.Empty;
        public string RutaThumbnail { get; set; } = string.Empty;
        public string RutaWebP { get; set; } = string.Empty;
        public bool EstaActiva { get; set; } = true;
        public DateTime FechaSubida { get; set; }
    }
}
