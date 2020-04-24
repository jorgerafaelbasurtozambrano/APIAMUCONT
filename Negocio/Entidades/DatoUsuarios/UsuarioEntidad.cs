using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class UsuarioEntidad
    {
        public string IdUsuario { get; set; }
        public string IdPersona { get; set; }
        public string UsuarioLogin { get; set; }
        public string Contrasena { get; set; }
        public string encriptada { get; set; }
    }
}
