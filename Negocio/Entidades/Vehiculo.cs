using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class Vehiculo
    {
        public string IdVehiculo { get; set; }
        public string Placa { get; set; }
        public string IdAsignarTU { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Estado { get; set; }
        public string encriptada { get; set; }
    }
}
