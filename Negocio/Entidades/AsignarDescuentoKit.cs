using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class AsignarDescuentoKit
    {
        public string IdAsignarDescuentoKit { get; set; }
        public string IdDescuento { get; set; }
        public string IdKit { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string encriptada { get; set; }
        public bool Estado { get; set; }
        public Descuento Descuento { get; set; }
    }
}
