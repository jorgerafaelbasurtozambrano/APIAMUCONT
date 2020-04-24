using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades.DatoUsuarios
{
    public class ConfigurarProducto
    {
        public string IdConfigurarProducto { get; set; }
        public string IdAsignacionTu { get; set; }
        public string IdProducto { get; set; }
        public string IdMedida { get; set; }
        public string IdPresentacion { get; set; }
        public string Codigo { get; set; }

        public int CantidadMedida { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool estado { get; set; }
        public string encriptada { get; set; }
    }
}
