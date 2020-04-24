using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class TipoProducto
    {
        public string IdTipoProducto { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? estado { get; set; }
        public string TipoUsuarioUtilizado { get; set; }
        public string encriptada { get; set; }
    }
}
