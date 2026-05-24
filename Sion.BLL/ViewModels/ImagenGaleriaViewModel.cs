using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.ViewModels
{
    public class ImagenGaleriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        public string RutaOriginal { get; set; } = string.Empty;
        public string RutaThumbnail { get; set; } = string.Empty;
        public string RutaWebP { get; set; } = string.Empty;

        public bool EstaActiva { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}
