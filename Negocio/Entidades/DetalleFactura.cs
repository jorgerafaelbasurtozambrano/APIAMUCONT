using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class DetalleFactura
    {
        public string IdDetalleFactura { get; set; }
        public string IdCabeceraFactura { get; set; }
        public string IdAsignarProductoLote { get; set; }
        public int? Cantidad { get; set; }
        public bool Faltante { get; set; }
        public string encriptada { get; set; }
        public List<AsignarProductoLote> AsignarProductoLote { get; set; }
    }

    public class DetalleFacturaVenta
    {
        public string IdDetalleFactura { get; set; }
        public string IdCabeceraFactura { get; set; }
        public string IdAsignarProductoLote { get; set; }
        public int? Cantidad { get; set; }
        public bool Faltante { get; set; }
        public string encriptada { get; set; }
        public AsignarProductoLote AsignarProductoLote { get; set; }
        public CabeceraFactura CabeceraFactura { get; set; }
    }
}
