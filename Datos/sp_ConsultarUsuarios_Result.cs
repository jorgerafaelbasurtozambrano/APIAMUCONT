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
    
    public partial class sp_ConsultarUsuarios_Result
    {
        public int UsuarioIdUsuario { get; set; }
        public int UsuarioIdPersona { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public Nullable<System.DateTime> UsuarioFechaCreacion { get; set; }
        public Nullable<bool> UsuarioEstado { get; set; }
        public int PersonaIdPersona { get; set; }
        public string PersonaNumeroDocumento { get; set; }
        public string PersonaApellidoPaterno { get; set; }
        public string PersonaApellidoMaterno { get; set; }
        public string PersonaPrimerNombre { get; set; }
        public string PersonaSegundoNombre { get; set; }
        public System.DateTime PersonaFechaCreacion { get; set; }
        public bool PersonaEstado { get; set; }
        public int TipoDocumentoIdTipoDocumento { get; set; }
        public string TipoDocumentoDescripcion { get; set; }
        public int TipoDocumentoIdentificador { get; set; }
        public System.DateTime TipoDocumentoFechaCreacion { get; set; }
        public bool TipoDocumentoEstado { get; set; }
        public int IdAsignacionTU { get; set; }
        public int AsignacionTipoUsuarioIdUsuario { get; set; }
        public int AsigancionTipoUsuarioIdTipoUsuario { get; set; }
        public Nullable<System.DateTime> AsignacionTipoUsuarioFechaCreacion { get; set; }
        public bool AsignacionTipoUsuarioEstado { get; set; }
        public int TipoUsuarioIdTipoUsuario { get; set; }
        public string TipoUsuarioDescripcion { get; set; }
        public string TipoUsuarioIdentificacion { get; set; }
        public Nullable<System.DateTime> TipoUsuarioFechaCreacion { get; set; }
        public bool TipoUsuarioEstado { get; set; }
        public int IdModuloTipo { get; set; }
        public int ModuloTipoIdTipoUsuario { get; set; }
        public int ModuloTipoIdModulo { get; set; }
        public Nullable<System.DateTime> ModuloTipoFechaCreacion { get; set; }
        public bool ModuloTipoEstado { get; set; }
        public int IdModulo { get; set; }
        public string ModuloDescripcion { get; set; }
        public string Controlador { get; set; }
        public string Metodo { get; set; }
        public int ModuloIdentificador { get; set; }
        public Nullable<System.DateTime> ModuloFechaCreacion { get; set; }
        public bool ModuloEstado { get; set; }
        public int IdPrivilegioModuloTipo { get; set; }
        public int PrivilegioModuloTipoIdPrivilegio { get; set; }
        public int PrivilegioModuloTipoIdModuloTipo { get; set; }
        public Nullable<System.DateTime> PrivilegioModuloTipoFechaCreacion { get; set; }
        public bool PrivilegioModuloTipoEstado { get; set; }
        public int IdPrivilegios { get; set; }
        public string PrivilegiosDescripcion { get; set; }
        public int PrivilegiosIdentificador { get; set; }
        public Nullable<System.DateTime> PrivilegioFechaCreacion { get; set; }
        public bool PrivilegioEstado { get; set; }
    }
}