using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Descuento
    {
        public string IdDescuento { get; set; }
        public int? Porcentaje { get; set; }
        public bool? Estado { get; set; }
        public string DescuentoUtilizado { get; set; }
        public string encriptada { get; set; }
    }
}
