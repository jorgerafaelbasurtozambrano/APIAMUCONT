using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class ModuloTipo
    {
        public int IdModuloTipo { get; set; }
        public int IdTipoUsuario { get; set; }
        public Modulo Modulo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public string encriptada { get; set; }
    }
}
