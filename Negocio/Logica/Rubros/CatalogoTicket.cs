using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;

namespace Negocio.Logica.Rubros
{
    public class CatalogoTicket
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoVehiculo GestionVehiculo = new CatalogoVehiculo();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        List<Ticket> ListaTicket = new List<Ticket>();
        public Ticket InsertarTiketPorCarro(Ticket _Ticket)
        {
            Vehiculo _Vehiculo = new Vehiculo();
            _Vehiculo = GestionVehiculo.ConsultarVehiculoPorPlaca(_Ticket._Vehiculo.Placa).FirstOrDefault();
            if (_Vehiculo == null)
            {
                _Vehiculo = new Vehiculo();
                _Vehiculo = GestionVehiculo.IngresarVehiculo(new Vehiculo() {Placa = _Ticket._Vehiculo.Placa,IdAsignarTU = _Ticket.IdAsignarTU });
                if (_Vehiculo.IdVehiculo == null)
                {
                    _Ticket.IdTicket = null;
                    return _Ticket;
                }
            }
            _Vehiculo.IdVehiculo = Seguridad.DesEncriptar(_Vehiculo.IdVehiculo);
            foreach (var item in ConexionBD.sp_CrearTicketPorCarro(int.Parse(_Ticket.IdAsignarTU),_Ticket.PesoBruto,int.Parse(_Vehiculo.IdVehiculo), int.Parse(_Ticket.IdPersona),int.Parse(_Ticket._TipoRubro.IdTipoRubro),int.Parse(_Ticket._TipoPresentacionRubro.IdTipoPresentacionRubro)))
            {
                _Ticket.IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString());
                _Ticket.Codigo = item.TicketCompraCodigo;
                _Ticket.FechaIngreso = item.TicketCompraFechaIngreso;
                _Ticket.PesoBruto = item.TicketCompraPesoBruto;
                _Ticket.IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString());
                _Ticket.IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString());
                _Ticket._PersonaEntidad = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                _Ticket.IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString());
                _Ticket.IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString());
                _Ticket.Estado = item.TicketCompraEstado;
                _Ticket._Vehiculo = new Vehiculo()
                {
                    IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                    Estado = item.VehiculoEstado,
                    Placa = item.VehiculoPlaca,
                    FechaCreacion = item.VehiculoFechaCreacion,
                    IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                };
                _Ticket._TipoRubro = new TipoRubro()
                {
                    IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                    Descripcion = item.TipoRubroDescripcion,
                    FechaCreacion = item.TipoRubroFechaCreacion,
                    Identificador = item.TipoRuboIdentificador,
                    Estado = item.TipoRubroEstado
                };
                _Ticket._TipoPresentacionRubro = new TipoPresentacionRubro()
                {
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                    Descripcion = item.TipoPresentacionRubrosDescripcion,
                    FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                    Identificador = item.TipoPresentacionRubrosIdentificador,
                    Estado = item.TipoPresentacionRubrosEstado
                };
            }
            return _Ticket;
        }
        public Ticket InsertarTicketPorSaco(Ticket _Ticket)
        {
            foreach (var item in ConexionBD.sp_CrearTicketPorSaco(int.Parse(_Ticket.IdAsignarTU), _Ticket.PesoNeto, int.Parse(_Ticket.IdPersona), int.Parse(_Ticket._TipoRubro.IdTipoRubro), int.Parse(_Ticket._TipoPresentacionRubro.IdTipoPresentacionRubro),_Ticket.PorcentajeHumedad,_Ticket.PorcentajeImpureza,_Ticket.PrecioPorQuintal))
            {
                _Ticket.IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString());
                _Ticket.Codigo = item.TicketCompraCodigo;
                _Ticket.FechaIngreso = item.TicketCompraFechaIngreso;
                _Ticket.IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString());
                _Ticket.IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString());
                _Ticket.IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString());
                _Ticket.IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString());
                _Ticket.Estado = item.TicketCompraEstado;
                _Ticket.PesoAPagar = item.TicketCompraPesoAPagar;
                _Ticket.PesoSinImpurezas = item.TicketCompraPesoSinImpurezas;
                _Ticket.PesoNeto = item.TicketCompraPesoNeto;
                _Ticket.PesoBruto = item.TicketCompraPesoBruto;
                _Ticket.PorcentajeHumedad = item.TicketCompraPorcentajeHumedad;
                _Ticket.PorcentajeImpureza = item.TicketCompraPorcentajeImpurezas;
                _Ticket.PrecioPorQuintal = item.TicketCompraPrecioPorQuintal;
                _Ticket.TotalAPagar = item.TicketCompraTotalAPagar;
                _Ticket._PersonaEntidad = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                _Ticket._TipoRubro = new TipoRubro()
                {
                    IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                    Descripcion = item.TipoRubroDescripcion,
                    FechaCreacion = item.TipoRubroFechaCreacion,
                    Identificador = item.TipoRuboIdentificador,
                    Estado = item.TipoRubroEstado
                };
                _Ticket._TipoPresentacionRubro = new TipoPresentacionRubro()
                {
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                    Descripcion = item.TipoPresentacionRubrosDescripcion,
                    FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                    Identificador = item.TipoPresentacionRubrosIdentificador,
                    Estado = item.TipoPresentacionRubrosEstado
                };
            }
            return _Ticket;
        }
        public List<Ticket> ConsultarTikets()
        {
            ListaTicket = new List<Ticket>();
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarTicketPorCarro())
            {
                PersonaEntidad DatoPersona = new PersonaEntidad();
                DatoPersona = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.TicketCompraIdPersona.ToString()).FirstOrDefault();
                if (DatoPersona == null)
                {
                    DatoPersona = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                    Personas.Add(DatoPersona);
                }
                ListaTicket.Add(new Ticket() {
                    IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString()),
                    Codigo = item.TicketCompraCodigo,
                    FechaIngreso = item.TicketCompraFechaIngreso,
                    PesoBruto = item.TicketCompraPesoBruto,
                    IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString()),
                    IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString()),
                    Estado = item.TicketCompraEstado,
                    _PersonaEntidad = DatoPersona,
                    _Vehiculo = new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    },
                    _TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    },
                    _TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    },
                });
            }
            return ListaTicket;
        }
        public List<Ticket> ConsultarTiketsFinalizados()
        {
            ListaTicket = new List<Ticket>();
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarTicketPorSaco())
            {
                PersonaEntidad DatoPersona = new PersonaEntidad();
                DatoPersona = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.TicketCompraIdPersona.ToString()).FirstOrDefault();
                if (DatoPersona == null)
                {
                    DatoPersona = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                    Personas.Add(DatoPersona);
                }
                string IdAsignarTUAnulada = "";
                if (item.TicketCompraAnulada == true)
                {
                    IdAsignarTUAnulada = Seguridad.Encriptar(item.TicketCompraIdAsignacionTUAnulada.ToString());
                }

                List<Vehiculo> DatoVehiculo = new List<Vehiculo>();
                if (item.VehiculoIdVehiculo != null)
                {
                    DatoVehiculo.Add(new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    });
                }

                ListaTicket.Add(new Ticket()
                {
                    IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString()),
                    Codigo = item.TicketCompraCodigo,
                    FechaIngreso = item.TicketCompraFechaIngreso,
                    FechaSalida = item.TicketCompraFechaSalida,
                    PesoBruto = item.TicketCompraPesoBruto,
                    PesoTara = item.TicketCompraPesoTara,
                    PesoNeto = item.TicketCompraPesoNeto,
                    PesoSinImpurezas = item.TicketCompraPesoSinImpurezas,
                    PesoAPagar = item.TicketCompraPesoAPagar,
                    PrecioPorQuintal = item.TicketCompraPrecioPorQuintal,
                    IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString()),
                    //idvehiculo

                    IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString()),
                    PorcentajeImpureza = item.TicketCompraPorcentajeImpurezas,
                    PorcentajeHumedad = item.TicketCompraPorcentajeHumedad,
                    TotalAPagar = item.TicketCompraTotalAPagar,
                    Estado = item.TicketCompraEstado,
                    Anulada = item.TicketCompraAnulada,
                    _Vehiculo = DatoVehiculo.FirstOrDefault(),
                    IdAsignarTUAnulada = IdAsignarTUAnulada,
                    _PersonaEntidad = DatoPersona,
                    _TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    },
                    _TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    },
                });
            }
            return ListaTicket;
        }
        public List<Ticket> ConsultarTiketsPorPlaca(string Placa)
        {
            ListaTicket = new List<Ticket>();
            foreach (var item in ConexionBD.sp_ConsultarTicketPorCarroPorPlacaCarro(Placa))
            {
                ListaTicket.Add(new Ticket()
                {
                    IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString()),
                    Codigo = item.TicketCompraCodigo,
                    FechaIngreso = item.TicketCompraFechaIngreso,
                    PesoBruto = item.TicketCompraPesoBruto,
                    IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString()),
                    IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString()),
                    Estado = item.TicketCompraEstado,
                    _Vehiculo = new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    },
                    _TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    },
                    _TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    },
                });
            }
            return ListaTicket;
        }
        public List<Ticket> ConsultarTiketsPorId(int IdTicket)
        {
            ListaTicket = new List<Ticket>();
            foreach (var item in ConexionBD.sp_ConsultarTicketPorCarroPorId(IdTicket))
            {
                List<Vehiculo> DatoVehiculo = new List<Vehiculo>();
                string IdAsignarTUAnulada = "";
                if (item.TicketCompraAnulada == true)
                {
                    IdAsignarTUAnulada = Seguridad.Encriptar(item.TicketCompraIdAsignacionTUAnulada.ToString());
                }
                if (item.VehiculoPlaca != null)
                {
                    DatoVehiculo.Add(new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    });
                }

                ListaTicket.Add(new Ticket()
                {
                    IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString()),
                    Codigo = item.TicketCompraCodigo,
                    FechaIngreso = item.TicketCompraFechaIngreso,
                    FechaSalida = item.TicketCompraFechaSalida,
                    PesoBruto = item.TicketCompraPesoBruto,
                    PesoTara = item.TicketCompraPesoTara,
                    PesoNeto = item.TicketCompraPesoNeto,
                    PesoSinImpurezas = item.TicketCompraPesoSinImpurezas,
                    PesoAPagar = item.TicketCompraPesoAPagar,
                    PrecioPorQuintal = item.TicketCompraPrecioPorQuintal,
                    IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString()),

                    IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString()),
                    PorcentajeImpureza = item.TicketCompraPorcentajeImpurezas,
                    PorcentajeHumedad = item.TicketCompraPorcentajeHumedad,
                    TotalAPagar = item.TicketCompraTotalAPagar,
                    Estado = item.TicketCompraEstado,
                    Anulada = item.TicketCompraAnulada,
                    _Vehiculo = DatoVehiculo.FirstOrDefault(),
                    IdAsignarTUAnulada = IdAsignarTUAnulada,
                    _TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    },
                    _TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    },
                });
            }
            return ListaTicket;
        }
        public bool EliminarTicket(int _idTicket)
        {
            try
            {
                ConexionBD.sp_EliminarTicket(_idTicket);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Ticket FinalizarTicket(Ticket _Ticket)
        {
            try
            {
                foreach (var item in ConexionBD.sp_FinalizarTicket(int.Parse(_Ticket.IdTicket), _Ticket.PesoTara, _Ticket.PorcentajeHumedad, _Ticket.PrecioPorQuintal, _Ticket.PorcentajeImpureza))
                {
                    _Ticket.IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString());
                    _Ticket.Codigo = item.TicketCompraCodigo;
                    _Ticket.FechaIngreso = item.TicketCompraFechaIngreso;
                    _Ticket.PesoBruto = item.TicketCompraPesoBruto;
                    _Ticket.IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString());
                    _Ticket.IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString());
                    _Ticket._PersonaEntidad = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                    _Ticket.IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString());
                    _Ticket.IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString());
                    _Ticket.Estado = item.TicketCompraEstado;
                    _Ticket.PesoTara = item.TicketCompraPesoTara;
                    _Ticket.PorcentajeHumedad = item.TicketCompraPorcentajeHumedad;
                    _Ticket.PrecioPorQuintal = item.TicketCompraPrecioPorQuintal;
                    _Ticket.PorcentajeImpureza = item.TicketCompraPorcentajeImpurezas;
                    _Ticket.FechaSalida = item.TicketCompraFechaSalida;
                    _Ticket.TotalAPagar = item.TicketCompraTotalAPagar;
                    _Ticket.PesoNeto = item.TicketCompraPesoNeto;
                    _Ticket.PesoSinImpurezas = item.TicketCompraPesoSinImpurezas;
                    _Ticket.PesoAPagar = item.TicketCompraPesoAPagar;
                    _Ticket._Vehiculo = new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    };
                    _Ticket._TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    };
                    _Ticket._TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    };
                }
                return _Ticket;
            }
            catch (Exception)
            {

                _Ticket.IdTicket = null;
                return _Ticket;
            }
        }
        public List<StockRubro> ConsultarStockRubro()
        {
            List<StockRubro> StockRubros = new List<StockRubro>();
            foreach (var item in ConexionBD.sp_ConsultarStockRubros())
            {
                StockRubros.Add(new StockRubro()
                {
                    IdStockRubro = Seguridad.Encriptar(item.StockRubroIdStockRubro.ToString()),
                    Stock = item.StockRubroStock,
                    _TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRubroIdentificador,
                        Estado = item.TipoRubroEstado
                    },
                });
            }
            return StockRubros;
        }
        public Ticket AnularTicket(Ticket _Ticket)
        {
            try
            {
                foreach (var item in ConexionBD.sp_AnularTicket(int.Parse(_Ticket.IdTicket), int.Parse(_Ticket.IdAsignarTU)))
                {
                    List<PersonaEntidad> Personas = new List<PersonaEntidad>();
                    PersonaEntidad DatoPersona = new PersonaEntidad();
                    DatoPersona = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.TicketCompraIdPersona.ToString()).FirstOrDefault();
                    if (DatoPersona == null)
                    {
                        DatoPersona = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                    }
                    string IdAsignarTUAnulada = "";
                    if (item.TicketCompraAnulada == true)
                    {
                        IdAsignarTUAnulada = Seguridad.Encriptar(item.TicketCompraIdAsignacionTUAnulada.ToString());
                    }

                    List<Vehiculo> DatoVehiculo = new List<Vehiculo>();
                    if (item.VehiculoIdVehiculo != null)
                    {
                        DatoVehiculo.Add(new Vehiculo()
                        {
                            IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                            Estado = item.VehiculoEstado,
                            Placa = item.VehiculoPlaca,
                            FechaCreacion = item.VehiculoFechaCreacion,
                            IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                        });
                    }
                    _Ticket.IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString());
                    _Ticket.Codigo = item.TicketCompraCodigo;
                    _Ticket.FechaIngreso = item.TicketCompraFechaIngreso;
                    _Ticket.FechaSalida = item.TicketCompraFechaSalida;
                    _Ticket.PesoBruto = item.TicketCompraPesoBruto;
                    _Ticket.PesoTara = item.TicketCompraPesoTara;
                    _Ticket.PesoNeto = item.TicketCompraPesoNeto;
                    _Ticket.PesoSinImpurezas = item.TicketCompraPesoSinImpurezas;
                    _Ticket.PesoAPagar = item.TicketCompraPesoAPagar;
                    _Ticket.PrecioPorQuintal = item.TicketCompraPrecioPorQuintal;
                    _Ticket.IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString());
                    //idvehiculo
                    _Ticket.IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString());
                    _Ticket.IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString());
                    _Ticket.IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString());
                    _Ticket.PorcentajeImpureza = item.TicketCompraPorcentajeImpurezas;
                    _Ticket.PorcentajeHumedad = item.TicketCompraPorcentajeHumedad;
                    _Ticket.TotalAPagar = item.TicketCompraTotalAPagar;
                    _Ticket.Estado = item.TicketCompraEstado;
                    _Ticket.Anulada = item.TicketCompraAnulada;
                    _Ticket._Vehiculo = DatoVehiculo.FirstOrDefault();
                    _Ticket.IdAsignarTUAnulada = IdAsignarTUAnulada;
                    _Ticket._PersonaEntidad = DatoPersona;
                    _Ticket._TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    };
                    _Ticket._TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    };
                }
                return _Ticket;
            }
            catch (Exception)
            {
                _Ticket.IdTicket = null;
                return _Ticket;
            }
        }
        public List<Ticket> ConsultarTiketsAnulados()
        {
            ListaTicket = new List<Ticket>();
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarTicketAnulados())
            {
                PersonaEntidad DatoPersona = new PersonaEntidad();
                DatoPersona = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.TicketCompraIdPersona.ToString()).FirstOrDefault();
                if (DatoPersona == null)
                {
                    DatoPersona = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                    Personas.Add(DatoPersona);
                }

                PersonaEntidad DatoPersonaAnulada = new PersonaEntidad();
                DatoPersonaAnulada = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.UsuarioIdPersona.ToString()).FirstOrDefault();
                if (DatoPersonaAnulada == null)
                {
                    DatoPersonaAnulada = GestionPersona.ConsultarPersonaPorId(item.UsuarioIdPersona).FirstOrDefault();
                    DatoPersonaAnulada.AsignacionTipoUsuario = new AsignacionTipoUsuario()
                    {
                        IdAsignacionTUEncriptada = Seguridad.Encriptar(item.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                        TipoUsuario = new TipoUsuario()
                        {
                            IdTipoUsuario = Seguridad.Encriptar(item.TipoUsuarioIdTipoUsuario.ToString()),
                            Identificacion = item.TipoUsuarioIdentificacion,
                            Descripcion = item.TipoUsuarioDescripcion,
                            FechaCreacion = item.TipoUsuarioFechaCreacion
                        },
                    };
                    Personas.Add(DatoPersonaAnulada);
                }
                List<Vehiculo> DatoVehiculo = new List<Vehiculo>();
                if (item.VehiculoIdVehiculo != null)
                {
                    DatoVehiculo.Add(new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    });
                }

                ListaTicket.Add(new Ticket()
                {
                    IdTicket = Seguridad.Encriptar(item.TicketCompraIdTicket.ToString()),
                    Codigo = item.TicketCompraCodigo,
                    FechaIngreso = item.TicketCompraFechaIngreso,
                    FechaSalida = item.TicketCompraFechaSalida,
                    PesoBruto = item.TicketCompraPesoBruto,
                    PesoTara = item.TicketCompraPesoTara,
                    PesoNeto = item.TicketCompraPesoNeto,
                    PesoSinImpurezas = item.TicketCompraPesoSinImpurezas,
                    PesoAPagar = item.TicketCompraPesoAPagar,
                    PrecioPorQuintal = item.TicketCompraPrecioPorQuintal,
                    IdAsignarTU = Seguridad.Encriptar(item.TicketCompraIdAsignarTU.ToString()),
                    //idvehiculo

                    IdPersona = Seguridad.Encriptar(item.TicketCompraIdPersona.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.TicketCompraIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TicketCompraIdTipoPresentacionRubros.ToString()),
                    PorcentajeImpureza = item.TicketCompraPorcentajeImpurezas,
                    PorcentajeHumedad = item.TicketCompraPorcentajeHumedad,
                    TotalAPagar = item.TicketCompraTotalAPagar,
                    Estado = item.TicketCompraEstado,
                    Anulada = item.TicketCompraAnulada,
                    _Vehiculo = DatoVehiculo.FirstOrDefault(),
                    IdAsignarTUAnulada = Seguridad.Encriptar(item.TicketCompraIdAsignacionTUAnulada.ToString()),
                    _PersonaEntidad = DatoPersona,
                    _PersonaQueDaAnular = DatoPersonaAnulada,
                    _TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    },
                    _TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    },
                });
            }
            return ListaTicket;
        }
    }
}
