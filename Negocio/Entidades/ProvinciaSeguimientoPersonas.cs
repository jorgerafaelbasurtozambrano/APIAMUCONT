using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class CantonPersonas
    {
        public string IdCanton { get; set; }
        public string Descripcion { get; set; }
        public List<ParroquiaPersonas> ParroquiaPersonas { get; set; }
    }
    public class ParroquiaPersonas
    {
        public string IdParroquia { get; set; }
        public string Descripcion { get; set; }
        public List<ComunidadesPersonas> ComunidadesPersonas { get; set; }
    }
    public class ComunidadesPersonas
    {
        public string IdComunidad { get; set; }
        public string Descripcion { get; set; }
        public List<AsignarTecnicoPersonaComunidad> PersonaEntidad { get; set; }
    }
    public class ProvinciaSeguimientoPersonas
    {
        public string IdProvincia { get; set; }
        public string Descripcion { get; set; }
        public List<CantonPersonas> CantonPersonas { get; set; }
    }
}
