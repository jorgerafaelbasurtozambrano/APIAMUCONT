using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Lote
    {
        public string IdLote { get; set; }
        public string Codigo { get; set; }
        public int Capacidad { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Estado { get; set; }
        public string encriptada { get; set; }
        public string LoteUtilizado { get; set; }
        public AsignarProductoLote AsignarProductoLote { get; set; }
    }
}
