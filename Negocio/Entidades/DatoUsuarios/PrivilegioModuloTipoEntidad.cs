using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class PrivilegioModuloTipoEntidad
    {
        public int IdPrivilegioModuloTipo { get; set; }
        public int IdPrivilegio { get; set; }
        public int IdModuloTipo { get; set; }
        public string encriptada { get; set; }
    }
}
