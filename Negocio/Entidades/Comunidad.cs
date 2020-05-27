using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Negocio.Entidades
{
    public class Comunidad
    {
        public string IdComunidad { get; set; }
        public string Descripcion { get; set; }
        public Parroquia Parroquia { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public Tokens Tokens { get; set; }
        public string ComunidadUtilizado { get; set; }
        public List<AsignarComunidadFactura> ListaAsignarComunidadFactura { get; set; }
        public string encriptada { get; set; }
    }
}
