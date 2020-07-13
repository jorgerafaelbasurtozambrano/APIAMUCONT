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
    public class CatalogoVentaRubro
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoVehiculo GestionVehiculo = new CatalogoVehiculo();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        public TicketVenta InsertarTicketVentaRubroPorCarro(TicketVenta _TicketVenta)
        {
            Vehiculo _Vehiculo = new Vehiculo();
            _Vehiculo = GestionVehiculo.ConsultarVehiculoPorPlaca(_TicketVenta._Vehiculo.Placa).FirstOrDefault();
            if (_Vehiculo == null)
            {
                _Vehiculo = new Vehiculo();
                _Vehiculo = GestionVehiculo.IngresarVehiculo(new Vehiculo() { Placa = _TicketVenta._Vehiculo.Placa, IdAsignarTU = _TicketVenta.IdAsignarTU });
                if (_Vehiculo.IdVehiculo == null)
                {
                    _TicketVenta.IdTicketVenta = null;
                    return _TicketVenta;
                }
            }
            _Vehiculo.IdVehiculo = Seguridad.DesEncriptar(_Vehiculo.IdVehiculo);

            foreach (var item in ConexionBD.sp_CrearTicketVentaPorCarro(int.Parse(_TicketVenta.IdPersonaCliente), int.Parse(_TicketVenta.IdPersonaChofer), int.Parse(_TicketVenta._TipoRubro.IdTipoRubro), int.Parse(_TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro), int.Parse(_TicketVenta.IdAsignarTU), int.Parse(_Vehiculo.IdVehiculo), _TicketVenta.PesoTara))
            {
                _TicketVenta.IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString());
                _TicketVenta.Codigo = item.VentaRubroCodigo;
                _TicketVenta.FechaIngreso = item.VentaRubroFechaEntrada;
                _TicketVenta.FechaSalida = item.VentaRubroFechaSalida;
                _TicketVenta.IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString());
                _TicketVenta._PersonaCliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                _TicketVenta.IdPersonaChofer = Seguridad.Encriptar(item.VentaRubroIdPersonaChofer.ToString());
                //int idChofer = int.Parse(item.VentaRubroIdPersonaChofer.ToString());
                _TicketVenta._PersonaChofer = GestionPersona.ConsultarPersonaPorId(int.Parse(Seguridad.DesEncriptar(_TicketVenta.IdPersonaChofer))).FirstOrDefault();
                _TicketVenta.IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString());
                _TicketVenta.IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString());
                _TicketVenta.IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString());
                _TicketVenta.IdVehiculo = Seguridad.Encriptar(item.VentaRubroIdVehiculo.ToString());
                _TicketVenta.PesoTara = item.VentaRubroPesoTara;
                _TicketVenta.PesoBruto = item.VentaRubroPesoBruto;
                _TicketVenta.PrecioPorQuintal = item.VentaRubroPrecioPorQuintal;
                _TicketVenta.PorcentajeImpureza = item.VentaRubroPorcentajeImpureza;
                _TicketVenta.PorcentajeHumedad = item.VentaRubroPorcentajeHumedad;
                _TicketVenta.PesoNeto = item.VentaRubroPesoNeto;
                _TicketVenta.PesoACobrar = item.VentaRubroPesoACobrar;
                _TicketVenta.TotalACobrar = item.VentaRubroTotalACobrar;
                _TicketVenta.Anulada = item.VentaRubroAnulado;
                _TicketVenta.Estado = item.VentaRubroEstado;
                _TicketVenta._Vehiculo = new Vehiculo()
                {
                    IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                    Estado = item.VehiculoEstado,
                    Placa = item.VehiculoPlaca,
                    FechaCreacion = item.VehiculoFechaCreacion,
                    IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                };
                _TicketVenta._TipoRubro = new TipoRubro()
                {
                    IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                    Descripcion = item.TipoRubroDescripcion,
                    FechaCreacion = item.TipoRubroFechaCreacion,
                    Identificador = item.TipoRuboIdentificador,
                    Estado = item.TipoRubroEstado
                };
                _TicketVenta._TipoPresentacionRubro = new TipoPresentacionRubro()
                {
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                    Descripcion = item.TipoPresentacionRubrosDescripcion,
                    FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                    Identificador = item.TipoPresentacionRubrosIdentificador,
                    Estado = item.TipoPresentacionRubrosEstado
                };
            }
            return _TicketVenta;
        }
        public TicketVenta InsertarTicketVentaRubroPorSaco(TicketVenta _TicketVenta)
        {
            foreach (var item in ConexionBD.sp_CrearTicketVentaPorSaco(int.Parse(_TicketVenta.IdPersonaCliente), int.Parse(_TicketVenta._TipoRubro.IdTipoRubro), int.Parse(_TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro), int.Parse(_TicketVenta.IdAsignarTU), _TicketVenta.PesoNeto, _TicketVenta.PorcentajeImpureza, _TicketVenta.PorcentajeHumedad, _TicketVenta.PrecioPorQuintal))
            {
                _TicketVenta.IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString());
                _TicketVenta.Codigo = item.VentaRubroCodigo;
                _TicketVenta.FechaIngreso = item.VentaRubroFechaEntrada;
                _TicketVenta.FechaSalida = item.VentaRubroFechaSalida;
                _TicketVenta.IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString());
                _TicketVenta._PersonaCliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                _TicketVenta.IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString());
                _TicketVenta.IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString());
                _TicketVenta.IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString());
                _TicketVenta.PesoTara = item.VentaRubroPesoTara;
                _TicketVenta.PesoBruto = item.VentaRubroPesoBruto;
                _TicketVenta.PrecioPorQuintal = item.VentaRubroPrecioPorQuintal;
                _TicketVenta.PorcentajeImpureza = item.VentaRubroPorcentajeImpureza;
                _TicketVenta.PorcentajeHumedad = item.VentaRubroPorcentajeHumedad;
                _TicketVenta.PesoNeto = item.VentaRubroPesoNeto;
                _TicketVenta.PesoACobrar = item.VentaRubroPesoACobrar;
                _TicketVenta.TotalACobrar = item.VentaRubroTotalACobrar;
                _TicketVenta.Estado = item.VentaRubroEstado;
                _TicketVenta.Anulada = item.VentaRubroAnulado;
                _TicketVenta._TipoRubro = new TipoRubro()
                {
                    IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                    Descripcion = item.TipoRubroDescripcion,
                    FechaCreacion = item.TipoRubroFechaCreacion,
                    Identificador = item.TipoRuboIdentificador,
                    Estado = item.TipoRubroEstado
                };
                _TicketVenta._TipoPresentacionRubro = new TipoPresentacionRubro()
                {
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                    Descripcion = item.TipoPresentacionRubrosDescripcion,
                    FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                    Identificador = item.TipoPresentacionRubrosIdentificador,
                    Estado = item.TipoPresentacionRubrosEstado
                };
            }
            return _TicketVenta;
        }
        public List<TicketVenta> ConsultarTicketVentaRubroPorPlaca(string Placa)
        {
            List<TicketVenta> ListaTicket = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarTicketVentaPorPlacaCarro(Placa))
            {
                ListaTicket.Add(new TicketVenta()
                {
                    IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString()),
                    Codigo = item.VentaRubroCodigo,
                    Anulada = item.VentaRubroAnulado,
                    FechaIngreso = item.VentaRubroFechaEntrada,
                    FechaSalida = item.VentaRubroFechaSalida,
                    IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString()),
                    IdPersonaChofer = Seguridad.Encriptar(item.VentaRubroIdPersonaChofer.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString()),
                    IdVehiculo = Seguridad.Encriptar(item.VentaRubroIdVehiculo.ToString()),
                    PesoTara = item.VentaRubroPesoTara,
                    PesoBruto = item.VentaRubroPesoBruto,
                    PrecioPorQuintal = item.VentaRubroPrecioPorQuintal,
                    PorcentajeImpureza = item.VentaRubroPorcentajeImpureza,
                    PorcentajeHumedad = item.VentaRubroPorcentajeHumedad,
                    PesoNeto = item.VentaRubroPesoNeto,
                    PesoACobrar = item.VentaRubroPesoACobrar,
                    TotalACobrar = item.VentaRubroTotalACobrar,
                    Estado = item.VentaRubroEstado,
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
                    }
                });
            }
            return ListaTicket;
        }
        public List<StockRubro> ConsultarStockRubroPorTipoRubro(int idTipoRubro)
        {
            List<StockRubro> StockRubros = new List<StockRubro>();
            foreach (var item in ConexionBD.sp_ConsultarStockRubroPorIdTipoRubro(idTipoRubro))
            {
                StockRubros.Add(new StockRubro()
                {
                    IdStockRubro = Seguridad.Encriptar(item.IdStockRubro.ToString()),
                    Stock = item.Stock,
                    IdTipoRubro = Seguridad.Encriptar(item.IdTipoRubro.ToString()),
                });
            }
            return StockRubros;
        }
        public List<TicketVenta> ConsultarTicketVentaFinalizados()
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            List<TicketVenta> _TicketVentas = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarTicketVentaFinalizados())
            {
                PersonaEntidad Cliente = new PersonaEntidad();
                PersonaEntidad AsignacionTU = new PersonaEntidad();
                List<PersonaEntidad> Chofer = new List<PersonaEntidad>();
                Cliente = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaCliente.ToString()).FirstOrDefault();
                if (Cliente == null)
                {
                    Cliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                    Personas.Add(Cliente);
                }
                AsignacionTU = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.UsuarioIdPersona.ToString()).FirstOrDefault();
                if (AsignacionTU == null)
                {
                    AsignacionTU = GestionPersona.ConsultarPersonaPorId(item.UsuarioIdPersona).FirstOrDefault();
                    AsignacionTU.AsignacionTipoUsuario = new AsignacionTipoUsuario()
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
                    Personas.Add(AsignacionTU);
                }
                List<Vehiculo> _Vehiculo = new List<Vehiculo>();
                string IdChofer = "";
                string IdVehiculo = "";
                if (item.VentaRubroIdVehiculo != null)
                {
                    IdChofer = Seguridad.Encriptar(item.VentaRubroIdPersonaChofer.ToString());
                    IdVehiculo = Seguridad.Encriptar(item.VentaRubroIdVehiculo.ToString());
                    _Vehiculo.Add(new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    });
                    int idChofer = item.VentaRubroIdPersonaChofer.Value;
                    Chofer.Add(GestionPersona.ConsultarPersonaPorId(idChofer).FirstOrDefault());
                    Personas.Add(Chofer.FirstOrDefault());
                }
                _TicketVentas.Add(new TicketVenta()
                {
                    IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString()),
                    Anulada = item.VentaRubroAnulado,
                    Codigo = item.VentaRubroCodigo,
                    FechaIngreso = item.VentaRubroFechaEntrada,
                    FechaSalida = item.VentaRubroFechaSalida,
                    IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString()),
                    _PersonaCliente = Cliente,
                    IdPersonaChofer = IdChofer,
                    IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString()),
                    IdVehiculo = IdVehiculo,
                    _PersonaChofer = Chofer.FirstOrDefault(),
                    PesoTara = item.VentaRubroPesoTara,
                    PesoBruto = item.VentaRubroPesoBruto,
                    _PersonaUsuario = AsignacionTU,
                    PrecioPorQuintal = item.VentaRubroPrecioPorQuintal,
                    PorcentajeImpureza = item.VentaRubroPorcentajeImpureza,
                    PorcentajeHumedad = item.VentaRubroPorcentajeHumedad,
                    PesoNeto = item.VentaRubroPesoNeto,
                    PesoACobrar = item.VentaRubroPesoACobrar,
                    TotalACobrar = item.VentaRubroTotalACobrar,
                    Estado = item.VentaRubroEstado,
                    _Vehiculo = _Vehiculo.FirstOrDefault(),
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
                    }
                });
            }
            return _TicketVentas;
        }
        public List<TicketVenta> ConsultarTicketVentaSinFinalizar()
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            List<TicketVenta> _TicketVentas = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarTicketVentaSinFinalizar())
            {
                PersonaEntidad Cliente = new PersonaEntidad();
                PersonaEntidad AsignacionTU = new PersonaEntidad();
                List<PersonaEntidad> Chofer = new List<PersonaEntidad>();
                Cliente = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaCliente.ToString()).FirstOrDefault();
                if (Cliente == null)
                {
                    Cliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                    Personas.Add(Cliente);
                }
                AsignacionTU = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.UsuarioIdPersona.ToString()).FirstOrDefault();
                if (AsignacionTU == null)
                {
                    AsignacionTU = GestionPersona.ConsultarPersonaPorId(item.UsuarioIdPersona).FirstOrDefault();
                    AsignacionTU.AsignacionTipoUsuario = new AsignacionTipoUsuario()
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
                    Personas.Add(AsignacionTU);
                }
                List<Vehiculo> _Vehiculo = new List<Vehiculo>();
                string IdChofer = "";
                string IdVehiculo = "";
                if (item.VentaRubroIdVehiculo != null)
                {
                    IdChofer = Seguridad.Encriptar(item.VentaRubroIdPersonaChofer.ToString());
                    IdVehiculo = Seguridad.Encriptar(item.VentaRubroIdVehiculo.ToString());
                    _Vehiculo.Add(new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    });
                    int idChofer = item.VentaRubroIdPersonaChofer.Value;
                    Chofer.Add(GestionPersona.ConsultarPersonaPorId(idChofer).FirstOrDefault());
                    Personas.Add(Chofer.FirstOrDefault());
                }
                _TicketVentas.Add(new TicketVenta()
                {
                    Anulada = item.VentaRubroAnulado,
                    IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString()),
                    Codigo = item.VentaRubroCodigo,
                    FechaIngreso = item.VentaRubroFechaEntrada,
                    FechaSalida = item.VentaRubroFechaSalida,
                    IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString()),
                    _PersonaCliente = Cliente,
                    IdPersonaChofer = IdChofer,
                    IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString()),
                    IdVehiculo = IdVehiculo,
                    _PersonaChofer = Chofer.FirstOrDefault(),
                    PesoTara = item.VentaRubroPesoTara,
                    PesoBruto = item.VentaRubroPesoBruto,
                    _PersonaUsuario = AsignacionTU,
                    PrecioPorQuintal = item.VentaRubroPrecioPorQuintal,
                    PorcentajeImpureza = item.VentaRubroPorcentajeImpureza,
                    PorcentajeHumedad = item.VentaRubroPorcentajeHumedad,
                    PesoNeto = item.VentaRubroPesoNeto,
                    PesoACobrar = item.VentaRubroPesoACobrar,
                    TotalACobrar = item.VentaRubroTotalACobrar,
                    Estado = item.VentaRubroEstado,
                    _Vehiculo = _Vehiculo.FirstOrDefault(),
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
                    }
                });
            }
            return _TicketVentas;
        }
        public List<TicketVenta> ConsultarTicketVentaRubroPorId(int IdTicketVenta)
        {
            List<TicketVenta> ListaTicket = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarTickenVentaPorId(IdTicketVenta))
            {
                ListaTicket.Add(new TicketVenta()
                {
                    Anulada = item.Anulado,
                    IdTicketVenta = Seguridad.Encriptar(item.IdVentaRubro.ToString()),
                    Codigo = item.Codigo,
                    FechaIngreso = item.FechaEntrada,
                    FechaSalida = item.FechaSalida,
                    IdPersonaCliente = Seguridad.Encriptar(item.IdPersonaCliente.ToString()),
                    IdTipoRubro = Seguridad.Encriptar(item.IdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.IdTipoPresentacionRubro.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString()),
                    PesoTara = item.PesoTara,
                    PesoBruto = item.PesoBruto,
                    PrecioPorQuintal = item.PrecioPorQuintal,
                    PorcentajeImpureza = item.PorcentajeImpureza,
                    PorcentajeHumedad = item.PorcentajeHumedad,
                    PesoNeto = item.PesoNeto,
                    PesoACobrar = item.PesoACobrar,
                    TotalACobrar = item.TotalACobrar,
                    Estado = item.Estado,
                });
            }
            return ListaTicket;
        }
        public bool EliminarTicketVenta(int IdTicketVenta)
        {
            try
            {
                ConexionBD.sp_EliminarTicketVenta(IdTicketVenta);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public TicketVenta FinalizarTicketVentaRubro(TicketVenta _TicketVenta)
        {
            try
            {
                foreach (var item in ConexionBD.sp_FinalizarTicketVenta(int.Parse(_TicketVenta.IdTicketVenta), _TicketVenta.PesoBruto, _TicketVenta.PrecioPorQuintal, _TicketVenta.PorcentajeImpureza, _TicketVenta.PorcentajeHumedad))
                {
                    PersonaEntidad AsignacionTU = new PersonaEntidad();
                    AsignacionTU = GestionPersona.ConsultarPersonaPorId(item.UsuarioIdPersona).FirstOrDefault();
                    AsignacionTU.AsignacionTipoUsuario = new AsignacionTipoUsuario()
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
                    _TicketVenta._PersonaCliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                    _TicketVenta._PersonaUsuario = AsignacionTU;
                    _TicketVenta.IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString());
                    _TicketVenta.Codigo = item.VentaRubroCodigo;
                    _TicketVenta.FechaIngreso = item.VentaRubroFechaEntrada;
                    _TicketVenta.FechaSalida = item.VentaRubroFechaSalida;
                    _TicketVenta.IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString());
                    _TicketVenta.IdPersonaChofer = Seguridad.Encriptar(item.VentaRubroIdPersonaChofer.ToString());
                    _TicketVenta._PersonaChofer = GestionPersona.ConsultarPersonaPorId(int.Parse(Seguridad.DesEncriptar(_TicketVenta.IdPersonaChofer))).FirstOrDefault();
                    _TicketVenta.IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString());
                    _TicketVenta.IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString());
                    _TicketVenta.IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString());
                    _TicketVenta.IdVehiculo = Seguridad.Encriptar(item.VentaRubroIdVehiculo.ToString());
                    _TicketVenta.PesoTara = item.VentaRubroPesoTara;
                    _TicketVenta.PesoBruto = item.VentaRubroPesoBruto;
                    _TicketVenta.PrecioPorQuintal = item.VentaRubroPrecioPorQuintal;
                    _TicketVenta.PorcentajeImpureza = item.VentaRubroPorcentajeImpureza;
                    _TicketVenta.PorcentajeHumedad = item.VentaRubroPorcentajeHumedad;
                    _TicketVenta.PesoNeto = item.VentaRubroPesoNeto;
                    _TicketVenta.PesoACobrar = item.VentaRubroPesoACobrar;
                    _TicketVenta.TotalACobrar = item.VentaRubroTotalACobrar;
                    _TicketVenta.Estado = item.VentaRubroEstado;
                    _TicketVenta._Vehiculo = new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    };
                    _TicketVenta._TipoRubro = new TipoRubro()
                    {
                        IdTipoRubro = Seguridad.Encriptar(item.TipoRubroIdTipoRubro.ToString()),
                        Descripcion = item.TipoRubroDescripcion,
                        FechaCreacion = item.TipoRubroFechaCreacion,
                        Identificador = item.TipoRuboIdentificador,
                        Estado = item.TipoRubroEstado
                    };
                    _TicketVenta._TipoPresentacionRubro = new TipoPresentacionRubro()
                    {
                        IdTipoPresentacionRubro = Seguridad.Encriptar(item.TipoPresentacionRubrosIdTipoPresentacionRubros.ToString()),
                        Descripcion = item.TipoPresentacionRubrosDescripcion,
                        FechaCreacion = item.TipoPresentacionRubrosFechaCreacion,
                        Identificador = item.TipoPresentacionRubrosIdentificador,
                        Estado = item.TipoPresentacionRubrosEstado
                    };
                }
                return _TicketVenta;
            }
            catch (Exception)
            {
                _TicketVenta.IdTicketVenta = null;
                return _TicketVenta;
            }
        }
        public bool AnularTicketVenta(TicketVenta _TicketVenta)
        {
            try
            {
                ConexionBD.sp_AnularTicketVenta(int.Parse(_TicketVenta.IdTicketVenta), int.Parse(_TicketVenta.IdAsignarTU));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<TicketVenta> ConsultarTicketVentaAnulados()
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            List<TicketVenta> _TicketVentas = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarTicketVentaAnulados())
            {
                PersonaEntidad Cliente = new PersonaEntidad();
                PersonaEntidad AsignacionTU = new PersonaEntidad();
                List<PersonaEntidad> Chofer = new List<PersonaEntidad>();
                Cliente = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaCliente.ToString()).FirstOrDefault();
                if (Cliente == null)
                {
                    Cliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                    Personas.Add(Cliente);
                }
                AsignacionTU = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.UsuarioIdPersona.ToString()).FirstOrDefault();
                if (AsignacionTU == null)
                {
                    AsignacionTU = GestionPersona.ConsultarPersonaPorId(item.UsuarioIdPersona).FirstOrDefault();
                    AsignacionTU.AsignacionTipoUsuario = new AsignacionTipoUsuario()
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
                    Personas.Add(AsignacionTU);
                }
                List<Vehiculo> _Vehiculo = new List<Vehiculo>();
                string IdChofer = "";
                string IdVehiculo = "";
                if (item.VentaRubroIdVehiculo != null)
                {
                    IdChofer = Seguridad.Encriptar(item.VentaRubroIdPersonaChofer.ToString());
                    IdVehiculo = Seguridad.Encriptar(item.VentaRubroIdVehiculo.ToString());
                    _Vehiculo.Add(new Vehiculo()
                    {
                        IdVehiculo = Seguridad.Encriptar(item.VehiculoIdVehiculo.ToString()),
                        Estado = item.VehiculoEstado,
                        Placa = item.VehiculoPlaca,
                        FechaCreacion = item.VehiculoFechaCreacion,
                        IdAsignarTU = Seguridad.Encriptar(item.VehiculoIdAsignarTU.ToString()),
                    });
                    int idChofer = item.VentaRubroIdPersonaChofer.Value;
                    Chofer.Add(GestionPersona.ConsultarPersonaPorId(idChofer).FirstOrDefault());
                    Personas.Add(Chofer.FirstOrDefault());
                }
                _TicketVentas.Add(new TicketVenta()
                {
                    IdTicketVenta = Seguridad.Encriptar(item.VentaRubroIdVentaRubro.ToString()),
                    Anulada = item.VentaRubroAnulado,
                    Codigo = item.VentaRubroCodigo,
                    FechaIngreso = item.VentaRubroFechaEntrada,
                    FechaSalida = item.VentaRubroFechaSalida,
                    IdPersonaCliente = Seguridad.Encriptar(item.VentaRubroIdPersonaCliente.ToString()),
                    _PersonaCliente = Cliente,
                    IdPersonaChofer = IdChofer,
                    IdTipoRubro = Seguridad.Encriptar(item.VentaRubroIdTipoRubro.ToString()),
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.VentaRubroIdTipoPresentacionRubro.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.VentaRubroIdAsignarTU.ToString()),
                    IdVehiculo = IdVehiculo,
                    _PersonaChofer = Chofer.FirstOrDefault(),
                    PesoTara = item.VentaRubroPesoTara,
                    PesoBruto = item.VentaRubroPesoBruto,
                    //_PersonaUsuario = AsignacionTU,
                    _PersonaQueDaAnular = AsignacionTU,
                    PrecioPorQuintal = item.VentaRubroPrecioPorQuintal,
                    PorcentajeImpureza = item.VentaRubroPorcentajeImpureza,
                    PorcentajeHumedad = item.VentaRubroPorcentajeHumedad,
                    PesoNeto = item.VentaRubroPesoNeto,
                    PesoACobrar = item.VentaRubroPesoACobrar,
                    TotalACobrar = item.VentaRubroTotalACobrar,
                    Estado = item.VentaRubroEstado,
                    _Vehiculo = _Vehiculo.FirstOrDefault(),
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
                    }
                });
            }
            return _TicketVentas;
        }
    }
}
