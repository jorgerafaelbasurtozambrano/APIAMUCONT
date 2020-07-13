using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class TipoRubro
    {
        public string IdTipoRubro { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Identificador { get; set; }
        public bool Estado { get; set; }
        public string encriptada { get; set; }
    }
}
