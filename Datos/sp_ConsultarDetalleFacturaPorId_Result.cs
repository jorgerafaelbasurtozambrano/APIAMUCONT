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
    
    public partial class sp_ConsultarDetalleFacturaPorId_Result
    {
        public int DetalleFacturaCantidad { get; set; }
        public bool DetalleFacturaFaltante { get; set; }
        public int DetalleFacturaIdAsignarProductoLote { get; set; }
        public int DetalleFacturaIdCabeceraFactura { get; set; }
        public int DetalleFacturaIdDetalleFactura { get; set; }
        public string CabeceraFacturaCodigo { get; set; }
        public bool CabeceraFacturaEstado_Cabecera_Factura { get; set; }
        public System.DateTime CabeceraFacturaFechaGeneracion { get; set; }
        public bool CabeceraFacturaFinalizado { get; set; }
        public int CabeceraFacturaIdAsignacionTU { get; set; }
        public int CabeceraFacturaIdCabeceraFactura { get; set; }
        public int CabeceraFacturaIdTipoTransaccion { get; set; }
        public Nullable<System.DateTime> AsignarProductoLoteFechaExpiracion { get; set; }
        public int AsignarProductoLoteIdAsignarProductoLote { get; set; }
        public Nullable<int> AsignarProductoLoteIdLote { get; set; }
        public int AsignarProductoLoteIdRelacionLogica { get; set; }
        public bool AsignarProductoLotePerteneceKit { get; set; }
        public decimal AsignarProductoLoteValorUnitario { get; set; }
    }
}