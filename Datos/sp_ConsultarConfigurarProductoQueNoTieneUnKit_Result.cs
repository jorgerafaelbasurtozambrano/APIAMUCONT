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
    
    public partial class sp_ConsultarConfigurarProductoQueNoTieneUnKit_Result
    {
        public int TipoProductoIdTipoProducto { get; set; }
        public string TipoProductoDescripcion { get; set; }
        public System.DateTime TipoProductoFechaCreacion { get; set; }
        public System.DateTime TipoProductoFechaActualizacion { get; set; }
        public bool TipoProductoEstado { get; set; }
        public Nullable<int> ProductoIdProducto { get; set; }
        public Nullable<int> ProductoIdTipoProducto { get; set; }
        public string ProductoDescripcion { get; set; }
        public string ProductoNombre { get; set; }
        public Nullable<System.DateTime> ProductoFechaCreacion { get; set; }
        public Nullable<System.DateTime> ProductoFechaActualizacion { get; set; }
        public Nullable<bool> ProductoEstado { get; set; }
        public Nullable<int> MedidaIdMedida { get; set; }
        public string MedidaDescripcion { get; set; }
        public Nullable<System.DateTime> MedidaFechaCreacion { get; set; }
        public Nullable<System.DateTime> MedidaFechaActualizacion { get; set; }
        public Nullable<bool> MedidaEstado { get; set; }
        public Nullable<int> PresentacionIdPresentacion { get; set; }
        public string PresentacionDescripcion { get; set; }
        public Nullable<System.DateTime> PresentacionFechaCreacion { get; set; }
        public Nullable<System.DateTime> PresentacionFechaActualizacion { get; set; }
        public Nullable<bool> PresentacionEstado { get; set; }
        public Nullable<int> ConfigurarProductoIdConfigurarProducto { get; set; }
        public Nullable<int> ConfigurarProductoIdAsignacionTU { get; set; }
        public Nullable<int> ConfigurarProductoIdProducto { get; set; }
        public Nullable<int> ConfigurarProductoIdMedida { get; set; }
        public Nullable<int> ConfigurarProductoIdPresentacion { get; set; }
        public string ConfigurarProductoCodigo { get; set; }
        public Nullable<int> ConfigurarProductoCantidadMedida { get; set; }
        public Nullable<System.DateTime> ConfigurarProductoFechaCreacion { get; set; }
        public Nullable<System.DateTime> ConfigurarProductoFechaActualizacion { get; set; }
        public Nullable<bool> ConfigurarProductoEstado { get; set; }
        public Nullable<int> AsignarProductoKitIdAsignarProductoKit { get; set; }
        public Nullable<int> AsignarProductoKitIdConfigurarProducto { get; set; }
        public Nullable<int> AsignarProductoKitIdAsignarDescuentoKit { get; set; }
        public Nullable<System.DateTime> AsignarProductoKitFechaCreacion { get; set; }
        public Nullable<System.DateTime> AsignarProductoKitFechaActualizacion { get; set; }
        public Nullable<bool> AsignarProductoKitEstado { get; set; }
        public Nullable<int> AsignarDescuentoKitIdAsignarDescuentoKit { get; set; }
        public Nullable<int> AsignarDescuentoKitIdDescuento { get; set; }
        public Nullable<int> AsignarDescuentoKitIdKit { get; set; }
        public Nullable<System.DateTime> AsignarDescuentoKitFechaCreacion { get; set; }
        public Nullable<System.DateTime> AsignarDescuentoKitFechaModificacion { get; set; }
        public Nullable<int> DescuentoIdDescuento { get; set; }
        public Nullable<int> DescuentoPorcentaje { get; set; }
        public Nullable<bool> DescuentoEstado { get; set; }
    }
}
