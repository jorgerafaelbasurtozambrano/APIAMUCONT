using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string NumeroDocumento { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }


        public Telefono Telefono { get; set; }
        public Correo Correo { get; set; }
        public TipoDocumento TipoDocumentos { get; set; }
        public AsignacionPersonaComunidad AsigancionPersonaComunidad { get; set; }


        public AsignacionTipoUsuario AsignacionTipoUsuario { get; set; }
        public ModuloTipo ModuloTipo { get; set; }
        public PrivilegioModuloTipo PrivilegioModuloTipo { get; set; }


        public Tokens Tokens { get; set; }

    }
}
