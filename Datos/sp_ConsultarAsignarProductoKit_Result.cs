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
    
    public partial class sp_ConsultarAsignarProductoKit_Result
    {
        public int AsignarProductoKitIdAsignarProductoKit { get; set; }
        public int AsignarProductoKitIdConfigurarProducto { get; set; }
        public int AsignarProductoKitIdAsignarDescuentoKit { get; set; }
        public System.DateTime AsignarProductoKitFechaCreacion { get; set; }
        public System.DateTime AsignarProductoKitFechaActualizacion { get; set; }
        public bool AsignarProductoKitEstado { get; set; }
        public Nullable<int> ConfigurarProductoIdConfigurarProducto { get; set; }
        public Nullable<int> ConfigurarProductoIdAsignacionTU { get; set; }
        public Nullable<int> ConfigurarProductoIdProducto { get; set; }
        public Nullable<int> ConfigurarProductoIdPresentacion { get; set; }
        public Nullable<int> ConfigurarProductoIdMedida { get; set; }
        public Nullable<int> ConfigurarProductoCantidadMedida { get; set; }
        public string ConfigurarProductoCodigo { get; set; }
        public Nullable<System.DateTime> ConfigurarProductoFechaCreacion { get; set; }
        public Nullable<System.DateTime> ConfigurarProductoFechaActualizacion { get; set; }
        public Nullable<bool> ConfigurarProductoEstado { get; set; }
        public Nullable<int> ConfigurarProductoIva { get; set; }
        public Nullable<int> MedidaIdMedida { get; set; }
        public string MedidaDescripcion { get; set; }
        public Nullable<System.DateTime> MedidaFechaCreacion { get; set; }
        public Nullable<System.DateTime> MedidaFechaActualizacion { get; set; }
        public Nullable<bool> MedidaEstado { get; set; }
        public Nullable<int> PresentacionIdPresentacion { get; set; }
        public string PresentacionDescripcion { get; set; }
        public Nullable<System.DateTime> PresentacionFechaCreacion { get; set; }
        public Nullable<System.DateTime> PresentacionActualizacion { get; set; }
        public Nullable<bool> PresentacionEstado { get; set; }
        public Nullable<int> ProductoIdProducto { get; set; }
        public Nullable<int> ProductoIdTipoProducto { get; set; }
        public string ProductoDescripcion { get; set; }
        public string ProductoNombre { get; set; }
        public Nullable<System.DateTime> ProductoFechaCreacion { get; set; }
        public Nullable<System.DateTime> ProductoFechaActualizacion { get; set; }
        public Nullable<bool> ProductoEstado { get; set; }
        public Nullable<int> TipoProductoIdTipoProducto { get; set; }
        public string TipoProductoDescripcion { get; set; }
        public Nullable<System.DateTime> TipoProductoFechaCreacion { get; set; }
        public Nullable<System.DateTime> TipoProductoFechaActualizacion { get; set; }
        public Nullable<bool> TipoProductoEstado { get; set; }
        public int KitIdKit { get; set; }
        public string KitCodigo { get; set; }
        public string KitDescripcion { get; set; }
        public System.DateTime KitFechaCreacion { get; set; }
        public System.DateTime KitFechaActualizacion { get; set; }
        public bool KitEstado { get; set; }
        public int AsignarDescuentoKitIdAsignarDescuentoKit { get; set; }
        public int AsignarDescuentoKitIdDescuento { get; set; }
        public int AsignarDescuentoKitIdKit { get; set; }
        public System.DateTime AsignarDescuentoKitFechaCreacion { get; set; }
        public System.DateTime AsignarDescuentoKitFechaModificacion { get; set; }
        public int DescuentoIdDescuento { get; set; }
        public int DescuentoPorcentaje { get; set; }
        public bool DescuentoEstado { get; set; }
    }
}
