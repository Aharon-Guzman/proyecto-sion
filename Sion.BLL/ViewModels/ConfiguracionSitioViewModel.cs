using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.ViewModels
{
    public class ConfiguracionSitioViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Clave { get; set; } = string.Empty;

        [Required(ErrorMessage ="El valor es obligatorio")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string Valor { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
