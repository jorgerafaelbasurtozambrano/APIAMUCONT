using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Telefono
    {
        public string IdTelefono { get; set; }
        public string IdPersona { get; set; }
        public string Numero { get; set; }
        public TipoTelefono TipoTelefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public Tokens Tokens { get; set; }
    }
}
