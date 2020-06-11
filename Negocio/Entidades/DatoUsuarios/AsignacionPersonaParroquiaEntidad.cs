using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class AsignacionPersonaParroquiaEntidad
    {
        public string IdAsignacionPC { get; set; }
        public string IdPersona { get; set; }
        public string IdParroquia { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public string  Referencia { get; set; }

        public string encriptada { get; set; }
    }
}
