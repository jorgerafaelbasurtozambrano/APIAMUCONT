using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class PersonaEntidad
    {
        public string IdPersona { get; set; }
        public string NumeroDocumento { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string encriptada { get; set; }

        public string IdUsuario { get; set; }
        public bool? EstadoUsuario { get; set; }
        public bool? EstadoAsignacionTipoUsuario { get; set; }
        public string IdAsignacionTipoUsuario { get; set; }
        public List<Telefono> ListaTelefono { get; set; }
        public List<Correo> ListaCorreo { get; set; }
        public List<AsignacionPersonaParroquia> AsignacionPersonaParroquia { get; set; }
        public AsignacionPersonaParroquia AsignacionPersonaComunidad { get; set; }
        public AsignacionTipoUsuario AsignacionTipoUsuario { get; set; }
        public TipoDocumento _objTipoDocumento { get; set; }
        public List<Comunidad> ListaComunidad { get; set; }
        public List<AsignarTecnicoPersonaComunidad> _AsignarTecnicoPersonaComunidad { get; set; }
    }
}
