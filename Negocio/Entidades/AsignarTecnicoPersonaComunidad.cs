using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class AsignarTecnicoPersonaComunidad
    {
        public string IdAsignarTecnicoPersonaComunidad { get; set; }
        /// <summary>
        ///  The id del tecnico es requerido
        /// </summary>
        public string IdAsignarTUTecnico { get; set; }
        /// <summary>
        ///  The id de la persona es requerido
        /// </summary>
        public string IdPersona { get; set; }
        public string IdComunidad { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string encriptada { get; set; }
        public Comunidad Comunidad { get; set; }
    }
}
