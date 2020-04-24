using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class TipoUsuario
    {
        public string IdTipoUsuario { get; set; }
        public string Descripcion { get; set; }
        public string Identificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean? Estado { get; set; }
        public string encriptada { get; set; }
        public string IdAsignacionTu { get; set; }
    }
}
