using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.ViewModels
{
    public class DonacionViewModel
    {
        public int Id { get; set; }

        public string TransaccionPaypalId { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Monto { get; set; }

        public string Moneda { get; set; } = string.Empty;

        public string? NombreDonante { get; set; }
        public string? EmailDonante { get; set; }

        public bool EsRecurrente { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
