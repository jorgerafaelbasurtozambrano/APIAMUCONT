using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class AsignacionTipoUsuarioEntidad
    {
        public string IdAsignacionTU { get; set; }
        public string IdUsuario { get; set; }
        public string IdTipoUsuario { get; set; }
        public string encriptada { get; set; }
    }
}
