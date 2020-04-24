using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class DetalleVenta
    {
        public string IdDetalleVenta { get; set; }
        public string IdCabeceraFactura { get; set; }
        public string IdAsignarProductoLote { get; set; }
        public string AplicaDescuento { get; set; }
        public string Faltante { get; set; }
        public int Cantidad { get; set; }
        public string encriptada { get; set; }
        public AsignarProductoLote AsignarProductoLote { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? Total { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
