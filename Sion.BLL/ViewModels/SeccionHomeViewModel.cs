using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.ViewModels
{
    public class SeccionHomeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;
        public string? RutaImagen { get; set; }
        
        [Required(ErrorMessage = "El estilo es obligatorio")]
        public string Estilo { get; set; } = string.Empty;

        public int Orden { get; set; }
        public bool EstaActiva { get; set; }
    }
}
