using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class AsignarTecnicoPersonaComunidad
    {
        public string IdAsignarTecnicoPersonaComunidad { get; set; }
        public string IdAsignarTUTecnico { get; set; }
        public string IdPersona { get; set; }
        public string IdComunidad { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string encriptada { get; set; }
        public Comunidad Comunidad { get; set; }
        public int? NumeroVisita { get; set; }
        public PersonaEntidad Persona { get; set; }
        public string IdCanton { get; set; }
        public string IdParroquia { get; set; }
    }

    public class TrasnferirTecnico
    {
        public string IdAsignarTUAntiguo { get; set; }
        public string IdAsignarTUNuevo { get; set; }
        public string encriptada { get; set; }
    }
}
