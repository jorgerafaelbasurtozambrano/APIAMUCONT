using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.Reporte
{
    public class PromedioPrecios
    {
        public string FechaInicio { get; set; }
        public decimal? Precio { get; set; }
    }
    public class PromedioPreciosCompraVenta
    {
        public List<string> Fecha { get; set; }
        public List<decimal> PrecioCompra { get; set; }
        public List<decimal> PrecioVenta { get; set; }
    }
    public class ComparacionQuintales
    {
        public decimal QuintalesComprados { get; set; }
        public decimal PorcentajeQuintalesComprados { get; set; }
        public decimal QuintalesVendidos { get; set; }
        public decimal PorcentajeQuintalesVendidos { get; set; }
    }
}
