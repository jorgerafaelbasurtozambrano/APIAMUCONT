using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
namespace Negocio.Entidades
{
    public class ConfigurarProductos
    {
        public string IdConfigurarProducto { get; set; }
        public string IdAsignacionTu { get; set; }
        public Producto Producto { get; set; }
        public Medida Medida { get; set; }
        public Presentacion Presentacion { get; set; }
        public string Codigo { get; set; }
        public int? CantidadMedida { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool? estado { get; set; }
        public string encriptada { get; set; }
        public string ConfigurarProductosUtilizado { get; set; }

        public PrecioConfigurarProducto PrecioConfigurarProducto { get; set; }

    }
}
