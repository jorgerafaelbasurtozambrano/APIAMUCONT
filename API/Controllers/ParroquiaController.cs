using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class ParroquiaController : ApiController
    {
        CatalogoParroquia GestionParroquia = new CatalogoParroquia();
        CatalogoCanton GestionCanton = new CatalogoCanton();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoParroquia")]
        public object IngresoParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (ParroquiaEntidad.encriptada == null || string.IsNullOrEmpty(ParroquiaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(ParroquiaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (ParroquiaEntidad.IdCanton == null || string.IsNullOrEmpty(ParroquiaEntidad.IdCanton.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id del canton";
                        }
                        else if (ParroquiaEntidad.Descripcion == null || string.IsNullOrEmpty(ParroquiaEntidad.Descripcion.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta la descripcion de la parroquia";
                        }
                        else
                        {
                            ParroquiaEntidad.IdCanton = Seguridad.DesEncriptar(ParroquiaEntidad.IdCanton);
                            Canton DatoCanton = new Canton();
                            DatoCanton = GestionCanton.ConsultarCantonPorId(int.Parse(ParroquiaEntidad.IdCanton)).FirstOrDefault();
                            if (DatoCanton == null)
                            {
                                codigo = "500";
                                mensaje = "El canton que desea asignar no existe";
                            }
                            else
                            {
                                Parroquia DatoParroquia = new Parroquia();
                                DatoParroquia = GestionParroquia.ConsultarParroquiaPorDescripcion(ParroquiaEntidad.Descripcion).FirstOrDefault();
                                if (DatoParroquia == null)
                                {
                                    DatoParroquia = new Parroquia();
                                    DatoParroquia = GestionParroquia.IngresarParroquia(ParroquiaEntidad);
                                    if (DatoCanton.IdCanton == null || string.IsNullOrEmpty(DatoCanton.IdCanton.Trim()))
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error en el servidor";
                                    }
                                    else
                                    {
                                        respuesta = DatoParroquia;
                                        codigo = "200";
                                        mensaje = "EXITO";
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                                else
                                {
                                    codigo = "418";
                                    mensaje = "Ya existe la parroquia que quiere insertar";
                                }
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
        [HttpPost]
        [Route("api/TalentoHumano/EliminarParroquia")]
        public object EliminarParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (ParroquiaEntidad.encriptada == null || string.IsNullOrEmpty(ParroquiaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(ParroquiaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (ParroquiaEntidad.IdParroquia == null || string.IsNullOrEmpty(ParroquiaEntidad.IdParroquia.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id de la parroquia";
                        }
                        else
                        {
                            ParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(ParroquiaEntidad.IdParroquia);
                            Parroquia DatoParroquia = new Parroquia();
                            DatoParroquia = GestionParroquia.ConsultarParroquiaPorId(int.Parse(ParroquiaEntidad.IdParroquia)).FirstOrDefault();
                            if (DatoParroquia == null)
                            {
                                codigo = "418";
                                mensaje = "La parroquia que intenta eliminar no existe";
                            }
                            else
                            {
                                if (DatoParroquia.PermitirEliminacion == true)
                                {
                                    if (GestionParroquia.EliminarParroquia(int.Parse(ParroquiaEntidad.IdParroquia)) == true)
                                    {
                                        mensaje = "EXITO";
                                        codigo = "200";
                                    }
                                    else
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al intentar eliminar la parroquia";
                                    }
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "No se puede eliminar la parroquia porque esta siendo usado";
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
        [Route("api/TalentoHumano/ActualizarParroquia")]
        public object ActualizarParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (ParroquiaEntidad.encriptada == null || string.IsNullOrEmpty(ParroquiaEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(ParroquiaEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (ParroquiaEntidad.IdCanton == null || string.IsNullOrEmpty(ParroquiaEntidad.IdCanton.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id del canton";
                        }
                        else if (ParroquiaEntidad.Descripcion == null || string.IsNullOrEmpty(ParroquiaEntidad.Descripcion.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta la descripcion de la parroquia";
                        }
                        else if (ParroquiaEntidad.IdParroquia == null || string.IsNullOrEmpty(ParroquiaEntidad.IdParroquia.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id de la parroquia";
                        }
                        else
                        {
                            ParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(ParroquiaEntidad.IdParroquia);
                            Parroquia DatoParroquia = new Parroquia();
                            DatoParroquia = GestionParroquia.ConsultarParroquiaPorId(int.Parse(ParroquiaEntidad.IdParroquia)).FirstOrDefault();
                            if (DatoParroquia == null)
                            {
                                codigo = "418";
                                mensaje = "La parroquia que intenta actualizar no existe";
                            }
                            else
                            {
                                ParroquiaEntidad.IdCanton = Seguridad.DesEncriptar(ParroquiaEntidad.IdCanton);
                                Canton DatoCanton = new Canton();
                                DatoCanton = GestionCanton.ConsultarCantonPorId(int.Parse(ParroquiaEntidad.IdCanton)).FirstOrDefault();
                                if (DatoCanton == null)
                                {
                                    codigo = "500";
                                    mensaje = "El canton que desea asignar no existe";
                                }
                                else
                                {
                                    DatoParroquia = new Parroquia();
                                    DatoParroquia = GestionParroquia.ModificarParroquia(ParroquiaEntidad);
                                    if (DatoParroquia.IdParroquia == null || string.IsNullOrEmpty(DatoParroquia.IdParroquia))
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al intentar actualizarla parroquia";
                                    }
                                    else
                                    {
                                        respuesta = DatoParroquia;
                                        mensaje = "EXITO";
                                        codigo = "200";
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
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
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ListaParroquia")]
        public object ListaParroquia([FromBody] Tokens Tokens)
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
                        respuesta = GestionParroquia.ObtenerListaParroquia();
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
        [Route("api/TalentoHumano/ListaParroquiaCanton")]
        public object ListaParroquiaCanton(Canton Canton)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Canton.encriptada == null || string.IsNullOrEmpty(Canton.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Canton.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Canton.IdCanton == null || string.IsNullOrEmpty(Canton.IdCanton.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id del canton";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            Canton.IdCanton = Seguridad.DesEncriptar(Canton.IdCanton);
                            respuesta = GestionParroquia.ListarParroquiaCanton(int.Parse(Canton.IdCanton));
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
        [Route("api/TalentoHumano/ConsultarParroquiaParaSeguimiento")]
        public object ConsultarParroquiaParaSeguimiento(Canton Canton)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Canton.encriptada == null || string.IsNullOrEmpty(Canton.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Canton.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Canton.IdCanton == null || string.IsNullOrEmpty(Canton.IdCanton.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id canton";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            Canton.IdCanton = Seguridad.DesEncriptar(Canton.IdCanton);
                            respuesta = GestionParroquia.ConsultarParroquiaParaSeguimiento(int.Parse(Canton.IdCanton));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
    }
}
