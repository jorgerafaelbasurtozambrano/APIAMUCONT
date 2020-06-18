using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class SaldoPendiente
    {
        public string IdSaldoPendiente { get; set; }
        public string IdConfigurarVenta { get; set; }
        public decimal? TotalFactura { get; set; }
        public decimal? TotalInteres { get; set; }
        public decimal? Pendiente { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
