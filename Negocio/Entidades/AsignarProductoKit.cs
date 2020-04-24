using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class AsignarProductoKit
    {
        public string IdAsignarProductoKit { get; set; }
        public string IdConfigurarProducto { get; set; }
        public string IdAsignarDescuentoKit { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Estado { get; set; }
        public string encriptada { get; set; }
        public ConfigurarProductos ListaProductos { get; set; }
        public Kit Kit { get; set; }

    }
}
