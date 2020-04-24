using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class TelefonoEntidad
    {
        public string IdTelefono { get; set; }
        public string IdPersona { get; set; }
        public string Numero { get; set; }
        public string IdTipoTelefono { get; set; }
        public string encriptada { get; set; }
    }
}
