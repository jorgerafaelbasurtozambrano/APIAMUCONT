using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Entidades
{
    public class Ticket
    {
        public string IdTicket { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public Vehiculo _Vehiculo { get; set; }
        public string IdPersona { get; set; }
        public decimal? PesoBruto { get; set; }
        public string IdTipoRubro { get; set; }
        public string IdTipoPresentacionRubro { get; set; }
        public TipoRubro _TipoRubro { get; set; }
        public TipoPresentacionRubro _TipoPresentacionRubro { get; set; }
        public string IdAsignarTU { get; set; }
        public string encriptada { get; set; }
        public bool Estado { get; set; }
        public PersonaEntidad _PersonaEntidad { get; set; }
        public PersonaEntidad _PersonaQueDaAnular { get; set; }




        public decimal? PesoTara { get; set; }
        public decimal? PorcentajeHumedad { get; set; }
        public decimal? PrecioPorQuintal { get; set; }
        public decimal? PorcentajeImpureza { get; set; }
        public decimal? TotalAPagar { get; set; }
        public decimal? PesoNeto { get; set; }
        public decimal? PesoSinImpurezas { get; set; }
        public decimal? PesoAPagar { get; set; }
        public DateTime? FechaSalida { get; set; }
        public bool Anulada { get; set; }
        public string IdAsignarTUAnulada { get; set; }
    }
}
