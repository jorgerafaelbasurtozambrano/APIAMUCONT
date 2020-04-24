using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class ModuloTipoEntidad
    {
        public int IdModuloTipo { get; set; }
        public int IdTipoUsuario { get; set; }
        public int IdModulo { get; set; }
        public string encriptada { get; set; }
    }
}
