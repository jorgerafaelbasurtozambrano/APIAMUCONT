using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class ConfiguracionInteres
    {
        public string IdConfiguracionInteres { get; set; }
        public string IdTipoInteres { get; set; }
        public int TasaInteres { get; set; }
        public string Estado { get; set; }
        public int PlazoMeses { get; set; }
    }
}
