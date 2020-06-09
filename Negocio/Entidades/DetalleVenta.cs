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
        public int? Cantidad { get; set; }
        public string encriptada { get; set; }
        public AsignarProductoLote AsignarProductoLote { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? Total { get; set; }
        public decimal? Subtotal { get; set; }
        public string IdKit { get; set; }
        public int? PorcentajeDescuento { get; set; }
        public bool? PerteneceKitCompleto { get; set; }
        public int? CantidadDisponible { get; set; }
        public bool? PermitirVender { get; set; }
        public int? Iva { get; set; }
        public decimal? CantidadDescontada { get; set; }
        public decimal? IvaAnadido { get; set; }
    }
}
