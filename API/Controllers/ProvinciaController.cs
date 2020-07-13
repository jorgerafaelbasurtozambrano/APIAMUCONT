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

namespace API.Controllers
{
    public class ProvinciaController : ApiController
    {
        CatalogoProvincia GestionProvincia = new CatalogoProvincia();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoProvincia")]
        public object IngresoProvincia(Provincia Provincia)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Provincia.encriptada == null || string.IsNullOrEmpty(Provincia.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Provincia.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Provincia.Descripcion == null || string.IsNullOrEmpty(Provincia.Descripcion.Trim()) || Provincia.Descripcion.Trim().ToUpper() == "NULL")
                        {
                            codigo = "400";
                            mensaje = "Falta la descripcion de la provincia";
                        }
                        else
                        {
                            Provincia DatoProvincia = new Provincia();
                            DatoProvincia = GestionProvincia.ConsultarProvinciaPorDescripcion(Provincia.Descripcion.ToUpper()).FirstOrDefault();
                            if (DatoProvincia == null)
                            {
                                DatoProvincia = new Provincia();
                                DatoProvincia = GestionProvincia.IngresoProvincia(Provincia);
                                if (DatoProvincia.IdProvincia == null || string.IsNullOrEmpty(DatoProvincia.IdProvincia.Trim()))
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error en el servidor";
                                }
                                else
                                {
                                    respuesta = DatoProvincia;
                                    codigo = "200";
                                    mensaje = "EXITO";
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                codigo = "418";
                                mensaje = "Ya existe la provincia que quiere insertar";
                            }
                        }
                    }
                }
                objeto = new {codigo, mensaje};
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
        [Route("api/TalentoHumano/EliminarProvincia")]
        public object EliminarProvincia(Provincia Provincia)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Provincia.encriptada == null || string.IsNullOrEmpty(Provincia.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Provincia.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Provincia.IdProvincia == null || string.IsNullOrEmpty(Provincia.IdProvincia.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id de la provincia";
                        }
                        else
                        {
                            Provincia DatoProvincia = new Provincia();
                            Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                            DatoProvincia = GestionProvincia.ConsultarProvinciaPorId(int.Parse(Provincia.IdProvincia)).FirstOrDefault();
                            if (DatoProvincia == null)
                            {
                                codigo = "500";
                                mensaje = "La provincia que quiere eliminar no existe";
                            }
                            else
                            {
                                if (DatoProvincia.PermitirEliminacion == true)
                                {
                                    if (GestionProvincia.EliminarProvincia(int.Parse(Provincia.IdProvincia)) == true)
                                    {
                                        codigo = "200";
                                        mensaje = "EXITO";
                                    }
                                    else
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al eliminar la provincia";
                                    }
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "No se puede eliminar porque esta siendo utilizado";
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
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ActualizarProvincia")]
        public object ActualizarProvincia(Provincia Provincia)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Provincia.encriptada == null || string.IsNullOrEmpty(Provincia.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Provincia.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Provincia.IdProvincia == null || string.IsNullOrEmpty(Provincia.IdProvincia.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id de la provincia";
                        }
                        else if (Provincia.Descripcion == null || string.IsNullOrEmpty(Provincia.Descripcion.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta la descripcion de la provincia";
                        }
                        else
                        {
                            Provincia DatoProvincia = new Provincia();
                            DatoProvincia = GestionProvincia.ConsultarProvinciaPorDescripcion(Provincia.Descripcion.ToUpper()).FirstOrDefault();
                            if (DatoProvincia == null)
                            {
                                Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                                DatoProvincia = new Provincia();
                                DatoProvincia = GestionProvincia.ConsultarProvinciaPorId(int.Parse(Provincia.IdProvincia)).FirstOrDefault();
                                if (DatoProvincia == null)
                                {
                                    codigo = "500";
                                    mensaje = "La provincia que quiere modificar no existe";
                                }
                                else
                                {
                                    DatoProvincia = new Provincia();
                                    DatoProvincia = GestionProvincia.ModificarProvincia(Provincia);
                                    if (DatoProvincia.IdProvincia == null)
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error en el servidor";
                                    }
                                    else
                                    {
                                        respuesta = DatoProvincia;
                                        codigo = "200";
                                        mensaje = "EXITO";
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                            }
                            else
                            {
                                codigo = "418";
                                mensaje = "Ya existe la provincia que quiere modificar";
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
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ListaProvincia")]
        public object ObtenerProvincias([FromBody] Tokens Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Tokens.encriptada == null || string.IsNullOrEmpty(Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = GestionProvincia.ConsultarProvincias();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje };
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
        [Route("api/TalentoHumano/ConsultarProvinciaParaSeguimiento")]
        public object ConsultarProvinciaParaSeguimiento([FromBody] Tokens Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Tokens.encriptada == null || string.IsNullOrEmpty(Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = GestionProvincia.ConsultarProvinciaParaSeguimiento();
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
