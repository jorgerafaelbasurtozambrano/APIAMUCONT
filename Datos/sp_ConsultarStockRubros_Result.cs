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
    
    public partial class sp_ConsultarStockRubros_Result
    {
        public int StockRubroIdStockRubro { get; set; }
        public int StockRubroIdTipoRubro { get; set; }
        public Nullable<decimal> StockRubroStock { get; set; }
        public int TipoRubroIdTipoRubro { get; set; }
        public string TipoRubroDescripcion { get; set; }
        public System.DateTime TipoRubroFechaCreacion { get; set; }
        public int TipoRubroIdentificador { get; set; }
        public bool TipoRubroEstado { get; set; }
    }
}