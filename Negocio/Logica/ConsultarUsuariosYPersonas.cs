using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Datos;
using Negocio.Logica.Seguridad;

namespace Negocio.Logica
{
    public class ConsultarUsuariosYPersonas
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Metodos.Seguridad seguridad = new Metodos.Seguridad();



        public List<Usuario> ListaUsuarios;
        public List<Persona> ListaPersona;
        public List<PersonaEntidad> ListaClientes;
        public UsuariosSistema LoginSistema(Login Login)
        {
            UsuariosSistema ListaUsuariosLogin = ObtenerListaUsuarios().Where(p => p.UsuarioLogin == Login.usuario).FirstOrDefault();
            if (ListaUsuariosLogin !=null)
            {
                string contrasena = p.desencriptar(ListaUsuariosLogin.Contrasena, "Contrasena");
                if (Login.contrasena == contrasena)
                {
                    if (ListaUsuariosLogin.ListaTipoUsuario.Count() == 0)
                    {
                        ListaUsuariosLogin = null;
                        return ListaUsuariosLogin;
                    }
                    else
                    {
                        return ListaUsuariosLogin;
                    }
                }
                else
                {
                    ListaUsuariosLogin = null;
                    return ListaUsuariosLogin;
                }
            }
            else
            {
                ListaUsuariosLogin = null;
                return ListaUsuariosLogin;
            }
        }
        public void CargarDatosUsuarios()
        {
            ListaUsuarios = new List<Usuario>();
            //foreach (var item in ConexionBD.sp_ConsultarUsuarios().Where(p=>p.AsignacionTipoUsuarioEstado==true).ToList())
            foreach (var item in ConexionBD.sp_ConsultarUsuarios())
            {
                ListaUsuarios.Add(new Usuario()
                {
                    IdUsuario = seguridad.Encriptar(item.UsuarioIdUsuario.ToString()),
                    UsuarioLogin = item.Usuario,
                    Contrasena = item.Contrasena,
                    FechaCreacion = item.UsuarioFechaCreacion,
                    Estado = item.UsuarioEstado,
                    Persona = new Persona()
                    {
                        IdPersona = item.PersonaIdPersona,
                        NumeroDocumento = item.PersonaNumeroDocumento,
                        ApellidoPaterno = item.PersonaApellidoPaterno,
                        ApellidoMaterno = item.PersonaApellidoMaterno,
                        PrimerNombre = item.PersonaPrimerNombre,
                        SegundoNombre = item.PersonaSegundoNombre,
                        FechaCreacion = item.PersonaFechaCreacion,
                        Estado = item.PersonaEstado,
                        Telefono = null,
                        AsignacionTipoUsuario = new AsignacionTipoUsuario()
                        {
                            IdAsignacionTU = item.IdAsignacionTU,
                            IdUsuario = item.AsigancionTipoUsuarioIdTipoUsuario,
                            FechaCreacion = item.AsignacionTipoUsuarioFechaCreacion,
                            Estado = item.AsignacionTipoUsuarioEstado,
                            TipoUsuario = new TipoUsuario()
                            {
                                IdTipoUsuario = item.TipoUsuarioIdTipoUsuario.ToString(),
                                Descripcion = item.TipoUsuarioDescripcion,
                                Identificacion = item.TipoUsuarioIdentificacion,
                                FechaCreacion = item.TipoUsuarioFechaCreacion,
                                Estado = item.AsignacionTipoUsuarioEstado,
                            },
                        },
                        ModuloTipo = new ModuloTipo()
                        {
                            IdModuloTipo = item.IdModuloTipo,
                            IdTipoUsuario = item.ModuloTipoIdTipoUsuario,
                            FechaCreacion = item.ModuloTipoFechaCreacion,
                            Estado = item.ModuloTipoEstado,
                            Modulo = new Modulo()
                            {
                                IdModulo = seguridad.Encriptar(item.IdModulo.ToString()),
                                Descripcion = item.ModuloDescripcion,
                                Controlador = item.Controlador,
                                Metodo = item.Metodo,
                                Identificador = item.ModuloIdentificador,
                                FechaCreacion = item.ModuloFechaCreacion,
                                Estado = item.ModuloEstado,
                            },
                        },
                        PrivilegioModuloTipo = new PrivilegioModuloTipo()
                        {
                            IdPrivilegioModuloTipo = item.IdPrivilegioModuloTipo,
                            IdModuloTipo = item.PrivilegioModuloTipoIdModuloTipo,
                            FechaCreacion = item.PrivilegioModuloTipoFechaCreacion,
                            Estado = item.PrivilegioModuloTipoEstado,
                            Privilegio = new Privilegios()
                            {
                                IdPrivilegios = item.IdPrivilegios.ToString(),
                                Descripcion = item.PrivilegiosDescripcion,
                                Identificador = item.PrivilegiosIdentificador,
                                FechaCreacion = item.PrivilegioFechaCreacion,
                                Estado = item.PrivilegioEstado,
                            },
                        },
                    },
                });
            }
        }
        /*public void CargarDatosClientes()
        {
            ListaPersona = new List<Persona>();
            foreach (var item in ConexionBD.sp_ConsultarPersonas().Where(p=>p.ComunidadEstado==true))
            {
                ListaPersona.Add(new Persona()
                {
                    IdPersona = item.TelefonoIdPersona,
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    FechaCreacion = item.PersonaFechaCreacion,
                    Estado = item.PersonaEstado,
                    Telefono = new Telefono()
                    {
                        IdTelefono = seguridad.Encriptar(item.TelefonoIdTelefono.ToString()),
                        IdPersona = seguridad.Encriptar(item.TelefonoIdPersona.ToString()),
                        Numero = item.TelefonoNumero,
                        FechaCreacion = item.TelefonoFechaCreacion,
                        Estado = item.TelefonoEstado,
                        TipoTelefono = new TipoTelefono()
                        {
                            IdTipoTelefono = seguridad.Encriptar(item.TipoTelefonoIdTipoTelefono.ToString()),
                            Descripcion = item.TipoTelefonoDescripcion,
                            Identificador = item.TipoTelefonoIdentificador,
                            FechaCreacion = item.TipoTelefonoFechaCreacion,
                            Estado = item.TipoTelefonoEstado,
                        },
                    },
                    Correo = new Correo()
                    {
                        IdCorreo = seguridad.Encriptar(item.CorreoIdCorreo),
                        IdPersona = item.CorreoIdPersona,
                        CorreoValor = item.CorreoCorreo,
                        FechaCreacion = item.CorreoFechaCreacion,
                        Estado = item.CorreoEstado,
                    },
                    TipoDocumentos = new TipoDocumento()
                    {
                        IdTipoDocumento = seguridad.Encriptar(item.TipoDocumentoIdTipoDocumento.ToString()),
                        Documento = item.TipoDocumentoDescripcion,
                        Identificador = item.TipoDocumentoIdentificador,
                        FechaCreacion = item.TipoDocumentoFechaCreacion,
                        Estado = item.TipoDocumentoEstado,
                    },
                    AsigancionPersonaComunidad = new AsignacionPersonaComunidad()
                    {
                        IdAsignacionPC = item.AsignacionPersonaComunidadIdAsignacionPC,
                        IdPersona = item.AsignacionPersonaComunidadIdPersona,
                        FechaCreacion = item.AsignacionPersonaComunidadFechaCreacion,
                        Estado = item.AsignacionPersonaComunidadEstado,
                        Comunidad = new Comunidad()
                        {
                            IdComunidad = item.ComunidadIdComunidad,
                            Descripcion = item.ComunidadDescripcion,
                            FechaCreacion = item.ComunidadFechaCreacion,
                            Estado = item.ComunidadEstado,
                            Parroquia = new Parroquia()
                            {
                                IdParroquia = item.ParroquiaIdParroquia,
                                Descripcion = item.ParroquiaDescripcion,
                                FechaCreacion = item.ParroquiaFechaCreacion,
                                Estado = item.ParroquiaEstado,
                                Canton = new Canton()
                                {
                                    IdCanton = item.CantonIdCanton,
                                    Descripcion = item.CantonDescripcion,
                                    FechaCreacion = item.CantonFechaCreacion,
                                    Estado = item.CantonEstado,
                                    Provincia = new Provincia()
                                    {
                                        IdProvincia = item.ProvinciaIdProvincia,
                                        Descripcion = item.ProvinciaDescripcion,
                                        FechaCreacion = item.ProvinciaFechaCreacion,
                                        Estado = item.ProvinciaEstado,
                                    }
                                }
                            },
                        },
                    },
                });

            }
        }*/
        public void CargarPersonas()
        {
            ListaClientes = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarListaPersonas())
            {
                List<Telefono> ListaTelefonos = new List<Telefono>();
                List<Correo> ListaCorreos = new List<Correo>();
                List<AsignacionPersonaParroquia> ListaAsignacionPersonaParroquia = new List<AsignacionPersonaParroquia>();
                if (ConexionBD.sp_ConsultarTelefonoPersona(item.IdPersona).Count()>0)
                {
                    foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(item.IdPersona))
                    {
                        ListaTelefonos.Add(new Telefono()
                        {
                            IdTelefono = seguridad.Encriptar(item1.IdTelefono.ToString()),
                            IdPersona = seguridad.Encriptar(item1.IdPersona.ToString()),
                            Numero = item1.Numero,
                            TipoTelefono = new TipoTelefono()
                            {
                                IdTipoTelefono = seguridad.Encriptar(item1.IdTipoTelefono.ToString()),
                                Descripcion = item1.Descripcion,
                                Identificador = item1.Identificador,
                                FechaCreacion = item1.TipoTelefonoFechaCreacion,
                                Estado = item1.TipoTelefonoEstado,
                            },
                            FechaCreacion = item1.FechaCreacion,
                            Estado = item1.Estado,

                        });
                    }
                }
                else
                {
                    ListaTelefonos = null;
                }
                if (ConexionBD.sp_ConsultarCorreoPersona(item.IdPersona).Count() > 0)
                {
                    foreach (var item2 in ConexionBD.sp_ConsultarCorreoPersona(item.IdPersona))
                    {
                        ListaCorreos.Add(new Correo()
                        {
                            IdCorreo = seguridad.Encriptar(item2.IdCorreo.ToString()),
                            IdPersona = seguridad.Encriptar(item2.IdPersona.ToString()),
                            CorreoValor = item2.Correo,
                            FechaCreacion = item2.FechaCreacion,
                            Estado = item2.Estado,
                        });
                    }
                }
                else
                {
                    ListaCorreos = null;
                }
                if (ConexionBD.sp_ConsultarResidenciaPersona(item.IdPersona).Count() > 0)
                {
                    foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.IdPersona))
                    {
                        ListaAsignacionPersonaParroquia.Add(new AsignacionPersonaParroquia()
                        {
                            IdPersona = seguridad.Encriptar(item3.AsignacionPersonaComunidadIdPersona.ToString()),
                            IdAsignacionPC = seguridad.Encriptar(item3.AsignacionPersonaParroquiaIdAsignacionPersonaParroquia.ToString()),
                            FechaCreacion = item3.AsignacionPersonaParroquiaFechaCreacion,
                            Estado = item3.AsignacionPersonaParroquiaEstado,
                            Parroquia = new Parroquia()
                            {
                                    IdParroquia = seguridad.Encriptar(item3.ParroquiaIdParroquia.ToString()),
                                    Descripcion = item3.ParroquiaDescripcion,
                                    FechaCreacion = item3.ParroquiaFechaCreacion,
                                    Estado = item3.ParroquiaEstado,
                                    Canton = new Canton()
                                    {
                                        IdCanton = seguridad.Encriptar(item3.CantonIdCanton.ToString()),
                                        Descripcion = item3.CantonDescripcion,
                                        FechaCreacion = item3.CantonFechaCreacion,
                                        Estado = item3.CantonEstado,
                                        Provincia = new Provincia()
                                        {
                                            IdProvincia = seguridad.Encriptar(item3.ProvinciaIdProvincia.ToString()),
                                            Descripcion = item3.ProvinciaDescripcion,
                                            FechaCreacion = item3.ProvinciaFechaCreacion,
                                            Estado = item3.ProvinciaEstado,
                                        },
                                    },
                                },
                    });
                        
                    }
                    ListaAsignacionPersonaParroquia=ListaAsignacionPersonaParroquia.GroupBy(a => a.IdAsignacionPC).Select(grp => grp.First()).ToList();
                }
                else
                {
                    ListaAsignacionPersonaParroquia = null;
                }
                ListaClientes.Add(new PersonaEntidad()
                {
                    IdPersona = seguridad.Encriptar(item.IdPersona.ToString()),
                    NumeroDocumento = item.NumeroDocumento,
                    ApellidoPaterno = item.ApellidoPaterno,
                    ApellidoMaterno = item.ApellidoMaterno,
                    PrimerNombre = item.PrimerNombre,
                    SegundoNombre = item.SegundoNombre,
                    IdTipoDocumento = seguridad.Encriptar(item.IdTipoDocumento.ToString()),
                    TipoDocumento = item.TipoDocumento,
                    IdUsuario = seguridad.Encriptar(item.IdUsuario.ToString()),
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    EstadoUsuario= item.EstadoUsuario,
                    EstadoAsignacionTipoUsuario = item.AsignacionTipoUsuarioEstado,
                    IdAsignacionTipoUsuario = seguridad.Encriptar(item.IdAsignacionTU.ToString()),
                });
            }
        }
        public bool UsuarioExistente(string Usuario)
        {
            if (ConexionBD.sp_ConsultarTodosLosUsuarios().ToList().Where(p => p.Usuario.Trim() == Usuario.Trim()).Count()>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<UsuariosSistema> ObtenerListaUsuarios()
        {
            CargarDatosUsuarios();
            List<UsuariosSistema> ListaDatos = new List<UsuariosSistema>();
            var Lista = ListaUsuarios.GroupBy(p => p.UsuarioLogin).Select(grp => grp.ToList());
            foreach (var item in Lista)
            {
                List<Modulo> Modulo = new List<Entidades.Modulo>();
                List<Privilegios> Privilegios = new List<Entidades.Privilegios>();
                List<TipoUsuario> ListaTipoUsuario = new List<TipoUsuario>();
                for (int i = 0; i < item.Count; i++)
                {
                    Privilegios.Add(new Entidades.Privilegios()
                    {
                        IdPrivilegios = seguridad.Encriptar(item[i].Persona.PrivilegioModuloTipo.Privilegio.IdPrivilegios.ToString()),
                        Descripcion = item[i].Persona.PrivilegioModuloTipo.Privilegio.Descripcion,
                        Identificador = item[i].Persona.PrivilegioModuloTipo.Privilegio.Identificador,
                        FechaCreacion = item[i].Persona.PrivilegioModuloTipo.Privilegio.FechaCreacion,
                        Estado = item[i].Persona.PrivilegioModuloTipo.Privilegio.Estado,
                    });
                }
                foreach (var item2 in ConexionBD.sp_BuscarModulos(int.Parse(item[0].Persona.AsignacionTipoUsuario.TipoUsuario.IdTipoUsuario)))
                {
                    Modulo.Add(new Entidades.Modulo()
                    {
                        IdModulo = seguridad.Encriptar(item2.ModuloIdModulo.ToString()),
                        Descripcion = item2.ModuloDescripcion,
                        Controlador = item2.ModuloControlador,
                        Metodo = item2.ModuloMetodo,
                        Identificador = item2.ModuloIdentificador,
                        FechaCreacion = item2.FechaCreacionModulo,
                        Estado = item2.EstadoModulo,
                    });
                }
                foreach (var item3 in ConexionBD.sp_ConsultarTiposUsuarioDeUnaPersona(int.Parse(seguridad.DesEncriptar(item[0].IdUsuario))))
                {
                    ListaTipoUsuario.Add(new TipoUsuario()
                    {
                        IdTipoUsuario = seguridad.Encriptar(item3.IdTipoUsuario.ToString()),
                        Descripcion = item3.Descripcion,
                        Identificacion = item3.IdentificacionTipoUsuario,
                        Estado = null,
                        IdAsignacionTu = seguridad.Encriptar(item3.IdAsignacionTU.ToString()),
                    });
                }
                ListaDatos.Add(new UsuariosSistema()
                {
                    IdPersona = seguridad.Encriptar(item[0].Persona.IdPersona.ToString()),
                    NumeroDocumento = item[0].Persona.NumeroDocumento,
                    ApellidoPaterno = item[0].Persona.ApellidoPaterno,
                    ApellidoMaterno = item[0].Persona.ApellidoMaterno,
                    PrimerNombre = item[0].Persona.PrimerNombre,
                    SegundoNombre = item[0].Persona.SegundoNombre,

                    IdUsuario = item[0].IdUsuario,
                    UsuarioLogin = item[0].UsuarioLogin,
                    Contrasena = item[0].Contrasena,
                    EstadoUsuario = item[0].Estado,

                    ListaTipoUsuario = ListaTipoUsuario,
                    Privilegios = Privilegios,
                    Modulo = Modulo

                });
            }
            return ListaDatos;
        }
        public List<PersonaEntidad> ObtenerUsuariosClientes()
        {
            CargarPersonas();
            return ListaClientes.GroupBy(a => a.IdPersona).Select(grp => grp.First()).ToList();
            //CargarDatosClientes();
            //return ListaPersona;
        }
        public List<PersonaEntidad> ObtenerUsuariosClientesInformacion()
        {
            CargarPersonas();
            List<PersonaEntidad> listaPersona = new List<PersonaEntidad>();
            listaPersona = ListaClientes.Where(p => p.IdUsuario == "").ToList();
            return listaPersona.GroupBy(a => a.IdPersona).Select(grp => grp.First()).ToList();
        }
        public PersonaEntidad FiltrarPersona(int IdPersona)
        {
            CargarPersonas();
            for (int i = 0; i < ListaClientes.Count; i++)
            {
                ListaClientes[i].IdPersona = seguridad.DesEncriptar(ListaClientes[i].IdPersona);
            }
            PersonaEntidad Persona = new PersonaEntidad();
            Persona=ListaClientes.Where(p => p.IdPersona == IdPersona.ToString()).FirstOrDefault();
            if (Persona != null)
            {
                Persona.IdPersona = seguridad.Encriptar(Persona.IdPersona);
            } else
            {
                Persona = null;
            }
            return Persona;
        }
        public Usuario ObtenerUsuarioLogin(string Usuario, string Contrasena)
        {
            
            CargarDatosUsuarios();
            Usuario ListaUsuariosLogin = ListaUsuarios.Where(p => p.UsuarioLogin == Usuario && p.Contrasena == Contrasena && p.Estado == true).First();
            return ListaUsuariosLogin;
        }
        public Usuario BuscarDatosPersonas(string NumeroDocumento)
        {
            CargarDatosUsuarios();
            return ListaUsuarios.Where(p => p.Persona.NumeroDocumento == NumeroDocumento).First();
        }
        public bool HabilitarUsuario(int IdUsuario)
        {
            try
            {
                ConexionBD.sp_HabilitarUsuario(IdUsuario);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            
        }
        public bool validacedula(string cedula)
        {
            int numv = 10;
            int div = 11;
            int[] coeficientes;
            if (int.Parse(cedula[2].ToString()) < 6) { coeficientes = new int[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 }; div = 10; }
            else
            {
                if (int.Parse(cedula[2].ToString()) == 6)
                {
                    coeficientes = new int[] { 3, 2, 7, 6, 5, 4, 3, 2 };
                    numv = 9;
                }
                else coeficientes = new int[] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            }
            int total = 0;
            int numprovincia = 24;
            int calculo = 0;
            cedula = cedula.Replace("-", "");
            char[] valores = cedula.ToCharArray(0, 9);

            if (((Convert.ToInt16(valores[2].ToString()) <= 6) || (Convert.ToInt16(valores[2].ToString()) == 9)) && (Convert.ToInt16(cedula.Substring(0, 2)) <= numprovincia))
            {
                for (int i = 0; i < numv - 1; i++)
                {
                    calculo = (Convert.ToInt16(valores[i].ToString())) * coeficientes[i];
                    if (div == 10) total += calculo > 9 ? calculo - 9 : calculo;
                    else total += calculo;
                }
                return (div - (total % div)) >= 10 ? 0 == Convert.ToInt16(cedula[numv - 1].ToString()) : (div - (total % div)) == Convert.ToInt16(cedula[numv - 1].ToString());
            }
            else return false;
        }

    }
}
