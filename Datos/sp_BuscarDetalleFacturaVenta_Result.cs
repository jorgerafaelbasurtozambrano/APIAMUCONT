//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    
    public partial class sp_BuscarDetalleFacturaVenta_Result
    {
        public int IdDetalleVenta { get; set; }
        public int IdCabeceraFactura { get; set; }
        public int IdAsignarProductoLote { get; set; }
        public bool AplicaDescuento { get; set; }
        public bool Faltante { get; set; }
        public int Cantidad { get; set; }
        public bool Estado { get; set; }
        public Nullable<int> PorcentajeDescuento { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Nullable<bool> PerteneceKitCompleto { get; set; }
        public int Iva { get; set; }
    }
}
