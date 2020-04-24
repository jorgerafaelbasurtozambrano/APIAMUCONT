using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class PrecioConfigurarProducto
    {
        public string IdPrecioConfigurarProducto { get; set; }
        public string IdConfigurarProducto { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
        public string encriptada { get; set; }
    }
}
