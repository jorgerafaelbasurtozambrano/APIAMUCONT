using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class StockRubro
    {
        public string IdStockRubro { get; set; }
        public string IdTipoRubro { get; set; }
        public decimal? Stock { get; set; }
        public TipoRubro _TipoRubro { get; set; }
    }
}
