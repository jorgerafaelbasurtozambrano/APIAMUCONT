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
    public class CatalogoAsignarComunidadFactura
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public AsignarComunidadFactura InsertarComunidadConfigurarVenta(AsignarComunidadFactura _AsignarComunidadFactura)
        {
            foreach (var item in ConexionBD.sp_CrearAsignarComunidadFactura(int.Parse(_AsignarComunidadFactura.IdComunidad),int.Parse(_AsignarComunidadFactura.IdCabeceraFactura), _AsignarComunidadFactura.Observacion))
            {
                _AsignarComunidadFactura.IdAsignarComunidadFactura = Seguridad.Encriptar(item.IdAsignarComunidadFactura.ToString());
                _AsignarComunidadFactura.IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString());
                _AsignarComunidadFactura.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                _AsignarComunidadFactura.Observacion = item.Observacion;
                _AsignarComunidadFactura.FechaAsignacion = item.FechaAsignacion;
            }
            return _AsignarComunidadFactura;
        }
        public bool EliminarAsignarComunidadFactura(int _idAsignarComunidadFactura)
        {
            try
            {
                ConexionBD.sp_EliminarAsignarComunidadFactura(_idAsignarComunidadFactura);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<AsignarComunidadFactura> ConsultarAsignarComunidadFactura(int _IdCabeceraFactura)
        {
            List<AsignarComunidadFactura> _Lista_AsignarComunidadConfigurarVenta = new List<AsignarComunidadFactura>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarComunidadFacturaPorIdCabeceraFactura(_IdCabeceraFactura))
            {
                _Lista_AsignarComunidadConfigurarVenta.Add(new AsignarComunidadFactura()
                {
                    IdAsignarComunidadFactura = Seguridad.Encriptar(item.AsignarComunidadConfigurarVentaIdAsignarComunidadConfigurarVenta.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.AsignarComunidadConfigurarVentaIdComunidad.ToString()),
                    IdCabeceraFactura = Seguridad.Encriptar(item.AsignarComunidadConfigurarVentaIdConfigurarVenta.ToString()),
                    Estado = item.AsignarComunidadConfigurarVentaEstado.ToString(),
                    FechaAsignacion = item.AsignarComunidadConfigurarVentaFechaAsignacion,
                    Observacion = item.AsignarComunidadConfigurarVentaObservacion,
                    Comunidad = new Comunidad()
                    {
                        IdComunidad = Seguridad.Encriptar(item.ComunidadIdComunidad.ToString()),
                        Descripcion = item.ComunidadDescripcion,
                        Parroquia = new Parroquia()
                        {
                            IdParroquia = Seguridad.Encriptar(item.ParroquiaIdParroquia.ToString()),
                            Descripcion = item.ParroquiaDescripcion,
                            Canton = new Canton()
                            {
                                IdCanton = Seguridad.Encriptar(item.CantonIdCanton.ToString()),
                                Descripcion = item.CantonDescripcion,
                                Provincia = new Provincia()
                                {
                                    IdProvincia = Seguridad.Encriptar(item.ProvinciaIdProvincia.ToString()),
                                    Descripcion = item.ProvinciaDescripcion
                                }
                            }
                        }
                    }
                });
            }
            return _Lista_AsignarComunidadConfigurarVenta;
        }
        public List<AsignarComunidadFactura> ConsultarAsignarComunidadFacturaPorFacturaYPorComunidad(int _IdCabeceraFactura,int _IdComunidad)
        {
            List<AsignarComunidadFactura> _Lista_AsignarComunidadConfigurarVenta = new List<AsignarComunidadFactura>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarComunidadFacturaPorCabeceraFacturaYComunidad(_IdCabeceraFactura, _IdComunidad))
            {
                _Lista_AsignarComunidadConfigurarVenta.Add(new AsignarComunidadFactura()
                {
                    IdAsignarComunidadFactura = Seguridad.Encriptar(item.IdAsignarComunidadFactura.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                    Estado = item.Estado.ToString(),
                    FechaAsignacion = item.FechaAsignacion,
                    Observacion = item.Observacion,
                });
            }
            return _Lista_AsignarComunidadConfigurarVenta;
        }
        public List<PersonaEntidad> ConsultarPersonasEnFacturasParaSeguimiento()
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasParaRealizarSeguimiento())
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                List<Comunidad> _ListaComunidadFactura = new List<Comunidad>();
                foreach (var item4 in ConexionBD.sp_ConsultarComunidadAsignadaAFacturaDeUnaPersona(item.PersonaIdPersona))
                {
                    List<AsignarComunidadFactura> _ListaAsignarComunidadFactura = new List<AsignarComunidadFactura>();
                    foreach (var item5 in ConexionBD.sp_ConsultarAsignarComunidadFacturaPorPersonaYPorComunidad(item.PersonaIdPersona, item4.ComunidadIdComunidad))
                    {
                        _ListaAsignarComunidadFactura.Add(new AsignarComunidadFactura()
                        {
                            IdAsignarComunidadFactura = Seguridad.Encriptar(item5.IdAsignarComunidadFactura.ToString()),
                            IdComunidad = Seguridad.Encriptar(item5.IdComunidad.ToString()),
                            IdCabeceraFactura = Seguridad.Encriptar(item5.IdCabeceraFactura.ToString()),
                            Observacion = item5.Observacion,
                            FechaAsignacion = item5.FechaAsignacion
                        });
                    }
                    _ListaComunidadFactura.Add(new Comunidad()
                    {
                        IdComunidad = Seguridad.Encriptar(item4.ComunidadIdComunidad.ToString()),
                        Descripcion = item4.ComunidadDescripcion,
                        ListaAsignarComunidadFactura = _ListaAsignarComunidadFactura,
                        Parroquia = new Parroquia()
                        {
                            IdParroquia = Seguridad.Encriptar(item4.ParroquiaIdParroquia.ToString()),
                            Descripcion = item4.ParroquiaDescripcion,
                            Canton = new Canton()
                            {
                                IdCanton = Seguridad.Encriptar(item4.CantonIdCanton.ToString()),
                                Descripcion = item4.CantonDescripcion,
                                Provincia = new Provincia()
                                {
                                    IdProvincia = Seguridad.Encriptar(item4.ProvinciaIdProvincia.ToString()),
                                    Descripcion = item4.ProvinciaDescripcion
                                }
                            }
                        }
                    });
                }
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    ListaComunidad = _ListaComunidadFactura,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasEnFacturasParaSeguimientoPorPersona(int _idPersona)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasParaRealizarSeguimientoPorPersona(_idPersona))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                List<Comunidad> _ListaComunidadFactura = new List<Comunidad>();
                foreach (var item4 in ConexionBD.sp_ConsultarComunidadAsignadaAFacturaDeUnaPersona(item.PersonaIdPersona))
                {
                    List<AsignarComunidadFactura> _ListaAsignarComunidadFactura = new List<AsignarComunidadFactura>();
                    foreach (var item5 in ConexionBD.sp_ConsultarAsignarComunidadFacturaPorPersonaYPorComunidad(item.PersonaIdPersona, item4.ComunidadIdComunidad))
                    {
                        _ListaAsignarComunidadFactura.Add(new AsignarComunidadFactura()
                        {
                            IdAsignarComunidadFactura = Seguridad.Encriptar(item5.IdAsignarComunidadFactura.ToString()),
                            IdComunidad = Seguridad.Encriptar(item5.IdComunidad.ToString()),
                            IdCabeceraFactura = Seguridad.Encriptar(item5.IdCabeceraFactura.ToString()),
                            Observacion = item5.Observacion,
                            FechaAsignacion = item5.FechaAsignacion
                        });
                    }
                    _ListaComunidadFactura.Add(new Comunidad()
                    {
                        IdComunidad = Seguridad.Encriptar(item4.ComunidadIdComunidad.ToString()),
                        Descripcion = item4.ComunidadDescripcion,
                        ListaAsignarComunidadFactura = _ListaAsignarComunidadFactura,
                        Parroquia = new Parroquia()
                        {
                            IdParroquia = Seguridad.Encriptar(item4.ParroquiaIdParroquia.ToString()),
                            Descripcion = item4.ParroquiaDescripcion,
                            Canton = new Canton()
                            {
                                IdCanton = Seguridad.Encriptar(item4.CantonIdCanton.ToString()),
                                Descripcion = item4.CantonDescripcion,
                                Provincia = new Provincia()
                                {
                                    IdProvincia = Seguridad.Encriptar(item4.ProvinciaIdProvincia.ToString()),
                                    Descripcion = item4.ProvinciaDescripcion
                                }
                            }
                        }
                    });
                }
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    ListaComunidad = _ListaComunidadFactura,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasEnFacturasParaSeguimientoPorCanton(int _idCanton)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasParaSeguimientoPorCanton(_idCanton))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasEnFacturasParaSeguimientoPorParroquia(int _idParroquia)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasParaSeguimientoPorParroquia(_idParroquia))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasParaSeguimientoPorComunidad(int _idComunidad)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasParaSeguimientoPorComunidad(_idComunidad))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasParaSeguimientoPorProvincia(int _idProvincia)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasParaSeguimientoPorProvincia(_idProvincia))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasAsignadasPorTecnico(int _idTecnico)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasAsignadasAunTecnico(_idTecnico))
            {
                bool PermitirEliminar = false;
                if (item.NumeroVisita == 0)
                {
                    PermitirEliminar = true;
                }
                else
                {
                    PermitirEliminar = false;
                }
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    EstadoAsignacionTipoUsuario = PermitirEliminar,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaEntidad> ConsultarPersonasAsignadasPorTecnicoConSuscomunidades(int _idTecnico)
        {
            List<PersonaEntidad> Personas = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasAsignadasAunTecnico(_idTecnico))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                List<AsignarTecnicoPersonaComunidad> ListaComunidadesAsignadas = new List<AsignarTecnicoPersonaComunidad>();
                foreach (var item4 in ConexionBD.sp_ConsultarComunidadesDeUnaPersonaAsignadoAUnTecnico(item.PersonaIdPersona, _idTecnico))
                {
                    ListaComunidadesAsignadas.Add(new AsignarTecnicoPersonaComunidad()
                    {
                        NumeroVisita = item4.NumeroVisitas,
                        IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item4.AsignarTecnicoPersonaComunidadIdAsignarTecnicoPersonaComunidad
                        .ToString()),
                        IdAsignarTUTecnico = Seguridad.Encriptar(item4.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico.ToString()),
                        IdPersona = Seguridad.Encriptar(item4.AsignarTecnicoPersonaComunidadIdPersona.ToString()),
                        Comunidad = new Comunidad()
                        {
                            IdComunidad = Seguridad.Encriptar(item4.ComunidadIdComunidad.ToString()),
                            Descripcion = item4.ComunidadDescripcion,
                            Parroquia = new Parroquia()
                            {
                                IdParroquia = Seguridad.Encriptar(item4.ParroquiaIdParroquia.ToString()),
                                Descripcion = item4.ParroquiaDescripcion,
                                Canton = new Canton()
                                {
                                    IdCanton = Seguridad.Encriptar(item4.CantonIdCanton.ToString()),
                                    Descripcion = item4.CantonDescripcion,
                                    Provincia = new Provincia()
                                    {
                                        IdProvincia = Seguridad.Encriptar(item4.ProvinciaIdProvincia.ToString()),
                                        Descripcion = item4.ProvinciaDescripcion
                                    }
                                }
                            }
                        }
                    });
                }
                Personas.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    EstadoUsuario = item.PersonaEstado,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    _AsignarTecnicoPersonaComunidad = ListaComunidadesAsignadas,
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
        public List<PersonaSeguimientoFinalizado> ConsultarPersonasSeguimientoFinalizadoPorTecnico(int idAsignacionTU)
        {
            List<PersonaSeguimientoFinalizado> Personas = new List<PersonaSeguimientoFinalizado>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasSeguimientoFinalizadoPorTecnico(idAsignacionTU))
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.PersonaIdPersona))
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
                List<Correo> ListaCorreos = new List<Correo>();
                foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.PersonaIdPersona))
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
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.PersonaIdPersona))
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
                List<Comunidad> Comunidades = new List<Comunidad>();
                foreach (var item4 in ConexionBD.sp_ConsultarComunidadesSeguimientoFinalizadoPorTecnicoYPersona(item.PersonaIdPersona, idAsignacionTU))
                {
                    List<AsignarTecnicoPersonaComunidad> ListaTecnicoPersonaComunidad = new List<AsignarTecnicoPersonaComunidad>();
                    foreach (var item5 in ConexionBD.sp_ConsultarHistorialAsistenciaSeguimientoFinalizadoPorTecnicoYPersona(item.PersonaIdPersona,idAsignacionTU,item4.ComunidadIdComunidad))
                    {
                        ListaTecnicoPersonaComunidad.Add(new AsignarTecnicoPersonaComunidad()
                        {
                            IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item5.IdAsignarTecnicoPersonaComunidad.ToString()),
                            IdAsignarTUTecnico = Seguridad.Encriptar(item5.IdAsignarTUTecnico.ToString()),
                            IdPersona = Seguridad.Encriptar(item5.IdPersona.ToString()),
                            IdComunidad = Seguridad.Encriptar(item5.IdComunidad.ToString()),
                            Estado = item5.Estado,
                            FechaAsignacion = item5.FechaAsignacion,
                            FechaFinalizacion = item5.FechaFinalizacion
                        });
                    }
                    Comunidades.Add(new Comunidad()
                    {
                        IdComunidad = Seguridad.Encriptar(item4.ComunidadIdComunidad.ToString()),
                        Descripcion = item4.ComunidadDescripcion,
                        FechaCreacion = item4.ComunidadFechaCreacion,
                        AsignarTecnicoPersonaComunidad = ListaTecnicoPersonaComunidad
                    });
                }

                Personas.Add(new PersonaSeguimientoFinalizado()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ListaComunidad = Comunidades,
                    ListaTelefono = ListaTelefonos,
                    Correo = ListaCorreos.FirstOrDefault(),
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia.FirstOrDefault(),
                    _objTipoDocumento = new TipoDocumento()
                    {
                        IdTipoDocumento = Seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion
                    }
                });
            }
            return Personas;
        }
    }
}
