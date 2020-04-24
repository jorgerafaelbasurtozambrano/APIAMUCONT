using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Privilegios
    {
        public string IdPrivilegios { get; set; }
        public string Descripcion { get; set; }
        public int Identificador { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
    }
}
