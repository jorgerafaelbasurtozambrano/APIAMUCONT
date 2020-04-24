using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class ConfigurarVenta
    {
        public string IdConfigurarVenta { get; set; }
        public string IdCabeceraFactura { get; set; }
        public string IdPersona { get; set; }
        public string IdSembrio { get; set; }
        public string EstadoConfVenta { get; set; }
        public string IdConfiguracionInteres { get; set; }
        public string Efectivo { get; set; }
        public int Descuento { get; set; }
        public string encriptada { get; set; }
    }
}
