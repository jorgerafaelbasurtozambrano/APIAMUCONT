using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class AsignarProductoLote
    {
        public string IdAsignarProductoLote { get; set; }
        public string IdLote { get; set; }
        public string IdRelacionLogica { get; set; }
        public string PerteneceKit { get; set; }
        public decimal? ValorUnitario { get; set; }

        public DateTime? FechaExpiracion { get; set; }
        public string encriptada { get; set; }

        public string IdCabeceraFactura { get; set; }
        public int Cantidad { get; set; }

        public Lote Lote { get; set; }
        public ConfigurarProductos ConfigurarProductos { get; set; }
        public AsignarProductosKits AsignarProductoKits { get; set; }
    }
}
