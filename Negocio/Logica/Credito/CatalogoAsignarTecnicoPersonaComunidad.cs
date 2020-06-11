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
    public class CatalogoAsignarTecnicoPersonaComunidad
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoAsignarComunidadFactura _CatalogoAsignarComunidadFactura = new CatalogoAsignarComunidadFactura();
        public AsignarTecnicoPersonaComunidad InsertarAsignarTecnicoPersonaComunidad(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            foreach (var item in ConexionBD.sp_CrearAsignarTecnicoPersonaComunidad(int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico),int.Parse(_AsignarTecnicoPersonaComunidad.IdPersona),int.Parse(_AsignarTecnicoPersonaComunidad.IdComunidad)))
            {
                _AsignarTecnicoPersonaComunidad.IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString());
                _AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString());
                _AsignarTecnicoPersonaComunidad.IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString());
                _AsignarTecnicoPersonaComunidad.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                _AsignarTecnicoPersonaComunidad.Estado = item.Estado;
                _AsignarTecnicoPersonaComunidad.FechaAsignacion = item.FechaAsignacion;
            }
            return _AsignarTecnicoPersonaComunidad;
        }
        public bool ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura(int _idPersona, int _idTecnico)
        {
            try
            {
                AsignarTecnicoPersonaComunidad _Dato = new AsignarTecnicoPersonaComunidad();
                PersonaEntidad Persona = new PersonaEntidad();
                _Dato = ConsultarAsignarTecnicoPersonaComunidadPorPersona(_idPersona).FirstOrDefault();
                if (_Dato == null)
                {
                    // si en esta instancia es igual a null es porque la persona no se le a asignado ningun tecnico por lo que el administradord el sistema deberea hacerlo
                    if (_idTecnico != 0)
                    {
                        Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                        if (Persona != null)
                    {
                        foreach (var item in Persona.ListaComunidad)
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                            _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                            //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                            if (_DatoAsginarTecnicoPersona == null)
                            {
                                //Asignarle A la persona Con el tecnico que ya Asignar el administrador
                                AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                _DatoAIngresar.IdAsignarTUTecnico = _idTecnico.ToString();
                                _DatoAIngresar.IdPersona = Seguridad.DesEncriptar(_Dato.IdPersona);
                                InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                            }
                        }
                    }
                    }
                }
                else
                {
                    Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                    if (Persona != null)
                    {
                        foreach (var item in Persona.ListaComunidad)
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                            _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                            //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                            if (_DatoAsginarTecnicoPersona == null)
                            {
                                //Asignarle A la persona Con el tecnico que ya tiene
                                AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                _DatoAIngresar.IdAsignarTUTecnico = Seguridad.DesEncriptar(_Dato.IdAsignarTUTecnico);
                                _DatoAIngresar.IdPersona = Seguridad.DesEncriptar(_Dato.IdPersona);
                                InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura1(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            try
            {
                int _idPersona = int.Parse(_AsignarTecnicoPersonaComunidad.IdPersona);
                int _idTecnicos = int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico);
                AsignarTecnicoPersonaComunidad _Dato = new AsignarTecnicoPersonaComunidad();
                PersonaEntidad Persona = new PersonaEntidad();
                _Dato = ConsultarAsignarTecnicoPersonaComunidadPorPersona(_idPersona).FirstOrDefault();
                if (_Dato == null)
                {
                    // si en esta instancia es igual a null es porque la persona no se le a asignado ningun tecnico por lo que el administradord el sistema deberea hacerlo
                    if (_idTecnicos != 0)
                    {
                        Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                        if (Persona != null)
                        {
                            foreach (var item in Persona.ListaComunidad)
                            {
                                AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                                _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                                //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                                if (_DatoAsginarTecnicoPersona == null)
                                {
                                    //Asignarle A la persona Con el tecnico que ya Asignar el administrador
                                    AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                    _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                    _DatoAIngresar.IdAsignarTUTecnico = _idTecnicos.ToString();
                                    _DatoAIngresar.IdPersona = _idPersona.ToString();
                                    InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                    if (Persona != null)
                    {
                        foreach (var item in Persona.ListaComunidad)
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                            _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                            //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                            if (_DatoAsginarTecnicoPersona == null)
                            {
                                //Asignarle A la persona Con el tecnico que ya tiene
                                AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                _DatoAIngresar.IdAsignarTUTecnico = Seguridad.DesEncriptar(_Dato.IdAsignarTUTecnico);
                                _DatoAIngresar.IdPersona = Seguridad.DesEncriptar(_Dato.IdPersona);
                                InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarAsignarTecnicoPersonaComunidadPorPersona(int _idPersona)
        {
            List<AsignarTecnicoPersonaComunidad> ListaAsignarTecnico = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarTecnicoPersonaComunidadPorPersona(_idPersona))
            {
                ListaAsignarTecnico.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    Estado = item.Estado,
                    FechaAsignacion = item.FechaAsignacion,
                });
            }
            return ListaAsignarTecnico;
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(int _idPersona,int _IdComunidad)
        {
            List<AsignarTecnicoPersonaComunidad> ListaAsignarTecnico = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, _IdComunidad))
            {
                ListaAsignarTecnico.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    Estado = item.Estado,
                    FechaAsignacion = item.FechaAsignacion,
                });
            }
            return ListaAsignarTecnico;
        }
        public bool EliminarPersonaDeUnTecnico(int _idPersona)
        {
            try
            {
                ConexionBD.sp_EliminarAsignacionPersonaDeUnTecnico(_idPersona);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<TipoUsuario> ConsultarTipoUsuarioTecncioDeUnaPersona(int IdAsignacionTU)
        {
            List<TipoUsuario> TipoUsuario = new List<TipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarSiUsuarioEsTecnico(IdAsignacionTU))
            {
                TipoUsuario.Add(new Entidades.TipoUsuario()
                {
                    IdTipoUsuario = Seguridad.Encriptar(item.IdTipoUsuario.ToString()),
                    Identificacion = item.Identificacion,
                    Estado = item.Estado,
                });
            }
            return TipoUsuario;
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarPorEstadoAsignarTPC(int _idAsignarTPC)
        {
            List<AsignarTecnicoPersonaComunidad> _lista = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarTecnicoPersonaComunidadPorEstado(_idAsignarTPC,"1"))
            {
                _lista.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    Estado = item.Estado,
                    FechaAsignacion = item.FechaAsignacion,
                });
            }
            return _lista;
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarPersonasAsignadasAunTecnicoPorComunidad(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            List<AsignarTecnicoPersonaComunidad> ListaPersonasPorComunidad = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasAsignadasAunTecnicoPorComunidad(int.Parse(_AsignarTecnicoPersonaComunidad.IdComunidad), int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico)))
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
                ListaPersonasPorComunidad.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.AsignarTecnicoPersonaComunidadIdAsignarTecnicoPersonaComunidad.ToString()),
                    Persona = new PersonaEntidad()
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
                    }
                });
            }
            return ListaPersonasPorComunidad;
        }
        public List<ProvinciaSeguimientoPersonas> ConsultarPersonasOrdenadasPorLugarPorTecnico(int _idAsignarTecnico)
        {
            List<ProvinciaSeguimientoPersonas> ListaPersona = new List<ProvinciaSeguimientoPersonas>();
            foreach (var item in ConexionBD.sp_ConsultarProvinciasParaSeguimientoPorTecnico(_idAsignarTecnico))
            {
                List<CantonPersonas> ListaCantones = new List<CantonPersonas>();
                foreach (var item1 in ConexionBD.sp_ConsultarCantonesParaSeguimientoPorTecnico(_idAsignarTecnico,item.IdProvincia))
                {
                    List<ParroquiaPersonas> ListaParroquias = new List<ParroquiaPersonas>();
                    foreach (var item2 in ConexionBD.sp_ConsultarParroquiaParaSeguimientoPorTecnico(_idAsignarTecnico,item1.IdCanton))
                    {
                        List<ComunidadesPersonas> ListaComunidades = new List<ComunidadesPersonas>();
                        foreach (var item3 in ConexionBD.sp_ConsultarComunidadesParaSeguimientoPorTecnico(_idAsignarTecnico,item2.IdParroquia))
                        {
                            List<AsignarTecnicoPersonaComunidad> ListaPersonasPorComunidad = new List<AsignarTecnicoPersonaComunidad>();
                            foreach (var item4 in ConexionBD.sp_ConsultarPersonasDeUnaComunidadPorTecnico(_idAsignarTecnico, item3.IdComunidad))
                            {
                                List<Telefono> ListaTelefonos = new List<Telefono>();
                                foreach (var item5 in ConexionBD.sp_ConsultarTelefonoPersona(item4.PersonaIdPersona))
                                {
                                    ListaTelefonos.Add(new Telefono()
                                    {
                                        IdTelefono = Seguridad.Encriptar(item5.IdTelefono.ToString()),
                                        IdPersona = Seguridad.Encriptar(item5.IdPersona.ToString()),
                                        Numero = item5.Numero,
                                        TipoTelefono = new TipoTelefono()
                                        {
                                            IdTipoTelefono = Seguridad.Encriptar(item5.IdTipoTelefono.ToString()),
                                            Descripcion = item5.Descripcion,
                                            Identificador = item5.Identificador,
                                            FechaCreacion = item5.TipoTelefonoFechaCreacion,
                                            Estado = item5.TipoTelefonoEstado,
                                        },
                                        FechaCreacion = item5.FechaCreacion,
                                        Estado = item5.Estado,
                                    });
                                }
                                List<Correo> ListaCorreos = new List<Correo>();
                                foreach (var item6 in ConexionBD.sp_ConsultarCorreoPersona(item4.PersonaIdPersona))
                                {
                                    ListaCorreos.Add(new Correo()
                                    {
                                        IdCorreo = Seguridad.Encriptar(item6.IdCorreo.ToString()),
                                        IdPersona = Seguridad.Encriptar(item6.IdPersona.ToString()),
                                        CorreoValor = item6.Correo,
                                        FechaCreacion = item6.FechaCreacion,
                                        Estado = item6.Estado,
                                    });
                                }
                                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                                foreach (var item7 in ConexionBD.sp_ConsultarResidenciaPersona(item4.PersonaIdPersona))
                                {
                                    ListaAsignacionPersonaParroquia.Add(new AsignacionPersonaParroquia()
                                    {
                                        Referencia = item7.AsignacionPersonaParroquiaReferencia,
                                        IdPersona = Seguridad.Encriptar(item7.AsignacionPersonaComunidadIdPersona.ToString()),
                                        IdAsignacionPC = Seguridad.Encriptar(item7.AsignacionPersonaParroquiaIdAsignacionPersonaParroquia.ToString()),
                                        FechaCreacion = item7.AsignacionPersonaParroquiaFechaCreacion,
                                        Estado = item7.AsignacionPersonaParroquiaEstado,
                                        Parroquia = new Parroquia()
                                        {
                                            IdParroquia = Seguridad.Encriptar(item7.ParroquiaIdParroquia.ToString()),
                                            Descripcion = item7.ParroquiaDescripcion,
                                            FechaCreacion = item7.ParroquiaFechaCreacion,
                                            Estado = item7.ParroquiaEstado,
                                            Canton = new Canton()
                                            {
                                                IdCanton = Seguridad.Encriptar(item7.CantonIdCanton.ToString()),
                                                Descripcion = item7.CantonDescripcion,
                                                FechaCreacion = item7.CantonFechaCreacion,
                                                Estado = item7.CantonEstado,
                                                Provincia = new Provincia()
                                                {
                                                    IdProvincia = Seguridad.Encriptar(item7.ProvinciaIdProvincia.ToString()),
                                                    Descripcion = item7.ProvinciaDescripcion,
                                                    FechaCreacion = item7.ProvinciaFechaCreacion,
                                                    Estado = item7.ProvinciaEstado,
                                                },
                                            },
                                        },
                                    });
                                }
                                ListaAsignacionPersonaParroquia = ListaAsignacionPersonaParroquia.GroupBy(a => a.IdAsignacionPC).Select(grp => grp.First()).ToList();
                                ListaPersonasPorComunidad.Add(new AsignarTecnicoPersonaComunidad()
                                {
                                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item4.AsignarTecnicoPersonaComunidadIdAsignarTecnicoPersonaComunidad.ToString()),
                                    IdComunidad = Seguridad.Encriptar(item4.AsignarTecnicoPersonaComunidadIdComunidad.ToString()),
                                    FechaAsignacion = item4.AsignarTecnicoPersonaComunidadFechaAsignacion,
                                    IdAsignarTUTecnico = Seguridad.Encriptar(item4.AsignarTecnicoPersonaComunidadIdAsignarTUTecnico.ToString()),
                                    Estado = item4.AsignarTecnicoPersonaComunidadEstado,     
                                    NumeroVisita = item4.NumeroVisita,
                                    Persona = new PersonaEntidad()
                                    {
                                        IdPersona = Seguridad.Encriptar(item4.PersonaIdPersona.ToString()),
                                        PrimerNombre = item4.PersonaPrimerNombre,
                                        SegundoNombre = item4.PersonaSegundoNombre,
                                        ApellidoMaterno = item4.PersonaApellidoMaterno,
                                        ApellidoPaterno = item4.PersonaApellidoPaterno,
                                        EstadoUsuario = item4.PersonaEstado,
                                        NumeroDocumento = item4.PersonaNumeroDocumento,
                                        ListaTelefono = ListaTelefonos,
                                        ListaCorreo = ListaCorreos,
                                        AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                                        _objTipoDocumento = new TipoDocumento()
                                        {
                                            IdTipoDocumento = Seguridad.Encriptar(item4.TipoDocumentoIdTipoDocumento.ToString()),
                                            Documento = item4.TipoDocumentoDescripcion
                                        }
                                    }
                                });
                            }
                            ListaComunidades.Add(new ComunidadesPersonas()
                            {
                                IdComunidad = Seguridad.Encriptar(item3.IdComunidad.ToString()),
                                Descripcion = item3.Descripcion,
                                PersonaEntidad = ListaPersonasPorComunidad
                            });
                        }
                        ListaParroquias.Add(new ParroquiaPersonas()
                        {
                            IdParroquia = Seguridad.Encriptar(item2.IdParroquia.ToString()),
                            Descripcion = item2.Descripcion,
                            ComunidadesPersonas = ListaComunidades
                        });
                    }
                    ListaCantones.Add(new CantonPersonas()
                    {
                        IdCanton = Seguridad.Encriptar(item1.IdCanton.ToString()),
                        Descripcion = item1.Descripcion,
                        ParroquiaPersonas = ListaParroquias
                    });
                }
                ListaPersona.Add(new ProvinciaSeguimientoPersonas()
                {
                    IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString()),
                    Descripcion = item.Descripcion,
                    CantonPersonas = ListaCantones
                });
            }
            return ListaPersona;
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarAsginarTecnicoPersonaComunidadPorId(int _id)
        {
            List<AsignarTecnicoPersonaComunidad> Lista = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarTecnicoPersonaComunidadPorId(_id))
            {
                Lista.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    Estado = item.Estado,
                    IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString()),
                    FechaAsignacion = item.FechaAsignacion,
                });
            }
            return Lista;
        }
        public bool FinalizarSeguimiento(int id)
        {
            try
            {
                ConexionBD.sp_FinalizarAsignarTecnicoPersonaComunidad(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TransferirTecnico(TrasnferirTecnico _TrasnferirTecnico)
        {
            try
            {
                ConexionBD.sp_TrasnferirPersonasAOtroTecnico(int.Parse(_TrasnferirTecnico.IdAsignarTUAntiguo), int.Parse(_TrasnferirTecnico.IdAsignarTUNuevo));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
