using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Usuario
    {
        public string IdUsuario { get; set; }
        public Persona Persona { get; set; }
        public string UsuarioLogin { get; set; }
        public string Contrasena { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean? Estado { get; set; }
        public Tokens Tokens { get; set; }
    }
}
