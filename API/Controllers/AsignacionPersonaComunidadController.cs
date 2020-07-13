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
    public class AsignacionPersonaComunidadController : ApiController
    {
        CatalogoAsigancionPersonaComunidad GestionAsignacionPersonaComunidad = new CatalogoAsigancionPersonaComunidad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoAsignacionPersonaParroquia")]
        public object IngresoAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignacionPersonaParroquiaEntidad.encriptada == null || string.IsNullOrEmpty(AsignacionPersonaParroquiaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignacionPersonaParroquiaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (AsignacionPersonaParroquiaEntidad.IdParroquia == null || string.IsNullOrEmpty(AsignacionPersonaParroquiaEntidad.IdParroquia.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id de la parroquia";
                        }
                        else if (AsignacionPersonaParroquiaEntidad.IdPersona == null || string.IsNullOrEmpty(AsignacionPersonaParroquiaEntidad.IdPersona.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id de la persona";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            AsignacionPersonaParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdParroquia);
                            AsignacionPersonaParroquiaEntidad.IdPersona = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdPersona);
                            respuesta = GestionAsignacionPersonaComunidad.IngresoAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad);
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
        [Route("api/TalentoHumano/EliminarAsignacionPersonaParroquia")]
        public object EliminarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaComunidadEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignacionPersonaComunidadEntidad.encriptada == null || string.IsNullOrEmpty(AsignacionPersonaComunidadEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignacionPersonaComunidadEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignacionPersonaComunidadEntidad.IdAsignacionPC = Seguridad.DesEncriptar(AsignacionPersonaComunidadEntidad.IdAsignacionPC);
                        respuesta = GestionAsignacionPersonaComunidad.EliminarAsignacionPersonaComunidad(int.Parse(AsignacionPersonaComunidadEntidad.IdAsignacionPC));
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
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
        [Route("api/TalentoHumano/ActualizarAsignacionPersonaParroquia")]
        public object ActualizarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignacionPersonaParroquiaEntidad.encriptada == null || string.IsNullOrEmpty(AsignacionPersonaParroquiaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignacionPersonaParroquiaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignacionPersonaParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdParroquia);
                        AsignacionPersonaParroquiaEntidad.IdPersona = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdPersona);
                        AsignacionPersonaParroquiaEntidad.IdAsignacionPC = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdAsignacionPC);
                        respuesta = GestionAsignacionPersonaComunidad.ModificarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad);
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
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
