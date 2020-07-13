using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Login
    {
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string encriptada { get; set; }
    }
}
