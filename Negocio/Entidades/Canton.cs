using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Canton
    {
        public string IdCanton { get; set; }
        public string Descripcion { get; set; }
        public Provincia Provincia { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean? Estado { get; set; }
        public string encriptada { get; set; }

        public bool PermitirEliminacion { get; set; }
    }
}
