using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Seguridad;
using Negocio;


namespace API.Controllers
{
    public class PersonaController : ApiController
    {
        static ConsultarUsuariosYPersonas GestionUsuarios = new ConsultarUsuariosYPersonas();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoTipoDocumento GestionTipoDocumento = new CatalogoTipoDocumento();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoPersona")]
        public object IngresoPersona(PersonaEntidad PersonaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (PersonaEntidad.encriptada == null || string.IsNullOrEmpty(PersonaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(PersonaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (PersonaEntidad.Telefono1 == null || string.IsNullOrEmpty(PersonaEntidad.Telefono1.Trim()))
                        {
                            if (PersonaEntidad.NumeroDocumento == null || string.IsNullOrEmpty(PersonaEntidad.NumeroDocumento.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el numero de documento de la persona";
                            }
                            else if (PersonaEntidad.ApellidoMaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoMaterno.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el apellido materno";
                            }
                            else if (PersonaEntidad.ApellidoPaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoPaterno.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el apellido paterno";
                            }
                            else if (PersonaEntidad.PrimerNombre == null || string.IsNullOrEmpty(PersonaEntidad.PrimerNombre.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta primer nombre";
                            }
                            else if (PersonaEntidad.SegundoNombre == null || string.IsNullOrEmpty(PersonaEntidad.SegundoNombre.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el segun nombre";
                            }
                            else if (PersonaEntidad.IdTipoDocumento == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoDocumento.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el id del tipo de documento";
                            }
                            else
                            {
                                PersonaEntidad DatoPersona = new PersonaEntidad();
                                DatoPersona = GestionPersona.ConsultarPersonaPorIdentificacion(PersonaEntidad.NumeroDocumento.Trim()).FirstOrDefault();
                                if (DatoPersona == null)
                                {
                                    PersonaEntidad.IdTipoDocumento = Seguridad.DesEncriptar(PersonaEntidad.IdTipoDocumento);
                                    DatoPersona = new PersonaEntidad();
                                    DatoPersona = GestionPersona.CrearSoloDatosDePersona(PersonaEntidad);
                                    if (DatoPersona.IdPersona == null || string.IsNullOrEmpty(DatoPersona.IdPersona))
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al intentar guardar la persona";
                                    }
                                    else
                                    {
                                        respuesta = DatoPersona;
                                        mensaje = "EXITO";
                                        codigo = "200";
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                                else
                                {
                                    codigo = "418";
                                    mensaje = "Ya existe una persona con el mismo numero de identificación";
                                }
                            }
                        }
                        else
                        {
                            if (PersonaEntidad.NumeroDocumento == null || string.IsNullOrEmpty(PersonaEntidad.NumeroDocumento.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el numero de documento de la persona";
                            }
                            else if (PersonaEntidad.ApellidoMaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoMaterno.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el apellido materno";
                            }
                            else if (PersonaEntidad.ApellidoPaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoPaterno.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el apellido paterno";
                            }
                            else if (PersonaEntidad.PrimerNombre == null || string.IsNullOrEmpty(PersonaEntidad.PrimerNombre.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta primer nombre";
                            }
                            else if (PersonaEntidad.SegundoNombre == null || string.IsNullOrEmpty(PersonaEntidad.SegundoNombre.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el segun nombre";
                            }
                            else if (PersonaEntidad.IdTipoDocumento == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoDocumento.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el id del tipo de documento";
                            }
                            else if (PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta el id de la parroquia";
                            }
                            else if (PersonaEntidad.AsignacionPersonaComunidad.Referencia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Referencia.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Falta la referencia";
                            }
                            else if (PersonaEntidad.Telefono1 == null || string.IsNullOrEmpty(PersonaEntidad.Telefono1.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Ingrese el primer numero de telefono";
                            }
                            else if (PersonaEntidad.IdTipoTelefono1 == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoTelefono1.Trim()))
                            {
                                codigo = "418";
                                mensaje = "Ingrese id tipo telefono del primer numero de telefono";
                            }
                            else
                            {
                                PersonaEntidad.IdTipoDocumento = Seguridad.DesEncriptar(PersonaEntidad.IdTipoDocumento);
                                PersonaEntidad.IdTipoTelefono1 = Seguridad.DesEncriptar(PersonaEntidad.IdTipoTelefono1);
                                if (PersonaEntidad.Telefono2 != null)
                                {
                                    if (PersonaEntidad.IdTipoTelefono2 == null)
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese el segundo id tipo de telefono";
                                        objeto = new { codigo, mensaje };
                                        return objeto;
                                    }
                                    else
                                    {
                                        PersonaEntidad.IdTipoTelefono2 = Seguridad.DesEncriptar(PersonaEntidad.IdTipoTelefono2);
                                    }
                                }

                                PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia = Seguridad.DesEncriptar(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia);
                                TipoDocumento DatoTipoDocumento = new TipoDocumento();
                                DatoTipoDocumento = GestionTipoDocumento.ListarTiposDocumentosPorId(int.Parse(PersonaEntidad.IdTipoDocumento)).FirstOrDefault();
                                if (DatoTipoDocumento == null)
                                {
                                    codigo = "418";
                                    mensaje = "El tipo de documento que quiere asignar no existe";
                                }
                                else
                                {
                                    PersonaEntidad DatoPersona = new PersonaEntidad();
                                    DatoPersona = GestionPersona.ConsultarPersonaPorIdentificacion(PersonaEntidad.NumeroDocumento.Trim()).FirstOrDefault();
                                    if (DatoPersona == null)
                                    {
                                        DatoPersona = new PersonaEntidad();
                                        DatoPersona = GestionPersona.IngresarPersona(PersonaEntidad);
                                        if (DatoPersona.IdPersona == null || string.IsNullOrEmpty(DatoPersona.IdPersona))
                                        {
                                            codigo = "500";
                                            mensaje = "Ocurrio un error al intentar guardar la persona";
                                        }
                                        else
                                        {
                                            respuesta = DatoPersona;
                                            mensaje = "EXITO";
                                            codigo = "200";
                                            objeto = new { codigo, mensaje, respuesta };
                                            return objeto;
                                        }
                                    }
                                    else
                                    {
                                        codigo = "418";
                                        mensaje = "Ya existe una persona con el mismo numero de identificación";
                                    }
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/EliminarPersona")]
        public object EliminarPersona(PersonaEntidad PersonaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (PersonaEntidad.encriptada == null || string.IsNullOrEmpty(PersonaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(PersonaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (PersonaEntidad.IdPersona == null || string.IsNullOrEmpty(PersonaEntidad.IdPersona.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id de la persona a eliminar";
                        }
                        else
                        {
                            PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                            PersonaEntidad DatoPersona = new PersonaEntidad();
                            DatoPersona = GestionPersona.ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                            if (DatoPersona == null)
                            {
                                codigo = "418";
                                mensaje = "La persona que intenta eliminar no existe";
                            }
                            else
                            {
                                if (DatoPersona.IdUsuario == "1")
                                {
                                    if (GestionPersona.EliminarPersona(int.Parse(PersonaEntidad.IdPersona)) == true)
                                    {
                                        mensaje = "EXITO";
                                        codigo = "200";
                                    }
                                    else
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al intentar eliminar la persona";
                                    }
                                }
                                else
                                {
                                    codigo = "418";
                                    mensaje = "No puede eliminar esta persona porque ya ha ejercido actividad dentro del sistema";
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ActualizarPersona")]
        public object ActualizarPersona(PersonaEntidad PersonaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (PersonaEntidad.encriptada == null || string.IsNullOrEmpty(PersonaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(PersonaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (PersonaEntidad.IdPersona == null || string.IsNullOrEmpty(PersonaEntidad.IdPersona.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta el numero de documento de la persona";
                        }
                        else if (PersonaEntidad.ApellidoMaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoMaterno.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta el apellido materno";
                        }
                        else if (PersonaEntidad.ApellidoPaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoPaterno.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta el apellido paterno";
                        }
                        else if (PersonaEntidad.PrimerNombre == null || string.IsNullOrEmpty(PersonaEntidad.PrimerNombre.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta primer nombre";
                        }
                        else if (PersonaEntidad.SegundoNombre == null || string.IsNullOrEmpty(PersonaEntidad.SegundoNombre.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta el segun nombre";
                        }
                        else if (PersonaEntidad.IdTipoDocumento == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoDocumento.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta el id del tipo de documento";
                        }
                        else
                        {
                            PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                            PersonaEntidad DatoPersona = new PersonaEntidad();
                            DatoPersona = GestionPersona.ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                            if (DatoPersona == null)
                            {
                                codigo = "418";
                                mensaje = "La persona que intenta actualizar no existe";
                            }
                            else
                            {
                                if (PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia.Trim()))
                                {
                                    codigo = "418";
                                    mensaje = "Falta el id de la parroquia";
                                }
                                else if (PersonaEntidad.AsignacionPersonaComunidad.Referencia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Referencia.Trim()))
                                {
                                    codigo = "418";
                                    mensaje = "Falta la referencia";
                                }
                                else if (PersonaEntidad.Telefono1 == null || string.IsNullOrEmpty(PersonaEntidad.Telefono1.Trim()))
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese el primer numero de telefono";
                                }
                                else if (PersonaEntidad.IdTipoTelefono1 == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoTelefono1.Trim()))
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese id tipo telefono del primer numero de telefono";
                                }
                                else
                                {
                                    PersonaEntidad.IdTipoDocumento = Seguridad.DesEncriptar(PersonaEntidad.IdTipoDocumento);
                                    PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia = Seguridad.DesEncriptar(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia);
                                    if (DatoPersona.AsignacionPersonaParroquia.Count > 0)
                                    {
                                        if (PersonaEntidad.IdTelefono1 == null || string.IsNullOrEmpty(PersonaEntidad.IdTelefono1.Trim()))
                                        {
                                            codigo = "418";
                                            mensaje = "Ingrese el id primer numero de telefono";
                                        }
                                        else
                                        {
                                            PersonaEntidad.IdTelefono1 = Seguridad.DesEncriptar(PersonaEntidad.IdTelefono1);
                                            TipoDocumento DatoTipoDocumento = new TipoDocumento();
                                            DatoTipoDocumento = GestionTipoDocumento.ListarTiposDocumentosPorId(int.Parse(PersonaEntidad.IdTipoDocumento)).FirstOrDefault();
                                            if (DatoTipoDocumento == null)
                                            {
                                                codigo = "418";
                                                mensaje = "El tipo de documento que quiere asignar no existe";
                                            }
                                            else
                                            {
                                                List<Telefono> ListaTelefono = new List<Telefono>();
                                                ListaTelefono = GestionPersona.ConsultarTelefonoPorPersona(int.Parse(PersonaEntidad.IdPersona));
                                                if (ListaTelefono.Where(p => Seguridad.DesEncriptar(p.IdTelefono) == PersonaEntidad.IdTelefono1).FirstOrDefault() == null)
                                                {
                                                    codigo = "418";
                                                    mensaje = "No se a encontrado el primer telefono a modificar";
                                                }
                                                else
                                                {
                                                    PersonaEntidad DatoPersonaAModificar = new PersonaEntidad();
                                                    DatoPersona.PrimerNombre = PersonaEntidad.PrimerNombre;
                                                    DatoPersona.SegundoNombre = PersonaEntidad.SegundoNombre;
                                                    DatoPersona.ApellidoMaterno = PersonaEntidad.ApellidoMaterno;
                                                    DatoPersona.ApellidoPaterno = PersonaEntidad.ApellidoPaterno;
                                                    DatoPersona.IdPersona = Seguridad.DesEncriptar(DatoPersona.IdPersona);
                                                    DatoPersona.IdTipoDocumento = PersonaEntidad.IdTipoDocumento;
                                                    DatoPersona.IdTelefono1 = PersonaEntidad.IdTelefono1;
                                                    DatoPersona.Telefono1 = PersonaEntidad.Telefono1;
                                                    DatoPersona.IdTipoTelefono1 = Seguridad.DesEncriptar(PersonaEntidad.IdTipoTelefono1);
                                                    if (ListaTelefono.Count > 1)
                                                    {
                                                        if (PersonaEntidad.Telefono2 != null)
                                                        {
                                                            if (PersonaEntidad.IdTipoTelefono2 == null)
                                                            {
                                                                codigo = "200";
                                                                mensaje = "Ingrese el id tipo telefono 2";
                                                                objeto = new { codigo, mensaje };
                                                                return objeto;
                                                            }
                                                            else
                                                            {
                                                                ListaTelefono[1].IdTelefono = Seguridad.DesEncriptar(ListaTelefono[1].IdTelefono);
                                                                DatoPersona.IdTelefono2 = ListaTelefono[1].IdTelefono;
                                                                DatoPersona.IdTipoTelefono2 = Seguridad.DesEncriptar(PersonaEntidad.IdTipoTelefono2);
                                                                DatoPersona.Telefono2 = PersonaEntidad.Telefono2;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DatoPersona.IdTelefono2 = null;
                                                        if (PersonaEntidad.Telefono2.ToString().ToUpper() != "NULL")
                                                        {
                                                            DatoPersona.IdTipoTelefono2 = Seguridad.DesEncriptar(PersonaEntidad.IdTipoTelefono2);
                                                            DatoPersona.Telefono2 = PersonaEntidad.Telefono2;
                                                        }
                                                    }
                                                    if (Seguridad.DesEncriptar(DatoPersona.AsignacionPersonaParroquia.FirstOrDefault().Parroquia.IdParroquia) == PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia)
                                                    {
                                                        DatoPersona.AsignacionPersonaComunidad = new AsignacionPersonaParroquia()
                                                        {
                                                            IdAsignacionPC = Seguridad.DesEncriptar(DatoPersona.AsignacionPersonaParroquia.FirstOrDefault().IdAsignacionPC),
                                                            Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia,
                                                            Estado = false,
                                                            Parroquia = new Parroquia()
                                                            {
                                                                IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia,
                                                            }
                                                        };
                                                    }
                                                    else
                                                    {
                                                        DatoPersona.AsignacionPersonaComunidad = new AsignacionPersonaParroquia()
                                                        {
                                                            Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia,
                                                            Estado = true,
                                                            Parroquia = new Parroquia()
                                                            {
                                                                IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia,
                                                            }
                                                        };
                                                    }

                                                    if (PersonaEntidad.Correo != null)
                                                    {
                                                        DatoPersona.Correo = PersonaEntidad.Correo;
                                                    }
                                                    DatoPersonaAModificar = GestionPersona.ModificarPersona(DatoPersona);
                                                    if (DatoPersonaAModificar.IdUsuario == null)
                                                    {
                                                        codigo = "500";
                                                        mensaje = "Ocurrio un error al tratar de modificar la persona";
                                                        respuesta = DatoPersonaAModificar;
                                                        objeto = new { codigo, mensaje, respuesta };
                                                        return objeto;
                                                    }
                                                    else
                                                    {
                                                        codigo = "200";
                                                        mensaje = "EXITO";
                                                        respuesta = DatoPersonaAModificar;
                                                        objeto = new { codigo, mensaje, respuesta };
                                                        return objeto;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PersonaEntidad DatoModificados = new PersonaEntidad();
                                        PersonaEntidad.NumeroDocumento = DatoPersona.NumeroDocumento;
                                        PersonaEntidad.IdTipoTelefono1 = Seguridad.DesEncriptar(PersonaEntidad.IdTipoTelefono1);
                                        DatoModificados = GestionPersona.CompletarDatosPersona(PersonaEntidad);
                                        if (DatoModificados.IdUsuario == null)
                                        {
                                            codigo = "500";
                                            mensaje = "Ocurrio un error al tratar de completar la informacion de la persona";
                                            respuesta = DatoModificados;
                                            objeto = new { codigo, mensaje, respuesta };
                                            return objeto;
                                        }
                                        else
                                        {
                                            codigo = "200";
                                            mensaje = "EXITO";
                                            respuesta = DatoModificados;
                                            objeto = new { codigo, mensaje, respuesta };
                                            return objeto;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/BuscarPersona")]
        public object BuscarDatosPersona(PersonaEntidad PersonaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (PersonaEntidad.encriptada == null || string.IsNullOrEmpty(PersonaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(PersonaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                        respuesta = GestionUsuarios.FiltrarPersona(int.Parse(PersonaEntidad.IdPersona));
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ConsultarPersonasDependeDeTipoUsuario")]
        public object ConsultarPersonasDependeDeTipoUsuario(TipoUsuario TipoUsuario)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (TipoUsuario.encriptada == null || string.IsNullOrEmpty(TipoUsuario.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(TipoUsuario.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (TipoUsuario.Identificacion == null || string.IsNullOrEmpty(TipoUsuario.Identificacion))
                        {
                            mensaje = "Ingrese El ID TIPO USUARIO";
                            codigo = "418";
                            objeto = new { codigo, mensaje };
                            return objeto;
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            respuesta = GestionPersona.ListaPersonasDependiendoDeTipoUsuario(int.Parse(TipoUsuario.Identificacion));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ActualizarCorreo")]
        public object ActualizarCorreo(PersonaEntidad PersonaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (PersonaEntidad.encriptada == null || string.IsNullOrEmpty(PersonaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(PersonaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (PersonaEntidad.IdPersona == null || string.IsNullOrEmpty(PersonaEntidad.IdPersona.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Falta el numero de documento de la persona";
                        }
                        else if (PersonaEntidad.Correo == null || string.IsNullOrEmpty(PersonaEntidad.Correo.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el correo de la persona";
                        }
                        else
                        {
                            PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                            Correo DatoCorreo = new Correo();
                            DatoCorreo = GestionPersona.IngresoCorreo(new Correo() { IdPersona = PersonaEntidad.IdPersona, CorreoValor = PersonaEntidad.Correo });
                            if (DatoCorreo.IdCorreo == null || string.IsNullOrEmpty(DatoCorreo.IdCorreo.Trim()))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al modificar el correo";
                            }
                            else
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                                respuesta = DatoCorreo;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
    }
}
