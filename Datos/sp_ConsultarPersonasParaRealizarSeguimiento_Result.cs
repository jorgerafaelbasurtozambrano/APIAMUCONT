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
    
    public partial class sp_ConsultarPersonasParaRealizarSeguimiento_Result
    {
        public int PersonaIdPersona { get; set; }
        public string PersonaApellidoMaterno { get; set; }
        public string PersonaApellidoPaterno { get; set; }
        public bool PersonaEstado { get; set; }
        public Nullable<System.DateTime> PersonaFechaActualizacion { get; set; }
        public System.DateTime PersonaFechaCreacion { get; set; }
        public int PersonaIdTipoDocumento { get; set; }
        public string PersonaNumeroDocumento { get; set; }
        public string PersonaPrimerNombre { get; set; }
        public string PersonaSegundoNombre { get; set; }
        public int TipoDocumentoIdTipoDocumento { get; set; }
        public string TipoDocumentoDescripcion { get; set; }
        public bool TipoDocumentoEstado { get; set; }
        public Nullable<System.DateTime> TipoDocumentoFechaActualizacion { get; set; }
        public System.DateTime TipoDocumentoFechaCreacion { get; set; }
        public int TipoDocumentoIdentificador { get; set; }
    }
}
