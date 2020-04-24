using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class CabeceraFactura
    {
        public string IdCabeceraFactura { get; set; }
        public string Codigo { get; set; }
        public string IdAsignacionTU { get; set; }
        public string IdTipoTransaccion { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public bool EstadoCabeceraFactura { get; set; }
        public bool Finalizado { get; set; }
        public string encriptada { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }
        public List<DetalleFactura> DetalleFactura { get; set; }
        public PersonaEntidad PersonaEntidad { get; set; }
        public TipoUsuario TipoUsuario { get; set; }




        //venta factura
        public List<DetalleVenta> DetalleVenta { get; set; }
    }
}
