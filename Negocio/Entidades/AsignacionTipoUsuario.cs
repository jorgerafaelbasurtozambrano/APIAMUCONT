using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class AsignacionTipoUsuario
    {
        public int IdAsignacionTU { get; set; }
        public int IdUsuario { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
    }
}
