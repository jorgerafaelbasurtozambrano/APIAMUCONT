using Negocio.Entidades.DatoUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Entidades
{
    public class TicketVenta
    {
        public string IdTicketVenta { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string IdPersonaCliente { get; set; }
        public string IdPersonaChofer { get; set; }
        public string IdTipoRubro { get; set; }
        public string IdTipoPresentacionRubro { get; set; }
        public string IdAsignarTU { get; set; }
        public string IdVehiculo { get; set; }
        public decimal? PesoTara { get; set; }
        public decimal? PesoBruto { get; set; }
        public decimal? PrecioPorQuintal { get; set; }
        public decimal? PorcentajeImpureza { get; set; }
        public decimal? PorcentajeHumedad { get; set; }
        public decimal? PesoNeto { get; set; }
        public decimal? PesoACobrar { get; set; }
        public decimal? TotalACobrar { get; set; }
        public bool Estado { get; set; }
        public Vehiculo _Vehiculo { get; set; }
        public TipoRubro _TipoRubro { get; set; }
        public TipoPresentacionRubro _TipoPresentacionRubro { get; set; }
        public PersonaEntidad _PersonaCliente { get; set; }
        public PersonaEntidad _PersonaChofer { get; set; }
        public PersonaEntidad _PersonaUsuario { get; set; }
        public PersonaEntidad _PersonaQueDaAnular { get; set; }
        public bool Anulada { get; set; }
        public string IdAsignarTUAnulada { get; set; }
        public string encriptada { get; set; }
        public bool? Modificado { get; set; }
        //public string _IdPersonaModifica { get; set; }
        //public PersonaEntidad _PersonaQueModifica { get; set; }
    }
}
