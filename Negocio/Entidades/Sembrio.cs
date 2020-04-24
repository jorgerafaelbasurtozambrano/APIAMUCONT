using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Sembrio
    {
        public string IdSembrio { get; set; }
        public string Descripcion { get; set; }
        public string IdComunidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Estado { get; set; }
        public string encriptada { get; set; }
        public Comunidad Comunidad { get; set; }
    }
}
