using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Tokens
    {
        public int IdToken { get; set; }
        public string Descripcion { get; set; }
        public int Identificador { get; set; }
        public Clave Clave { get; set; }
        public Boolean Estado { get; set; }

        public string encriptada { get; set; }
    }
}
