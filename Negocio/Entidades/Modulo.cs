using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Modulo
    {
        public string IdModulo { get; set; }
        public string Descripcion { get; set; }
        public string Controlador { get; set; }
        public string Metodo { get; set; }
        public int Identificador { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean Estado { get; set; }


        public int IdModuloTipo { get; set; }
        public string encriptada { get; set; }
    }
}
