using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Abono
    {
        public string IdAbono { get; set; }
        public string IdConfigurarVenta { get; set; }
        public string IdAsignarTU { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string encriptada { get; set; }
    }
}
