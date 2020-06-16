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
        public int? TasaInteres { get; set; }
        public string IdTipoInteresMora { get; set; }
        public int? TasaInteresMora { get; set; }
        public bool Estado { get; set; }
        public string encriptada { get; set; }
        //public TipoInteres TipoInteres { get; set; }
        //public TipoInteres TipoInteresMora { get; set; }
        public string utilizado { get; set; }
    }
}
