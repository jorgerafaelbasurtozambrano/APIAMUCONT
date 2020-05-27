using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class AsignarComunidadFactura
    {
        public string IdAsignarComunidadFactura { get; set; }
        public string IdComunidad { get; set; }
        public string IdCabeceraFactura { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public Comunidad Comunidad { get; set; }
        public string encriptada { get; set; }
    }
}
