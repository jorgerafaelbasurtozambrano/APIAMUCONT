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
    
    public partial class sp_CrearTicketVentaPorSaco_Result1
    {
        public int VentaRubroIdVentaRubro { get; set; }
        public string VentaRubroCodigo { get; set; }
        public System.DateTime VentaRubroFechaEntrada { get; set; }
        public Nullable<System.DateTime> VentaRubroFechaSalida { get; set; }
        public int VentaRubroIdPersonaCliente { get; set; }
        public Nullable<int> VentaRubroIdPersonaChofer { get; set; }
        public int VentaRubroIdTipoRubro { get; set; }
        public int VentaRubroIdTipoPresentacionRubro { get; set; }
        public int VentaRubroIdAsignarTU { get; set; }
        public Nullable<int> VentaRubroIdVehiculo { get; set; }
        public Nullable<decimal> VentaRubroPesoTara { get; set; }
        public Nullable<decimal> VentaRubroPesoBruto { get; set; }
        public Nullable<decimal> VentaRubroPrecioPorQuintal { get; set; }
        public Nullable<decimal> VentaRubroPorcentajeImpureza { get; set; }
        public Nullable<decimal> VentaRubroPorcentajeHumedad { get; set; }
        public Nullable<decimal> VentaRubroPesoNeto { get; set; }
        public Nullable<decimal> VentaRubroPesoACobrar { get; set; }
        public Nullable<decimal> VentaRubroTotalACobrar { get; set; }
        public bool VentaRubroEstado { get; set; }
        public bool VentaRubroAnulado { get; set; }
        public Nullable<int> VentaRubroIdAsignarTUAnulado { get; set; }
        public int TipoRubroIdTipoRubro { get; set; }
        public string TipoRubroDescripcion { get; set; }
        public System.DateTime TipoRubroFechaCreacion { get; set; }
        public bool TipoRubroEstado { get; set; }
        public int TipoRuboIdentificador { get; set; }
        public int TipoPresentacionRubrosIdTipoPresentacionRubros { get; set; }
        public string TipoPresentacionRubrosDescripcion { get; set; }
        public System.DateTime TipoPresentacionRubrosFechaCreacion { get; set; }
        public bool TipoPresentacionRubrosEstado { get; set; }
        public int TipoPresentacionRubrosIdentificador { get; set; }
    }
}
