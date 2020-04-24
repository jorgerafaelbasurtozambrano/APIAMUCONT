using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
namespace Negocio.Entidades.DatoUsuarios
{
    public class UsuariosSistema
    {
        public string IdPersona { get; set; }
        public string NumeroDocumento { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }

        public string IdUsuario { get; set; }
        public string UsuarioLogin { get; set; }
        public string Contrasena { get; set; }
        public bool? EstadoUsuario { get; set; }

        public List<TipoUsuario> ListaTipoUsuario { get; set; }
        public List<Privilegios> Privilegios { get; set; }
        public List<Modulo> Modulo { get; set; }
    }
}
