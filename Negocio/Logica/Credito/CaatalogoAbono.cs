using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;

namespace Negocio.Logica.Credito
{
    public class CaatalogoAbono
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public Abono InsertarAbono(Abono _Abono)
        {
            foreach (var item in ConexionBD.sp_CrearAbono(int.Parse(_Abono.IdConfigurarVenta),int.Parse(_Abono.IdAsignarTU),_Abono.Monto))
            {
                _Abono.IdAbono = Seguridad.Encriptar(item.IdAbono.ToString());
                _Abono.IdAsignarTU = Seguridad.Encriptar(item.IdAsignacionTU.ToString());
                _Abono.IdConfigurarVenta = Seguridad.Encriptar(item.IdConfigurarVenta.ToString());
                _Abono.Monto = item.Monto;
                _Abono.FechaRegistro = item.FechaRegistro;
                _Abono._ConfigurarVenta = ConsultarConfigurarVentaPorId(item.IdConfigurarVenta);
            }
            return _Abono;
        }
        public List<SaldoPendiente> ConsultarSaldoPendiente(int IdConfigurarVenta)
        {
            List<SaldoPendiente> Saldo = new List<SaldoPendiente>();
            foreach (var item in ConexionBD.sp_ConsultarSaldoPorConfigurarVenta(IdConfigurarVenta))
            {
                Saldo.Add(new SaldoPendiente()
                {
                    IdSaldoPendiente = Seguridad.Encriptar(item.IdSaldoPendiente.ToString()),
                    IdConfigurarVenta = Seguridad.Encriptar(item.IdConfiguracionVenta.ToString()),
                    Pendiente = item.Pendiente,
                    TotalFactura = item.TotalFactura,
                    TotalInteres = item.TotalInteres,
                    FechaRegistro = item.FechaRegistro,
                });
            }
            return Saldo;
        }
        public ConfigurarVenta ConsultarConfigurarVentaPorId(int idConfigurarVenta)
        {
            ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
            foreach (var item3 in ConexionBD.sp_ConsultarConfigurarVentaPorId(idConfigurarVenta))
            {
                string estado = "0";
                if (item3.EstadoConfVenta == true)
                {
                    estado = "1";
                }
                else
                {
                    estado = "0";
                }
                _ConfigurarVenta._SaldoPendiente = ConsultarSaldoPendiente(item3.IdConfigurarVenta).FirstOrDefault();
                _ConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item3.IdConfigurarVenta.ToString());
                _ConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item3.IdCabeceraFactura.ToString());
                _ConfigurarVenta.IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString());
                _ConfigurarVenta.EstadoConfVenta = estado;
                _ConfigurarVenta._PersonaEntidad = BuscarPersona(item3.IdPersona);
                _ConfigurarVenta.FechaFinalCredito = item3.FechaFinCredito;
                _ConfigurarVenta.Efectivo = item3.Efectivo.ToString();
                _ConfigurarVenta.AplicaSeguro = item3.AplicaSeguro.ToString();
                if (item3.IdConfiguracionInteres != null)
                {
                    _ConfigurarVenta.IdConfiguracionInteres = Seguridad.Encriptar(item3.IdConfiguracionInteres.ToString());
                    _ConfigurarVenta.ConfiguracionInteres = ConsultarConfiguracionInteresPorId(item3.IdConfiguracionInteres).FirstOrDefault();
                }
                _ConfigurarVenta.Descuento = item3.Descuento;
            }
            return _ConfigurarVenta;
        }
        public List<ConfiguracionInteres> ConsultarConfiguracionInteresPorId(int? IdConfiguracionInteres)
        {
            List<ConfiguracionInteres> ListaConfiguracionInteres = new List<ConfiguracionInteres>();
            foreach (var item in ConexionBD.sp_ConsultarConfiguracionInteresPorId(IdConfiguracionInteres))
            {
                ListaConfiguracionInteres.Add(new ConfiguracionInteres()
                {
                    IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString()),
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    TasaInteres = item.TasaInteres,
                    IdTipoInteresMora = Seguridad.Encriptar(item.IdTipoInteresMora.ToString()),
                    TasaInteresMora = item.TasaInteresMora,
                    Estado = item.Estado,
                    utilizado = item.ConfigurarInteresUtilizado
                });
            }
            return ListaConfiguracionInteres;
        }
        public PersonaEntidad BuscarPersona(int IdPersona)
        {
            PersonaEntidad _Persona = new PersonaEntidad();
            foreach (var item in ConexionBD.sp_ConsultarListaPersonas().Where(p => p.IdPersona == IdPersona).ToList())
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                List<Correo> ListaCorreos = new List<Correo>();
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.IdPersona))
                {
                    ListaTelefonos.Add(new Telefono()
                    {
                        IdTelefono = Seguridad.Encriptar(item1.IdTelefono.ToString()),
                        IdPersona = Seguridad.Encriptar(item1.IdPersona.ToString()),
                        Numero = item1.Numero,
                        TipoTelefono = new TipoTelefono()
                        {
                            IdTipoTelefono = Seguridad.Encriptar(item1.IdTipoTelefono.ToString()),
                            Descripcion = item1.Descripcion,
                            Identificador = item1.Identificador,
                            FechaCreacion = item1.TipoTelefonoFechaCreacion,
                            Estado = item1.TipoTelefonoEstado,
                        },
                        FechaCreacion = item1.FechaCreacion,
                        Estado = item1.Estado,

                    });
                }
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.IdPersona))
                {
                    ListaCorreos.Add(new Correo()
                    {
                        IdCorreo = Seguridad.Encriptar(item2.IdCorreo.ToString()),
                        IdPersona = Seguridad.Encriptar(item2.IdPersona.ToString()),
                        CorreoValor = item2.Correo,
                        FechaCreacion = item2.FechaCreacion,
                        Estado = item2.Estado,
                    });
                }
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.IdPersona).Where(p => p.AsignacionPersonaParroquiaEstado == true).ToList())
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
                _Persona.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                _Persona.NumeroDocumento = item.NumeroDocumento;
                _Persona.ApellidoPaterno = item.ApellidoPaterno;
                _Persona.ApellidoMaterno = item.ApellidoMaterno;
                _Persona.PrimerNombre = item.PrimerNombre;
                _Persona.SegundoNombre = item.SegundoNombre;
                _Persona.IdTipoDocumento = Seguridad.Encriptar(item.IdTipoDocumento.ToString());
                _Persona.TipoDocumento = item.TipoDocumento;
                _Persona.ListaTelefono = ListaTelefonos;
                _Persona.ListaCorreo = ListaCorreos;
                _Persona.AsignacionPersonaComunidad = ListaAsignacionPersonaParroquia.FirstOrDefault();
            }
            return _Persona;
        }
        public List<Abono> ConsultarAbonoPorFactura(int idConfigurarVenta)
        {
            List<Abono> Abonos = new List<Abono>();
            foreach (var item in ConexionBD.sp_ConsultarAbonoPorFactura(idConfigurarVenta))
            {
                Abonos.Add(new Abono()
                {
                    IdAbono = Seguridad.Encriptar(item.IdAbono.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.IdAsignacionTU.ToString()),
                    Monto = item.Monto,
                    FechaRegistro = item.FechaRegistro,
                    IdConfigurarVenta = Seguridad.Encriptar(item.IdConfigurarVenta.ToString())
                });
            }
            return Abonos;
        }
    }
}
