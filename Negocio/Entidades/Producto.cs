using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Negocio.Entidades
{
    public class Producto
    {
        public string IdProducto { get; set; }
        public string IdTipoProducto { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool? Estado { get; set; }
        public string encriptada { get; set; }
        public string ProductoUtilizado { get; set; }
        public TipoProducto TipoProducto { get; set; }
    }
}
