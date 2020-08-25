using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades.Reporte
{
    public class Tecnico
    {
        public PersonaEntidad _Tecnico { get; set; }
        public List<PersonaEntidad> _Clientes { get; set; }
    }
}
