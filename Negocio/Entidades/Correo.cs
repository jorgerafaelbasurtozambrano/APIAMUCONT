using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Correo
    {
        public string IdCorreo { get; set; }
        public string IdPersona { get; set; }
        public string CorreoValor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public string encriptada { get; set; }
    }
}
