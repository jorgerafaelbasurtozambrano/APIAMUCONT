using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
namespace Negocio.Entidades.DatoUsuarios
{
    public class UsuarioCliente
    {
        public int IdPersona { get; set; }
        public string NumeroDocumento { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        public int IdTipoUsuario { get; set; }
        public string Descripcion { get; set; }
        public string Identificacion { get; set; }

        public AsignacionPersonaComunidad AsignacionPersonaComunidad { get; set; }
        public List<Telefono> Telefonos { get; set; }
        public List<Correo> Correos { get; set; }

    }
}
