using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio;
using Negocio.Entidades.DatoUsuarios;

namespace API.Controllers
{
    public class CorreoController : ApiController
    {
        CatalogoCorreo GestionCorreo = new CatalogoCorreo();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoCorreo")]
        public object IngresoCorreo(Correo Correo)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                //var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                //var _clavePost = ListaClaves.Where(c => c.Identificador == 1).FirstOrDefault();
                //Object resultado = new object();
                //string ClavePutEncripBD = p.desencriptar(Correo.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                //string variable = Correo.CorreoValor.ToString();
                if (Correo.CorreoValor == null || string.IsNullOrEmpty(Correo.CorreoValor.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta ingresar el correo";
                }
                else if(Correo.IdPersona == null || string.IsNullOrEmpty(Correo.IdPersona.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta seleccionar la persona";
                }
                else
                {
                    Correo.IdPersona = Seguridad.DesEncriptar(Correo.IdPersona);
                    PersonaEntidad DatoPersona = new PersonaEntidad();
                    DatoPersona = GestionPersona.ConsultarPersonaPorId(int.Parse(Correo.IdPersona)).FirstOrDefault();
                    if (DatoPersona == null)
                    {
                        codigo = "500";
                        mensaje = "La persona seleccionada no existe";
                    }
                    else
                    {
                        Correo DatoCorreo = new Correo();
                        DatoCorreo = GestionCorreo.ConsultarCorreoPorPersona(Correo).FirstOrDefault();
                        if (DatoCorreo == null)
                        {
                            DatoCorreo = GestionCorreo.IngresoCorreo(Correo);
                            if (DatoCorreo.IdCorreo == null || string.IsNullOrEmpty(DatoCorreo.IdCorreo))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar ingresar el correo";
                            }
                            else
                            {
                                respuesta = DatoCorreo;
                                mensaje = "EXITO";
                                codigo = "200";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                        else
                        {
                            if (DatoCorreo.Estado == false)
                            {
                                DatoCorreo.IdCorreo = Seguridad.DesEncriptar(DatoCorreo.IdCorreo);
                                DatoCorreo.IdPersona = Seguridad.DesEncriptar(DatoCorreo.IdPersona);
                                if (GestionCorreo.HabilitarCorreo(DatoCorreo) == true)
                                {
                                    respuesta = DatoCorreo;
                                    mensaje = "EXITO";
                                    codigo = "200";
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al intentar ingresar el correo";
                                }
                            }
                            else
                            {
                                respuesta = Correo;
                                mensaje = "EXITO";
                                codigo = "200";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
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
        [Route("api/TalentoHumano/EliminarCorreo")]
        public object EliminarCorreo(Correo Correo)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveDelete = ListaClaves.Where(c => c.Identificador == 3).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(Correo.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Correo.IdCorreo = Seguridad.DesEncriptar(Correo.IdCorreo);
                    
                    respuesta = GestionCorreo.EliminarCorreo(int.Parse(Correo.IdCorreo));
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje, respuesta };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/TalentoHumano/ActualizarCorreo")]
        public object ActualizarCorreo(Correo Correo)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                //var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                //var _clavePut = ListaClaves.Where(c => c.Identificador == 2).FirstOrDefault();
                Object resultado = new object();
                //string ClavePutEncripBD = p.desencriptar(Correo.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                if (Correo.IdCorreo == null || string.IsNullOrEmpty(Correo.IdCorreo.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta ingresar el id correo";
                }
                else if (Correo.CorreoValor == null || string.IsNullOrEmpty(Correo.CorreoValor.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta ingresar el correo";
                }
                else
                {
                    Correo.IdCorreo = Seguridad.DesEncriptar(Correo.IdCorreo);
                    Correo DatoCorreo = new Correo();
                    DatoCorreo = GestionCorreo.ConsultarCorreoPorIdCorreo(int.Parse(Correo.IdCorreo)).FirstOrDefault();
                    if (DatoCorreo == null)
                    {
                        codigo = "500";
                        mensaje = "El correo que intenta modificar no existe";
                    }
                    else
                    {
                        DatoCorreo.IdPersona = Seguridad.DesEncriptar(DatoCorreo.IdPersona);
                        DatoCorreo.IdCorreo = Seguridad.DesEncriptar(DatoCorreo.IdCorreo);
                        DatoCorreo.CorreoValor = Correo.CorreoValor;
                        Correo = new Correo();
                        Correo = GestionCorreo.ModificarCorreo(DatoCorreo);
                        if (Correo.IdCorreo == null)
                        {
                            codigo = "418";
                            mensaje = "Ocurrio un error al intentar modificar el correo";
                        }
                        else
                        {
                            codigo = "200";
                            mensaje = "EXITO";
                            respuesta = Correo;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }


    }
}
