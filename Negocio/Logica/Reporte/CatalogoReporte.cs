using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades.Reporte;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Credito;

namespace Negocio.Logica.Reporte
{
    public class CatalogoReporte
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        CatalogoConfiguracionInteres GestionInteres = new CatalogoConfiguracionInteres();
        CatalogoDetalleVenta GestionDetalleVenta = new CatalogoDetalleVenta();
        public PromedioPreciosCompraVenta PrecioCompra(string Inicio, string Fin)
        {
            List<string> Fecha = new List<string>();
            List<decimal> CompraPrecios = new List<decimal>();
            List<decimal> VentaPrecios = new List<decimal>();
            List<PromedioPrecios> Compra = new List<PromedioPrecios>();
            List<PromedioPrecios> Venta = new List<PromedioPrecios>();
            foreach (var item in ConexionBD.sp_ConsultarPromedioPrecioCompraPorFecha(Inicio, Fin))
            {
                Compra.Add(new PromedioPrecios()
                {
                    FechaInicio = item.FechaIngreso,
                    Precio = item.PromedioPrecio
                });
                Fecha.Add(item.FechaIngreso);
            }
            foreach (var item in ConexionBD.sp_ConsultarPromedioPrecioVentaPorFecha(Inicio, Fin))
            {
                Venta.Add(new PromedioPrecios()
                {
                    FechaInicio = item.FechaIngreso,
                    Precio = item.PromedioPrecio
                });
                if (Fecha.Where(p => p.Contains(item.FechaIngreso)).FirstOrDefault() == null )
                {
                    Fecha.Add(item.FechaIngreso);
                }
            }
            Fecha.Sort();
            foreach (var item in Fecha)
            {
                PromedioPrecios _precioCompra = new PromedioPrecios();
                PromedioPrecios _precioVenta = new PromedioPrecios();
                _precioCompra = Compra.Where(p => p.FechaInicio == item).FirstOrDefault();
                _precioVenta = Venta.Where(p => p.FechaInicio == item).FirstOrDefault();
                if (_precioCompra == null)
                {
                    CompraPrecios.Add(0);
                }
                else
                {
                    CompraPrecios.Add(_precioCompra.Precio.Value);
                }
                if (_precioVenta == null)
                {
                    VentaPrecios.Add(0);
                }
                else
                {
                    VentaPrecios.Add(_precioVenta.Precio.Value);
                }
            }

            PromedioPreciosCompraVenta Dato = new PromedioPreciosCompraVenta();
            Dato.Fecha = Fecha;
            Dato.PrecioCompra = CompraPrecios;
            Dato.PrecioVenta = VentaPrecios;
            return Dato;
        }
        public List<Ticket> ConsultarCompraRubroPorPersona(string NumeroDocumento,string Inicio,string Fin)
        {
            List<Ticket>  ListaTicket = new List<Ticket>();
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarCompraRubroPorPersona(NumeroDocumento, Inicio, Fin))
            {
                if (Personas.FirstOrDefault() == null)
                {
                    Personas.Add(GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault());
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
                    _PersonaEntidad = Personas.FirstOrDefault(),
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
        public List<Ticket> ConsultarCompraRubroPorPresentacionRubro(string Identificador, string Inicio, string Fin)
        {
            List<Ticket> ListaTicket = new List<Ticket>();
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarCompraRubroPorPresentacionRubro(int.Parse(Identificador), Inicio, Fin))
            {
                PersonaEntidad _Persona = new PersonaEntidad();
                _Persona = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.TicketCompraIdPersona.ToString()).FirstOrDefault();
                if (_Persona == null)
                {
                    _Persona = GestionPersona.ConsultarPersonaPorId(item.TicketCompraIdPersona).FirstOrDefault();
                    Personas.Add(_Persona);
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
                    _PersonaEntidad = _Persona,
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
        public List<TicketVenta> ConsultarVentaRubroPorPersona(string NumeroDocumento, string Inicio, string Fin)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            List<TicketVenta> _TicketVentas = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarVentaRubroPorPersona(NumeroDocumento, Inicio, Fin))
            {
                PersonaEntidad Cliente = new PersonaEntidad();
                PersonaEntidad AsignacionTU = new PersonaEntidad();
                List<PersonaEntidad> Chofer = new List<PersonaEntidad>();
                PersonaEntidad _chofer = new PersonaEntidad();
                Cliente = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaCliente.ToString()).FirstOrDefault();
                if (Cliente == null)
                {
                    Cliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                    Personas.Add(Cliente);
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
                    _chofer = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaChofer.ToString()).FirstOrDefault();
                    if (_chofer == null)
                    {
                        _chofer = GestionPersona.ConsultarPersonaPorId(idChofer).FirstOrDefault();
                        Personas.Add(_chofer);
                    }
                    Chofer.Add(_chofer);
                }
                _TicketVentas.Add(new TicketVenta()
                {
                    Modificado = item.VentaRubroModificado,
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
        public List<TicketVenta> ConsultarVentaRubroPorPresentacionRubro(string Identificador, string Inicio, string Fin)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            List<TicketVenta> _TicketVentas = new List<TicketVenta>();
            foreach (var item in ConexionBD.sp_ConsultarVentaRubroPorPresentacionRubro(int.Parse(Identificador), Inicio, Fin))
            {
                PersonaEntidad Cliente = new PersonaEntidad();
                PersonaEntidad AsignacionTU = new PersonaEntidad();
                List<PersonaEntidad> Chofer = new List<PersonaEntidad>();
                PersonaEntidad _chofer = new PersonaEntidad();
                Cliente = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaCliente.ToString()).FirstOrDefault();
                if (Cliente == null)
                {
                    Cliente = GestionPersona.ConsultarPersonaPorId(item.VentaRubroIdPersonaCliente).FirstOrDefault();
                    Personas.Add(Cliente);
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
                }
                if (item.VentaRubroIdPersonaChofer != null)
                {
                    int idChofer = item.VentaRubroIdPersonaChofer.Value;
                    _chofer = Personas.Where(p => Seguridad.DesEncriptar(p.IdPersona) == item.VentaRubroIdPersonaChofer.ToString()).FirstOrDefault();
                    if (_chofer == null)
                    {
                        _chofer = GestionPersona.ConsultarPersonaPorId(idChofer).FirstOrDefault();
                        Personas.Add(_chofer);
                    }
                    Chofer.Add(_chofer);
                }
                _TicketVentas.Add(new TicketVenta()
                {
                    Modificado = item.VentaRubroModificado,
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
        public ComparacionQuintales ConsultarComparacionQuintales(string Inicio,string Fin)
        {
            ComparacionQuintales _ComparacionQuintales = new ComparacionQuintales();
            foreach (var item in ConexionBD.sp_ConsultarQuintalesComprado(Inicio, Fin))
            {
                _ComparacionQuintales.QuintalesComprados = item.Value;
            }
            foreach (var item in ConexionBD.sp_ConsultarQuintalesVendidos(Inicio, Fin))
            {
                _ComparacionQuintales.QuintalesVendidos = item.Value;
            }
            decimal total = _ComparacionQuintales.QuintalesComprados + _ComparacionQuintales.QuintalesVendidos;
            if (total != 0)
            {
                if (_ComparacionQuintales.QuintalesComprados == 0 && _ComparacionQuintales.QuintalesVendidos >0 )
                {
                    _ComparacionQuintales.PorcentajeQuintalesVendidos = 100;
                }
                else if (_ComparacionQuintales.QuintalesVendidos == 0 && _ComparacionQuintales.QuintalesComprados > 0)
                {
                    _ComparacionQuintales.PorcentajeQuintalesComprados = 100;
                }
                else
                {
                    _ComparacionQuintales.PorcentajeQuintalesComprados = (_ComparacionQuintales.QuintalesComprados * 100) / total;
                    _ComparacionQuintales.PorcentajeQuintalesVendidos = (_ComparacionQuintales.QuintalesVendidos * 100) / total;
                }
            }
            else
            {
                _ComparacionQuintales.PorcentajeQuintalesComprados = 0;
                _ComparacionQuintales.PorcentajeQuintalesVendidos = 0;
            }
            return _ComparacionQuintales;
        }
        public PromedioPreciosCompraVenta HumedadPromedio(string Inicio, string Fin)
        {
            List<string> Fecha = new List<string>();
            List<decimal> CompraHumedad = new List<decimal>();
            List<decimal> VentaHumedad = new List<decimal>();
            List<PromedioPrecios> Compra = new List<PromedioPrecios>();
            List<PromedioPrecios> Venta = new List<PromedioPrecios>();
            foreach (var item in ConexionBD.sp_ConsultarPromedioHumedadCompraPorFecha(Inicio, Fin))
            {
                Compra.Add(new PromedioPrecios()
                {
                    FechaInicio = item.FechaIngreso,
                    Precio = item.PromedioHumedad
                });
                Fecha.Add(item.FechaIngreso);
            }
            foreach (var item in ConexionBD.sp_ConsultarPromedioHumedadVentaPorFecha(Inicio, Fin))
            {
                Venta.Add(new PromedioPrecios()
                {
                    FechaInicio = item.FechaIngreso,
                    Precio = item.PromedioHumedad
                });
                if (Fecha.Where(p => p.Contains(item.FechaIngreso)).FirstOrDefault() == null)
                {
                    Fecha.Add(item.FechaIngreso);
                }
            }
            Fecha.Sort();
            foreach (var item in Fecha)
            {
                PromedioPrecios _precioCompra = new PromedioPrecios();
                PromedioPrecios _precioVenta = new PromedioPrecios();
                _precioCompra = Compra.Where(p => p.FechaInicio == item).FirstOrDefault();
                _precioVenta = Venta.Where(p => p.FechaInicio == item).FirstOrDefault();
                if (_precioCompra == null)
                {
                    CompraHumedad.Add(0);
                }
                else
                {
                    CompraHumedad.Add(_precioCompra.Precio.Value);
                }
                if (_precioVenta == null)
                {
                    VentaHumedad.Add(0);
                }
                else
                {
                    VentaHumedad.Add(_precioVenta.Precio.Value);
                }
            }

            PromedioPreciosCompraVenta Dato = new PromedioPreciosCompraVenta();
            Dato.Fecha = Fecha;
            Dato.PrecioCompra = CompraHumedad;
            Dato.PrecioVenta = VentaHumedad;
            return Dato;
        }
        public List<CabeceraFactura> ConsultarFacturasPendientes(string Inicio, string Fin)
        {
            List<CabeceraFactura> _listaFacturas = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarFacturasPendientes(Inicio, Fin))
            {
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),                    
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            IdSaldoPendiente = Seguridad.Encriptar(item.SaldoPendienteIdSaldoPendiente.ToString()),
                            IdConfigurarVenta = Seguridad.Encriptar(item.SaldoPendienteIdConfiguracionVenta.ToString()),
                            Pendiente = item.SaldoPendientePendiente,
                            TotalFactura = item.SaldoPendienteTotalFactura,
                            TotalInteres = item.SaldoPendienteTotalInteres,
                            FechaRegistro = item.SaldoPendienteFechaRegistro,
                        }
                    }
                });
            }
            return _listaFacturas;
        }
        public List<CabeceraFactura> ConsultarFacturasPendientesPorIdPersona(string IdPersona)
        {
            List<CabeceraFactura> _listaFacturas = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarFacturasPendientesPorIdPersona(int.Parse(IdPersona)))
            {
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            IdSaldoPendiente = Seguridad.Encriptar(item.SaldoPendienteIdSaldoPendiente.ToString()),
                            IdConfigurarVenta = Seguridad.Encriptar(item.SaldoPendienteIdConfiguracionVenta.ToString()),
                            Pendiente = item.SaldoPendientePendiente,
                            TotalFactura = item.SaldoPendienteTotalFactura,
                            TotalInteres = item.SaldoPendienteTotalInteres,
                            FechaRegistro = item.SaldoPendienteFechaRegistro,
                        }
                    }
                });
            }
            return _listaFacturas;
        }
        public List<CabeceraFactura> ConsultarFacturasVencidas(string Inicio, string Fin)
        {
            List<CabeceraFactura> _listaFacturas = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarFacturasVencidas(Inicio, Fin))
            {
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            IdSaldoPendiente = Seguridad.Encriptar(item.SaldoPendienteIdSaldoPendiente.ToString()),
                            IdConfigurarVenta = Seguridad.Encriptar(item.SaldoPendienteIdConfiguracionVenta.ToString()),
                            Pendiente = item.SaldoPendientePendiente,
                            TotalFactura = item.SaldoPendienteTotalFactura,
                            TotalInteres = item.SaldoPendienteTotalInteres,
                            FechaRegistro = item.SaldoPendienteFechaRegistro,
                        }
                    }
                });
            }
            return _listaFacturas;
        }
        public List<CabeceraFactura> ConsultarFacturasCanceladas(string Inicio, string Fin)
        {
            List<CabeceraFactura> _listaFacturas = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarFacturasCreditoCanceladas(Inicio, Fin))
            {
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            IdSaldoPendiente = Seguridad.Encriptar(item.SaldoPendienteIdSaldoPendiente.ToString()),
                            IdConfigurarVenta = Seguridad.Encriptar(item.SaldoPendienteIdConfiguracionVenta.ToString()),
                            Pendiente = item.total,
                            TotalFactura = item.SaldoPendienteTotalFactura,
                            TotalInteres = item.SaldoPendienteTotalInteres,
                            FechaRegistro = item.SaldoPendienteFechaRegistro,
                        }
                    }
                });
            }
            decimal? TotalDescuento = 0;
            decimal? TotalIva = 0;
            decimal? TotalSubtotal = 0;
            foreach (var item in ConexionBD.sp_ConsultarFacturasCanceladas(Inicio, Fin))
            {
                TotalDescuento = 0;
                TotalIva = 0;
                TotalSubtotal = 0;
                foreach (var item1 in GestionDetalleVenta.ListarDetalleVentaPorFactura(item.CabeceraFacturaIdCabeceraFactura))
                {
                    decimal ValorUnitario = 0;
                    decimal? Total = 0;
                    decimal? Subtotal = 0;
                    decimal? IvaAnadido = 0;
                    decimal? Descuento = 0;
                    ValorUnitario = item1.ValorUnitario;
                    Subtotal = ValorUnitario * item1.Cantidad;
                    if (item1.AplicaDescuento == "True")
                    {
                        if (item1.Iva > 0)
                        {
                            IvaAnadido = (Subtotal * (Convert.ToDecimal(item1.Iva) / 100));
                        }
                        Descuento = (Subtotal + IvaAnadido) * (Convert.ToDecimal(item1.PorcentajeDescuento) / 100);
                        Total = (Subtotal + IvaAnadido) - Descuento;
                    }
                    else
                    {
                        if (item1.Iva > 0)
                        {
                            IvaAnadido = Subtotal * (Convert.ToDecimal(item1.Iva) / 100);
                        }
                        Total = (Subtotal + IvaAnadido) - Descuento;
                    }
                    TotalDescuento = TotalDescuento + Descuento;
                    TotalSubtotal = TotalSubtotal + Subtotal;
                    TotalIva = TotalIva + IvaAnadido;
                }
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            Pendiente = TotalSubtotal + TotalIva - TotalDescuento,
                        }
                    }
                });
            }
            return _listaFacturas;
        }
        public List<CabeceraFactura> ConsultarFacturasEmitidas(string Inicio, string Fin)
        {
            List<CabeceraFactura> _listaFacturas = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarFacturasCreditoEmitidas(Inicio, Fin))
            {
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            IdSaldoPendiente = Seguridad.Encriptar(item.SaldoPendienteIdSaldoPendiente.ToString()),
                            IdConfigurarVenta = Seguridad.Encriptar(item.SaldoPendienteIdConfiguracionVenta.ToString()),
                            Pendiente = item.SaldoPendienteTotalFactura,
                            TotalFactura = item.SaldoPendienteTotalFactura,
                            TotalInteres = item.SaldoPendienteTotalInteres,
                            FechaRegistro = item.SaldoPendienteFechaRegistro,
                        }
                    }
                });
            }
            decimal? TotalDescuento = 0;
            decimal? TotalIva = 0;
            decimal? TotalSubtotal = 0;
            foreach (var item in ConexionBD.sp_ConsultarFacturasCanceladas(Inicio, Fin))
            {
                TotalDescuento = 0;
                TotalIva = 0;
                TotalSubtotal = 0;
                foreach (var item1 in GestionDetalleVenta.ListarDetalleVentaPorFactura(item.CabeceraFacturaIdCabeceraFactura))
                {
                    decimal ValorUnitario = 0;
                    decimal? Total = 0;
                    decimal? Subtotal = 0;
                    decimal? IvaAnadido = 0;
                    decimal? Descuento = 0;
                    ValorUnitario = item1.ValorUnitario;
                    Subtotal = ValorUnitario * item1.Cantidad;
                    if (item1.AplicaDescuento == "True")
                    {
                        if (item1.Iva > 0)
                        {
                            IvaAnadido = (Subtotal * (Convert.ToDecimal(item1.Iva) / 100));
                        }
                        Descuento = (Subtotal + IvaAnadido) * (Convert.ToDecimal(item1.PorcentajeDescuento) / 100);
                        Total = (Subtotal + IvaAnadido) - Descuento;
                    }
                    else
                    {
                        if (item1.Iva > 0)
                        {
                            IvaAnadido = Subtotal * (Convert.ToDecimal(item1.Iva) / 100);
                        }
                        Total = (Subtotal + IvaAnadido) - Descuento;
                    }
                    TotalDescuento = TotalDescuento + Descuento;
                    TotalSubtotal = TotalSubtotal + Subtotal;
                    TotalIva = TotalIva + IvaAnadido;
                }
                List<PersonaEntidad> _PersonaEntidad = new List<PersonaEntidad>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    _PersonaEntidad.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                }
                _listaFacturas.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    PersonaEntidad = _PersonaEntidad.FirstOrDefault(),
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.ConfigurarVentaIdCabeceraFactura.ToString()),
                        IdPersona = Seguridad.Encriptar(item.ConfigurarVentaIdPersona.ToString()),
                        FechaFinalCredito = item.ConfigurarVentaFechaFinCredito,
                        Efectivo = item.ConfigurarVentaEfectivo.ToString(),
                        AplicaSeguro = item.ConfigurarVentaAplicaSeguro.ToString(),
                        IdConfiguracionInteres = Seguridad.Encriptar(item.ConfigurarVentaIdConfiguracionInteres.ToString()),
                        ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item.ConfigurarVentaIdConfiguracionInteres).FirstOrDefault(),
                        Descuento = item.ConfigurarVentaDescuento,
                        _PersonaEntidad = new PersonaEntidad()
                        {
                            IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                            NumeroDocumento = item.PersonaNumeroDocumento,
                            ApellidoPaterno = item.PersonaApellidoPaterno,
                            ApellidoMaterno = item.PersonaApellidoMaterno,
                            PrimerNombre = item.PersonaPrimerNombre,
                            SegundoNombre = item.PersonaSegundoNombre,
                            IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                            TipoDocumento = item.TipoDocumentoDescripcion
                        },
                        _SaldoPendiente = new SaldoPendiente()
                        {
                            Pendiente = TotalSubtotal + TotalIva - TotalDescuento,
                        }
                    }
                });
            }
            return _listaFacturas;
        }
        public List<Abono> ConsultarAbonoPorId(string IdAbono)
        {
            List<Abono> _Abono = new List<Abono>();
            foreach (var item in ConexionBD.sp_ConsultarAbonoPorId(int.Parse(IdAbono)))
            {
                _Abono.Add(new Abono()
                {
                    IdAbono = item.AbonoIdAbono.ToString(),
                    FechaRegistro = item.AbonoFechaRegistro,
                    Monto = item.AbonoMonto,
                    _ConfigurarVenta = new ConfigurarVenta()
                    {
                        IdCabeceraFactura = item.ConfigurarVentaIdCabeceraFactura.ToString()
                    }
                });
            }
            return _Abono;
        }
        public List<Tecnico> ConsultarTecnicosParaSeguimiento()
        {
            List<Tecnico> _Tecnicos = new List<Tecnico>();
            foreach (var item in ConexionBD.sp_ConsultarTecnicosConPersonasParaSeguimientos())
            {
                List<PersonaEntidad> _Cliente = new List<PersonaEntidad>();
                foreach (var item1 in ConexionBD.sp_ConsultarPersonasAsignadasATecnicosParaSeguimientos(item.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico))
                {
                    List<AsignarTecnicoPersonaComunidad> ListaComunidadesAsignadas = new List<AsignarTecnicoPersonaComunidad>();
                    foreach (var item2 in ConexionBD.sp_ConsultarComunidadesDeUnaPersonaAsignadoAUnTecnico(item1.AsignarTecnicoPersonaComunidadIdPersona, item.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico))
                    {
                        ListaComunidadesAsignadas.Add(new AsignarTecnicoPersonaComunidad()
                        {
                            NumeroVisita = item2.NumeroVisitas,
                            IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item2.AsignarTecnicoPersonaComunidadIdAsignarTecnicoPersonaComunidad
                            .ToString()),
                            IdAsignarTUTecnico = Seguridad.Encriptar(item2.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico.ToString()),
                            IdPersona = Seguridad.Encriptar(item2.AsignarTecnicoPersonaComunidadIdPersona.ToString()),
                            Comunidad = new Comunidad()
                            {
                                IdComunidad = Seguridad.Encriptar(item2.ComunidadIdComunidad.ToString()),
                                Descripcion = item2.ComunidadDescripcion,
                                Parroquia = new Parroquia()
                                {
                                    IdParroquia = Seguridad.Encriptar(item2.ParroquiaIdParroquia.ToString()),
                                    Descripcion = item2.ParroquiaDescripcion,
                                    Canton = new Canton()
                                    {
                                        IdCanton = Seguridad.Encriptar(item2.CantonIdCanton.ToString()),
                                        Descripcion = item2.CantonDescripcion,
                                        Provincia = new Provincia()
                                        {
                                            IdProvincia = Seguridad.Encriptar(item2.ProvinciaIdProvincia.ToString()),
                                            Descripcion = item2.ProvinciaDescripcion
                                        }
                                    }
                                }
                            }
                        });
                    }
                    List<Telefono> ListaTelefonos = new List<Telefono>();
                    foreach (var item3 in ConexionBD.sp_ConsultarTelefonoPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        ListaTelefonos.Add(new Telefono()
                        {
                            IdTelefono = Seguridad.Encriptar(item3.IdTelefono.ToString()),
                            IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString()),
                            Numero = item3.Numero,
                            TipoTelefono = new TipoTelefono()
                            {
                                IdTipoTelefono = Seguridad.Encriptar(item3.IdTipoTelefono.ToString()),
                                Descripcion = item3.Descripcion,
                                Identificador = item3.Identificador,
                                FechaCreacion = item3.TipoTelefonoFechaCreacion,
                                Estado = item3.TipoTelefonoEstado,
                            },
                            FechaCreacion = item3.FechaCreacion,
                            Estado = item3.Estado,

                        });
                    }
                    List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                    foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        ListaAsignacionPersonaParroquia.Add(new AsignacionPersonaParroquia()
                        {
                            Referencia = item3.AsignacionPersonaParroquiaReferencia,
                            IdPersona = Seguridad.Encriptar(item3.AsignacionPersonaComunidadIdPersona.ToString()),
                            IdAsignacionPC = Seguridad.Encriptar(item3.AsignacionPersonaParroquiaIdAsignacionPersonaParroquia.ToString()),
                            FechaCreacion = item3.AsignacionPersonaParroquiaFechaCreacion,
                            Estado = item3.AsignacionPersonaParroquiaEstado,
                            Parroquia = new Parroquia()
                            {
                                IdParroquia = Seguridad.Encriptar(item3.ParroquiaIdParroquia.ToString()),
                                Descripcion = item3.ParroquiaDescripcion,
                                FechaCreacion = item3.ParroquiaFechaCreacion,
                                Estado = item3.ParroquiaEstado,
                                Canton = new Canton()
                                {
                                    IdCanton = Seguridad.Encriptar(item3.CantonIdCanton.ToString()),
                                    Descripcion = item3.CantonDescripcion,
                                    FechaCreacion = item3.CantonFechaCreacion,
                                    Estado = item3.CantonEstado,
                                    Provincia = new Provincia()
                                    {
                                        IdProvincia = Seguridad.Encriptar(item3.ProvinciaIdProvincia.ToString()),
                                        Descripcion = item3.ProvinciaDescripcion,
                                        FechaCreacion = item3.ProvinciaFechaCreacion,
                                        Estado = item3.ProvinciaEstado,
                                    },
                                },
                            },
                        });
                    }
                    ListaAsignacionPersonaParroquia = ListaAsignacionPersonaParroquia.GroupBy(a => a.IdAsignacionPC).Select(grp => grp.First()).ToList();
                    _Cliente.Add(new PersonaEntidad()
                    {
                        PrimerNombre = item1.PersonaPrimerNombre,
                        SegundoNombre = item1.PersonaSegundoNombre,
                        ApellidoMaterno = item1.PersonaApellidoMaterno,
                        ApellidoPaterno = item1.PersonaApellidoPaterno,
                        NumeroDocumento = item1.PersonaNumeroDocumento,
                        EstadoUsuario = item1.PersonaEstado,
                        ListaTelefono = ListaTelefonos,
                        AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                        _objTipoDocumento = new TipoDocumento()
                        {
                            Identificador = item1.TipoDocumentoIdentificador,
                            Documento = item1.TipoDocumentoDescripcion
                        },
                        _AsignarTecnicoPersonaComunidad = ListaComunidadesAsignadas,
                    });
                }
                _Tecnicos.Add(new Tecnico()
                {
                    _Tecnico = new PersonaEntidad()
                    {
                        PrimerNombre = item.PersonaPrimerNombre,
                        SegundoNombre = item.PersonaSegundoNombre,
                        ApellidoMaterno = item.PersonaApellidoMaterno,
                        ApellidoPaterno = item.PersonaApellidoPaterno,
                        NumeroDocumento = item.PersonaNumeroDocumento,
                        EstadoUsuario = item.PersonaEstado,
                        _objTipoDocumento = new TipoDocumento()
                        {
                            Identificador = item.TipoDocumentoIdentificador,
                            Documento = item.TipoDocumentoDescripcion
                        }
                    },
                    _Clientes = _Cliente
                });
            }
            return _Tecnicos;
        }
        public List<Tecnico> ConsultarTecnicosParaSeguimientoPorTecnico(int idAsignarTUTecnico)
        {
            List<Tecnico> _Tecnicos = new List<Tecnico>();
            foreach (var item in ConexionBD.sp_ConsultarTecnicoParaSeguimiento(idAsignarTUTecnico))
            {
                List<PersonaEntidad> _Cliente = new List<PersonaEntidad>();
                foreach (var item1 in ConexionBD.sp_ConsultarPersonasAsignadasATecnicosParaSeguimientos(item.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico))
                {
                    List<AsignarTecnicoPersonaComunidad> ListaComunidadesAsignadas = new List<AsignarTecnicoPersonaComunidad>();
                    foreach (var item2 in ConexionBD.sp_ConsultarComunidadesDeUnaPersonaAsignadoAUnTecnico(item1.AsignarTecnicoPersonaComunidadIdPersona, item.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico))
                    {
                        ListaComunidadesAsignadas.Add(new AsignarTecnicoPersonaComunidad()
                        {
                            NumeroVisita = item2.NumeroVisitas,
                            IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item2.AsignarTecnicoPersonaComunidadIdAsignarTecnicoPersonaComunidad
                            .ToString()),
                            IdAsignarTUTecnico = Seguridad.Encriptar(item2.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico.ToString()),
                            IdPersona = Seguridad.Encriptar(item2.AsignarTecnicoPersonaComunidadIdPersona.ToString()),
                            Comunidad = new Comunidad()
                            {
                                IdComunidad = Seguridad.Encriptar(item2.ComunidadIdComunidad.ToString()),
                                Descripcion = item2.ComunidadDescripcion,
                                Parroquia = new Parroquia()
                                {
                                    IdParroquia = Seguridad.Encriptar(item2.ParroquiaIdParroquia.ToString()),
                                    Descripcion = item2.ParroquiaDescripcion,
                                    Canton = new Canton()
                                    {
                                        IdCanton = Seguridad.Encriptar(item2.CantonIdCanton.ToString()),
                                        Descripcion = item2.CantonDescripcion,
                                        Provincia = new Provincia()
                                        {
                                            IdProvincia = Seguridad.Encriptar(item2.ProvinciaIdProvincia.ToString()),
                                            Descripcion = item2.ProvinciaDescripcion
                                        }
                                    }
                                }
                            }
                        });
                    }
                    List<Telefono> ListaTelefonos = new List<Telefono>();
                    foreach (var item3 in ConexionBD.sp_ConsultarTelefonoPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        ListaTelefonos.Add(new Telefono()
                        {
                            IdTelefono = Seguridad.Encriptar(item3.IdTelefono.ToString()),
                            IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString()),
                            Numero = item3.Numero,
                            TipoTelefono = new TipoTelefono()
                            {
                                IdTipoTelefono = Seguridad.Encriptar(item3.IdTipoTelefono.ToString()),
                                Descripcion = item3.Descripcion,
                                Identificador = item3.Identificador,
                                FechaCreacion = item3.TipoTelefonoFechaCreacion,
                                Estado = item3.TipoTelefonoEstado,
                            },
                            FechaCreacion = item3.FechaCreacion,
                            Estado = item3.Estado,

                        });
                    }
                    List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                    foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        ListaAsignacionPersonaParroquia.Add(new AsignacionPersonaParroquia()
                        {
                            Referencia = item3.AsignacionPersonaParroquiaReferencia,
                            IdPersona = Seguridad.Encriptar(item3.AsignacionPersonaComunidadIdPersona.ToString()),
                            IdAsignacionPC = Seguridad.Encriptar(item3.AsignacionPersonaParroquiaIdAsignacionPersonaParroquia.ToString()),
                            FechaCreacion = item3.AsignacionPersonaParroquiaFechaCreacion,
                            Estado = item3.AsignacionPersonaParroquiaEstado,
                            Parroquia = new Parroquia()
                            {
                                IdParroquia = Seguridad.Encriptar(item3.ParroquiaIdParroquia.ToString()),
                                Descripcion = item3.ParroquiaDescripcion,
                                FechaCreacion = item3.ParroquiaFechaCreacion,
                                Estado = item3.ParroquiaEstado,
                                Canton = new Canton()
                                {
                                    IdCanton = Seguridad.Encriptar(item3.CantonIdCanton.ToString()),
                                    Descripcion = item3.CantonDescripcion,
                                    FechaCreacion = item3.CantonFechaCreacion,
                                    Estado = item3.CantonEstado,
                                    Provincia = new Provincia()
                                    {
                                        IdProvincia = Seguridad.Encriptar(item3.ProvinciaIdProvincia.ToString()),
                                        Descripcion = item3.ProvinciaDescripcion,
                                        FechaCreacion = item3.ProvinciaFechaCreacion,
                                        Estado = item3.ProvinciaEstado,
                                    },
                                },
                            },
                        });
                    }
                    ListaAsignacionPersonaParroquia = ListaAsignacionPersonaParroquia.GroupBy(a => a.IdAsignacionPC).Select(grp => grp.First()).ToList();
                    _Cliente.Add(new PersonaEntidad()
                    {
                        PrimerNombre = item1.PersonaPrimerNombre,
                        SegundoNombre = item1.PersonaSegundoNombre,
                        ApellidoMaterno = item1.PersonaApellidoMaterno,
                        ApellidoPaterno = item1.PersonaApellidoPaterno,
                        NumeroDocumento = item1.PersonaNumeroDocumento,
                        EstadoUsuario = item1.PersonaEstado,
                        ListaTelefono = ListaTelefonos,
                        AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                        _objTipoDocumento = new TipoDocumento()
                        {
                            Identificador = item1.TipoDocumentoIdentificador,
                            Documento = item1.TipoDocumentoDescripcion
                        },
                        _AsignarTecnicoPersonaComunidad = ListaComunidadesAsignadas,
                    });
                }
                _Tecnicos.Add(new Tecnico()
                {
                    _Tecnico = new PersonaEntidad()
                    {
                        PrimerNombre = item.PersonaPrimerNombre,
                        SegundoNombre = item.PersonaSegundoNombre,
                        ApellidoMaterno = item.PersonaApellidoMaterno,
                        ApellidoPaterno = item.PersonaApellidoPaterno,
                        NumeroDocumento = item.PersonaNumeroDocumento,
                        EstadoUsuario = item.PersonaEstado,
                        _objTipoDocumento = new TipoDocumento()
                        {
                            Identificador = item.TipoDocumentoIdentificador,
                            Documento = item.TipoDocumentoDescripcion
                        }
                    },
                    _Clientes = _Cliente
                });
            }
            return _Tecnicos;
        }
        public Decimal ConsultarValorRecuperadoVenta(string Inicio,string Fin)
        {
            Decimal valor = 0;
            foreach (var item in ConexionBD.sp_ConsultarValorRecuperadoVenta(Inicio, Fin))
            {
                valor = item.VALOR;
            }
            return valor;
        }
        public Decimal ConsultarValorInvertidoCompra(string Inicio, string Fin)
        {
            Decimal valor = 0;
            foreach (var item in ConexionBD.sp_ConsultarValorInvertidoCompra(Inicio, Fin))
            {
                valor = item.VALOR;
            }
            return valor;
        }
        public List<Tecnico> ConsultarTecnicosParaSeguimientoGerencia()
        {
            List<Tecnico> _Tecnicos = new List<Tecnico>();
            foreach (var item in ConexionBD.sp_ConsultarTecnicosConPersonasParaSeguimientos())
            {
                List<PersonaEntidad> _Cliente = new List<PersonaEntidad>();
                foreach (var item1 in ConexionBD.sp_ConsultarPersonasAsignadasATecnicosParaSeguimientos(item.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico))
                {
                    List<Telefono> ListaTelefonos = new List<Telefono>();
                    foreach (var item3 in ConexionBD.sp_ConsultarTelefonoPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        ListaTelefonos.Add(new Telefono()
                        {
                            IdTelefono = Seguridad.Encriptar(item3.IdTelefono.ToString()),
                            IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString()),
                            Numero = item3.Numero,
                            TipoTelefono = new TipoTelefono()
                            {
                                IdTipoTelefono = Seguridad.Encriptar(item3.IdTipoTelefono.ToString()),
                                Descripcion = item3.Descripcion,
                                Identificador = item3.Identificador,
                                FechaCreacion = item3.TipoTelefonoFechaCreacion,
                                Estado = item3.TipoTelefonoEstado,
                            },
                            FechaCreacion = item3.FechaCreacion,
                            Estado = item3.Estado,

                        });
                    }
                    List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                    foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        ListaAsignacionPersonaParroquia.Add(new AsignacionPersonaParroquia()
                        {
                            Referencia = item3.AsignacionPersonaParroquiaReferencia,
                            IdPersona = Seguridad.Encriptar(item3.AsignacionPersonaComunidadIdPersona.ToString()),
                            IdAsignacionPC = Seguridad.Encriptar(item3.AsignacionPersonaParroquiaIdAsignacionPersonaParroquia.ToString()),
                            FechaCreacion = item3.AsignacionPersonaParroquiaFechaCreacion,
                            Estado = item3.AsignacionPersonaParroquiaEstado,
                            Parroquia = new Parroquia()
                            {
                                IdParroquia = Seguridad.Encriptar(item3.ParroquiaIdParroquia.ToString()),
                                Descripcion = item3.ParroquiaDescripcion,
                                FechaCreacion = item3.ParroquiaFechaCreacion,
                                Estado = item3.ParroquiaEstado,
                                Canton = new Canton()
                                {
                                    IdCanton = Seguridad.Encriptar(item3.CantonIdCanton.ToString()),
                                    Descripcion = item3.CantonDescripcion,
                                    FechaCreacion = item3.CantonFechaCreacion,
                                    Estado = item3.CantonEstado,
                                    Provincia = new Provincia()
                                    {
                                        IdProvincia = Seguridad.Encriptar(item3.ProvinciaIdProvincia.ToString()),
                                        Descripcion = item3.ProvinciaDescripcion,
                                        FechaCreacion = item3.ProvinciaFechaCreacion,
                                        Estado = item3.ProvinciaEstado,
                                    },
                                },
                            },
                        });
                    }
                    ListaAsignacionPersonaParroquia = ListaAsignacionPersonaParroquia.GroupBy(a => a.IdAsignacionPC).Select(grp => grp.First()).ToList();
                    Decimal Pendiente = 0;
                    Decimal Abonado = 0;
                    Decimal CantidadComprada = 0;
                    Decimal CantidadAbonada = 0;
                    foreach (var item3 in ConexionBD.sp_ConsultarSaldoPendientePorPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        Pendiente = item3.Pendiente;
                    }
                    foreach (var item3 in ConexionBD.sp_ConsultarSaldoAbonadoPorPersona(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        Abonado = item3.Abonado;
                    }
                    foreach (var item3 in ConexionBD.sp_ConsultarTotalDeFacturasCompradasACreditoPorPersonaEnAnoActual(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        CantidadComprada = item3.CANTIDAD_COMPRADA;
                    }
                    foreach (var item3 in ConexionBD.sp_ConsultarTotalDeAbonosFacturasCompradasACreditoPorPersonaEnAnoActual(item1.AsignarTecnicoPersonaComunidadIdPersona))
                    {
                        CantidadAbonada = item3.CANTIDAD_ABONADA;
                    }
                    _Cliente.Add(new PersonaEntidad()
                    {
                        Pendiente = Pendiente,
                        Abonado = Abonado,
                        CantidadComprada = CantidadComprada,
                        CantidadAbonada = CantidadAbonada,
                        PrimerNombre = item1.PersonaPrimerNombre,
                        SegundoNombre = item1.PersonaSegundoNombre,
                        ApellidoMaterno = item1.PersonaApellidoMaterno,
                        ApellidoPaterno = item1.PersonaApellidoPaterno,
                        NumeroDocumento = item1.PersonaNumeroDocumento,
                        EstadoUsuario = item1.PersonaEstado,
                        ListaTelefono = ListaTelefonos,
                        AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                        _objTipoDocumento = new TipoDocumento()
                        {
                            Identificador = item1.TipoDocumentoIdentificador,
                            Documento = item1.TipoDocumentoDescripcion
                        },
                    });
                }
                _Tecnicos.Add(new Tecnico()
                {
                    _Tecnico = new PersonaEntidad()
                    {
                        PrimerNombre = item.PersonaPrimerNombre,
                        SegundoNombre = item.PersonaSegundoNombre,
                        ApellidoMaterno = item.PersonaApellidoMaterno,
                        ApellidoPaterno = item.PersonaApellidoPaterno,
                        NumeroDocumento = item.PersonaNumeroDocumento,
                        EstadoUsuario = item.PersonaEstado,
                        _objTipoDocumento = new TipoDocumento()
                        {
                            Identificador = item.TipoDocumentoIdentificador,
                            Documento = item.TipoDocumentoDescripcion
                        }
                    },
                    _Clientes = _Cliente
                });
            }
            return _Tecnicos;
        }
    }
}
