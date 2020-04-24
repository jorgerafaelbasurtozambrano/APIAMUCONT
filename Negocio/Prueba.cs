using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Prueba
    {
        Metodos.Seguridad seguridad = new Metodos.Seguridad();
        public string encriptar(string texto, string clave)
        {
            return seguridad.EncryptStringAES(texto, clave);
        }

        public string desencriptar(string token, string clave)
        {
            return seguridad.DecryptStringAES(token, clave);
        }


    }
}
