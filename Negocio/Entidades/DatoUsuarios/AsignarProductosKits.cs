using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class AsignarProductosKits
    {
        public string IdKit { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Estado { get; set; }
        public string encriptada { get; set; }
        public List<AsignarProductoKit> ListaAsignarProductoKit { get; set; }
    }
}
