using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class AsignarSeguro
    {
        public string IdAsignarSeguro { get; set; }
        public string IdConfigurarVenta { get; set; }
        public string IdAsignarTUResp { get; set; }
        public string IdAsignarTUTecn { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string encriptada { get; set; }
        public PersonaEntidad _Responsable { get; set; }
        public PersonaEntidad _Tecnico { get; set; }
    }
}
