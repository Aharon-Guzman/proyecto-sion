using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Entities
{
    public class Donacion
    {
        public int Id { get; set; }
        public string TransaccionPaypalId { get; set; } = string.Empty;
        public decimal Monto { get; set; } = 0;
        public string Moneda { get; set; } = string.Empty;
        public string? NombreDonante { get; set; }
        public string? EmailDonante { get; set; }
        public bool EsRecurrente { get; set; } = false;
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
