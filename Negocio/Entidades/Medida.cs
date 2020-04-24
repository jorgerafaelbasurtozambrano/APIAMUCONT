using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Medida
    {
        public string IdMedida { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Estado { get; set; }
        public string encriptada { get; set; }
        public string MedidaUtilizado { get; set; }

    }
}
