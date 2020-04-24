using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class Stock
    {
        public string IdStock { get; set; }
        public int Cantidad { get; set; }
        public string IdAsignarProductoLote { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public AsignarProductoLote AsignarProductoLote { get; set; }
    }
}
