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
    
    public partial class sp_ConsultarComunidadesDeUnaPersonaAsignadoAUnTecnico_Result
    {
        public int AsignarTecnicoPersonaComunidadIdComunidad { get; set; }
        public bool AsignarTecnicoPersonaComunidadEstado { get; set; }
        public System.DateTime AsignarTecnicoPersonaComunidadFechaAsignacion { get; set; }
        public int AsignarTecnicoPersonaComunidadIdAsignarTecnicoPersonaComunidad { get; set; }
        public int AsignarTecnicoPersonaComunidadIdAsignarTUTecnico { get; set; }
        public int AsignarTecnicoPersonaComunidadIdPersona { get; set; }
        public int ComunidadIdComunidad { get; set; }
        public bool ComunidadEstado { get; set; }
        public string ComunidadDescripcion { get; set; }
        public Nullable<System.DateTime> ComunidadFechaActualizacion { get; set; }
        public System.DateTime ComunidadFechaCreacion { get; set; }
        public int ComunidadIdParroquia { get; set; }
        public int ParroquiaIdParroquia { get; set; }
        public string ParroquiaDescripcion { get; set; }
        public bool ParroquiaEstado { get; set; }
        public System.DateTime ParroquiaFechaCreacion { get; set; }
        public Nullable<System.DateTime> ParroquiaFechaActualizacion { get; set; }
        public int ParroquiaIdCanton { get; set; }
        public int CantonIdCanton { get; set; }
        public string CantonDescripcion { get; set; }
        public Nullable<System.DateTime> CantonFechaActualizacion { get; set; }
        public System.DateTime CantonFechaCreacion { get; set; }
        public int CantonIdProvincia { get; set; }
        public bool CantonEstado { get; set; }
        public string ProvinciaDescripcion { get; set; }
        public int ProvinciaIdProvincia { get; set; }
        public bool ProvinciaEstado { get; set; }
        public System.DateTime ProvinciaFechaCreacion { get; set; }
        public Nullable<System.DateTime> ProvinciaFechaActualizacion { get; set; }
        public Nullable<int> NumeroVisitas { get; set; }
    }
}
