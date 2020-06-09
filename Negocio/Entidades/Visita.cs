using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class Visita
    {
        public string IdVisita { get; set; }
        public string IdAsignarTecnicoPersonaComunidad { get; set; }
        public string IdAsignarTU { get; set; }
        public string Observacion { get; set; }
        public string encriptada { get; set; }
        public DateTime FechaRegistro { get; set; }
        public PersonaEntidad Persona { get; set; }
    }
}
