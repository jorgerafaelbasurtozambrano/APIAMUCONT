using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
using Negocio.Logica.Usuarios;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Logica
{
    public class CatalogoPersona
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoUsuario GestionUsuario = new CatalogoUsuario();
        ConsultarUsuariosYPersonas BuscaPersona = new ConsultarUsuariosYPersonas();
        public bool EliminarPersona(int IdPersona)
        {
            try
            {
                ConexionBD.sp_EliminarPersona(IdPersona);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public PersonaEntidad ModificarPersona(PersonaEntidad PersonaEntidad)
        {
            PersonaEntidad DatoNuevoPersona = new PersonaEntidad();
            try
            {
                foreach (var item in ConexionBD.sp_ModificarPersona(int.Parse(PersonaEntidad.IdPersona), PersonaEntidad.NumeroDocumento, PersonaEntidad.ApellidoPaterno.ToUpper(), PersonaEntidad.ApellidoMaterno.ToUpper(), PersonaEntidad.PrimerNombre.ToUpper(), PersonaEntidad.SegundoNombre.ToUpper(), int.Parse(PersonaEntidad.IdTipoDocumento)))
                {
                    PersonaEntidad.IdPersona = item.PersonaIdPersona.ToString();
                    PersonaEntidad.NumeroDocumento = item.PersonaNumeroDocumento;
                    PersonaEntidad.ApellidoPaterno = item.PersonaApellidoPaterno;
                    PersonaEntidad.ApellidoMaterno = item.PersonaApellidoMaterno;
                    PersonaEntidad.PrimerNombre = item.PersonaPrimerNombre;
                    PersonaEntidad.SegundoNombre = item.PersonaSegundoNombre;
                    PersonaEntidad.IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString());
                    PersonaEntidad.TipoDocumento = item.TipoDocumentoDescripcion;
                }
                PersonaEntidad.ListaTelefono[0].IdPersona = PersonaEntidad.IdPersona;
                PersonaEntidad.ListaTelefono[1].IdPersona = PersonaEntidad.IdPersona;

                ModificarTelefono(new TelefonoEntidad() {IdTelefono = PersonaEntidad.ListaTelefono[0].IdTelefono,IdPersona = PersonaEntidad.ListaTelefono[0].IdPersona , Numero = PersonaEntidad.ListaTelefono[0].Numero, IdTipoTelefono = PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono });
                ModificarTelefono(new TelefonoEntidad() { IdTelefono = PersonaEntidad.ListaTelefono[1].IdTelefono, IdPersona = PersonaEntidad.ListaTelefono[1].IdPersona, Numero = PersonaEntidad.ListaTelefono[1].Numero, IdTipoTelefono = PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono });


                if (PersonaEntidad.Correo!=null)
                {
                    IngresoCorreo(new Correo() {IdPersona  = PersonaEntidad.IdPersona ,CorreoValor = PersonaEntidad.Correo});
                }
                if (PersonaEntidad.AsignacionPersonaComunidad.Estado == false)
                {
                    ModificarAsignacionPersonaParroquia(new AsignacionPersonaParroquiaEntidad() {IdAsignacionPC = PersonaEntidad.AsignacionPersonaComunidad.IdAsignacionPC, IdPersona = PersonaEntidad.IdPersona, IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia, Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia });
                }
                else
                {
                    IngresoAsignacionPersonaComunidad(new AsignacionPersonaParroquiaEntidad() { IdPersona = PersonaEntidad.IdPersona, IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia, Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia });
                }
                DatoNuevoPersona = ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                DatoNuevoPersona.IdUsuario = "1";
                return DatoNuevoPersona;
            }
            catch (Exception)
            {
                DatoNuevoPersona = ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                DatoNuevoPersona.IdUsuario = null;
                return DatoNuevoPersona;
            }
        }
        public PersonaEntidad IngresarPersona(PersonaEntidad PersonaEntidad)
        {
            // ingresar la persona
            int IdPersona = 0;
            PersonaEntidad DatoPersonaEntidad = new PersonaEntidad();
            try
            {
                IdPersona = int.Parse(ConexionBD.sp_CrearPersona(PersonaEntidad.NumeroDocumento.Trim(), PersonaEntidad.ApellidoPaterno.ToUpper(), PersonaEntidad.ApellidoMaterno.ToUpper(), PersonaEntidad.PrimerNombre.ToUpper(), PersonaEntidad.SegundoNombre.ToUpper(), int.Parse(PersonaEntidad.IdTipoDocumento)).Select(x => x.Value.ToString()).FirstOrDefault());
                if (IdPersona != 0)
                {
                    //revisar si tiene correo
                    if (PersonaEntidad.Correo != null)
                    {
                        Correo DatoCorreo = new Correo();
                        DatoCorreo = ConsultarCorreoPorPersona(new Correo() { IdPersona = IdPersona.ToString(), CorreoValor = PersonaEntidad.Correo }).FirstOrDefault();
                        if (DatoCorreo == null)
                        {
                            DatoCorreo = new Correo();
                            DatoCorreo = IngresoCorreo(new Correo() { IdPersona = IdPersona.ToString(), CorreoValor = PersonaEntidad.Correo });
                            if (DatoCorreo.IdCorreo == null)
                            {
                                EliminarPersona(IdPersona);
                                DatoPersonaEntidad.IdPersona = null;
                                return DatoPersonaEntidad;
                            }
                        }
                        else
                        {
                            DatoCorreo.CorreoValor = PersonaEntidad.Correo;
                            DatoCorreo.IdPersona = Seguridad.DesEncriptar(DatoCorreo.IdPersona);
                            DatoCorreo.IdCorreo = Seguridad.DesEncriptar(DatoCorreo.IdCorreo);
                            Correo DatoCorreo1 = new Correo();
                            DatoCorreo1 = ModificarCorreo(DatoCorreo);
                            if (DatoCorreo1.IdCorreo == null)
                            {
                                EliminarPersona(IdPersona);
                                DatoPersonaEntidad.IdPersona = null;
                                return DatoPersonaEntidad;
                            }
                        }
                    }
                    //fin correo

                    //telefono
                    Telefono DatoTelefono = new Telefono();
                    DatoTelefono = IngresoTelefono(new Telefono() { IdPersona = IdPersona.ToString(), Numero = PersonaEntidad.ListaTelefono[0].Numero, TipoTelefono = new TipoTelefono() { IdTipoTelefono = PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono } });
                    if (DatoTelefono.IdTelefono == null || string.IsNullOrEmpty(DatoTelefono.IdTelefono.Trim()))
                    {
                        EliminarPersona(IdPersona);
                        DatoPersonaEntidad.IdPersona = null;
                        return DatoPersonaEntidad;
                    }
                    else
                    {
                        DatoTelefono = new Telefono();
                        DatoTelefono = IngresoTelefono(new Telefono() { IdPersona = IdPersona.ToString(), Numero = PersonaEntidad.ListaTelefono[1].Numero, TipoTelefono = new TipoTelefono() { IdTipoTelefono = PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono } });
                        if (DatoTelefono.IdTelefono == null || string.IsNullOrEmpty(DatoTelefono.IdTelefono.Trim()))
                        {
                            EliminarPersona(IdPersona);
                            DatoPersonaEntidad.IdPersona = null;
                            return DatoPersonaEntidad;
                        }
                    }
                    // Fin Telefono

                    //AsignarPersonaParroquia
                    AsignacionPersonaParroquia DatoAsignacionPersonaParroquia = new AsignacionPersonaParroquia();
                    DatoAsignacionPersonaParroquia = IngresoAsignacionPersonaComunidad(new AsignacionPersonaParroquiaEntidad() { IdPersona = IdPersona.ToString(), IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia,Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia });
                    if (DatoAsignacionPersonaParroquia.IdAsignacionPC == null || string.IsNullOrEmpty(DatoAsignacionPersonaParroquia.IdAsignacionPC.Trim()))
                    {
                        EliminarPersona(IdPersona);
                        DatoPersonaEntidad.IdPersona = null;
                        return DatoPersonaEntidad;
                    }
                    //Fin AsignarPersonaParroquia
                    return ConsultarPersonaPorId(IdPersona).FirstOrDefault();
                }
                else
                {
                    DatoPersonaEntidad.IdPersona = null;
                    return DatoPersonaEntidad;
                }
            }
            catch (Exception)
            {
                if (IdPersona != 0)
                {
                    EliminarPersona(IdPersona);
                }
                DatoPersonaEntidad.IdPersona = null;
                return DatoPersonaEntidad;
            }
            

        }
        public List<PersonaEntidad> ConsultarPersonaPorIdentificacion(string Identifacion)
        {
            List<PersonaEntidad> ListaPersona = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonaPorIdentificacion(Identifacion))
            {
                ListaPersona.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    PrimerNombre = item.PrimerNombre,
                    SegundoNombre = item.SegundoNombre,
                    NumeroDocumento = item.NumeroDocumento,
                    ApellidoMaterno = item.ApellidoMaterno,
                    ApellidoPaterno = item.ApellidoPaterno,
                });
            }
            return ListaPersona;
        }
        public List<PersonaEntidad> ListaPersonasDependiendoDeTipoUsuario(int identificador)
        {
            List<PersonaEntidad> ListaPersonaEntidad = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonasDependeDeTipoDeUsuario(identificador))
            {
                ListaPersonaEntidad.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    IdTipoDocumento = Seguridad.Encriptar(item.PersonaIdTipoDocumento.ToString()),
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    AsignacionTipoUsuario = new AsignacionTipoUsuario() {
                        IdAsignacionTUEncriptada = Seguridad.Encriptar(item.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                        FechaCreacion = item.AsignacionTipoUsuarioFechaCreacion,
                        Estado = item.AsignacionTipoUsuarioEstado,
                        TipoUsuario = new TipoUsuario()
                        {
                            IdTipoUsuario = Seguridad.Encriptar(item.TipoUsuarioIdTipoUsuario.ToString()),
                            Descripcion = item.TipoUsuarioDescripcion,
                            Identificacion = item.TipoUsuarioIdentificacion,
                        }
                    },
                });
            }
            return ListaPersonaEntidad;
        }

        public List<PersonaEntidad> ConsultarPersonaPorId(int idPersona)
        {
            List<PersonaEntidad> ListaPersona = new List<PersonaEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarPersonaPorId(idPersona))
            {
                string permitirEliminar = "0";
                if (item.TieneComunidadFactura == "1" || item.TieneFacturaVenta == "1" || item.TieneUsuario == "1")
                {
                    permitirEliminar = "0";
                }
                else
                {
                    permitirEliminar = "1";
                }

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

                foreach (var item3 in ConexionBD.sp_ConsultarResidenciaPersona(item.IdPersona))
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

                ListaPersona.Add(new PersonaEntidad()
                {
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    NumeroDocumento = item.NumeroDocumento,
                    ApellidoPaterno = item.ApellidoPaterno,
                    ApellidoMaterno = item.ApellidoMaterno,
                    PrimerNombre = item.PrimerNombre,
                    SegundoNombre = item.SegundoNombre,
                    IdTipoDocumento = Seguridad.Encriptar(item.IdTipoDocumento.ToString()),
                    TipoDocumento = item.Descripcion,
                    ListaTelefono = ListaTelefonos,
                    ListaCorreo = ListaCorreos,
                    AsignacionPersonaParroquia = ListaAsignacionPersonaParroquia,
                    IdUsuario = permitirEliminar
                });
            }
            return ListaPersona;
        }

        //CORREO
        public Correo IngresoCorreo(Correo Correo)
        {
            foreach (var item in ConexionBD.sp_CrearCorreo(int.Parse(Correo.IdPersona), Correo.CorreoValor))
            {
                Correo.IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString());
                Correo.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                Correo.CorreoValor = item.Correo;
                Correo.Estado = item.Estado;
                Correo.FechaCreacion = item.FechaCreacion;
            }
            return Correo;
        }

        public List<Correo> ConsultarCorreoPorPersona(Correo _Correo)
        {
            List<Correo> ListaCorreo = new List<Correo>();
            foreach (var item in ConexionBD.sp_ConsultarCorreoPorPersona(int.Parse(_Correo.IdPersona), _Correo.CorreoValor))
            {
                ListaCorreo.Add(new Correo()
                {
                    IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString()),
                    CorreoValor = item.Correo,
                    Estado = item.Estado,
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    FechaCreacion = item.FechaCreacion
                });
            }
            return ListaCorreo;
        }

        public Correo ModificarCorreo(Correo Correo)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarCorreo(int.Parse(Correo.IdCorreo), int.Parse(Correo.IdPersona), Correo.CorreoValor))
                {
                    Correo.IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString());
                    Correo.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                    Correo.CorreoValor = item.Correo;
                    Correo.Estado = item.Estado;
                    Correo.FechaCreacion = item.FechaCreacion;
                }
                return Correo;
            }
            catch (Exception)
            {
                Correo.IdCorreo = null;
                return Correo;
            }
        }


        //Telefono

        public List<Telefono> ConsultarTelefonoPorPersona(int idPersona)
        {
            List<Telefono> ListaTelefonos = new List<Telefono>();
            foreach (var item1 in ConexionBD.sp_ConsultarTelefonoPersona(idPersona))
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
            return ListaTelefonos;
        }
        public Telefono IngresoTelefono(Telefono Telefono)
        {
            foreach (var item in ConexionBD.sp_CrearTelefono(int.Parse(Telefono.IdPersona), Telefono.Numero, int.Parse(Telefono.TipoTelefono.IdTipoTelefono)))
            {
                Telefono.IdTelefono = Seguridad.Encriptar(item.IdTelefono.ToString());
                Telefono.Numero = item.Numero;
                Telefono.FechaCreacion = item.FechaCreacion;
            }
            return Telefono;
        }
        public Telefono ModificarTelefono(TelefonoEntidad TelefonoEntidad)
        {
            Telefono DatoTelefonoEntidad = new Telefono();
            try
            {
                foreach (var item in ConexionBD.sp_ModificarTelefono(int.Parse(TelefonoEntidad.IdTelefono), int.Parse(TelefonoEntidad.IdPersona), TelefonoEntidad.Numero, int.Parse(TelefonoEntidad.IdTipoTelefono)))
                {
                    DatoTelefonoEntidad.IdTelefono = Seguridad.Encriptar(item.TelefonoIdTelefono.ToString());
                    DatoTelefonoEntidad.IdPersona = Seguridad.Encriptar(item.TelefonoIdPersona.ToString());
                    DatoTelefonoEntidad.Numero = item.TelefonoNumero;
                    DatoTelefonoEntidad.TipoTelefono = new TipoTelefono()
                    {
                        IdTipoTelefono = Seguridad.Encriptar(item.TipoTelefonoIdTipoTelefono.ToString()),
                        Descripcion = item.TipoTelefonoDescripcion,
                        Identificador = item.TipoTelefonoIdentificador,
                        FechaCreacion = item.TipoTelefonoFechaCreacion,
                        Estado = item.TipoTelefonoEstado,
                    };
                    DatoTelefonoEntidad.FechaCreacion = item.TelefonoFechaCreacion;
                    DatoTelefonoEntidad.Estado = item.TelefonoEstado;
                }
                return DatoTelefonoEntidad;
            }
            catch (Exception)
            {
                DatoTelefonoEntidad.IdTelefono = null;
                return DatoTelefonoEntidad;
            }
        }

        //Asignar Persona Parroquia
        public AsignacionPersonaParroquia IngresoAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            AsignacionPersonaParroquia DatoAsignacionPersonaParroquia = new AsignacionPersonaParroquia();
            foreach (var item in ConexionBD.sp_CrearAsigancionPP(int.Parse(AsignacionPersonaParroquiaEntidad.IdPersona), int.Parse(AsignacionPersonaParroquiaEntidad.IdParroquia), AsignacionPersonaParroquiaEntidad.Referencia))
            {
                DatoAsignacionPersonaParroquia.IdAsignacionPC = Seguridad.Encriptar(item.IdAsignacionPP.ToString());
                DatoAsignacionPersonaParroquia.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                DatoAsignacionPersonaParroquia.Parroquia = new Parroquia()
                {
                    IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                };
                DatoAsignacionPersonaParroquia.FechaCreacion = item.FechaCreacion;
            }
            return DatoAsignacionPersonaParroquia;
        }
        public bool ModificarAsignacionPersonaParroquia(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarAsignacionPC(int.Parse(AsignacionPersonaParroquiaEntidad.IdAsignacionPC), int.Parse(AsignacionPersonaParroquiaEntidad.IdPersona), int.Parse(AsignacionPersonaParroquiaEntidad.IdParroquia), AsignacionPersonaParroquiaEntidad.Referencia);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
