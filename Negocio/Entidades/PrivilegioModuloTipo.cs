using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class PrivilegioModuloTipo
    {
        public int IdPrivilegioModuloTipo { get; set; }
        public Privilegios Privilegio { get; set; }
        public int IdModuloTipo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public Tokens Tokens { get; set; }
    }
}
