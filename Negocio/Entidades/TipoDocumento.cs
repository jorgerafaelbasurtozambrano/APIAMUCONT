using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class TipoDocumento
    {
        public string IdTipoDocumento { get; set; }
        public string Documento { get; set; }
        public int Identificador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
    }
}
