using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Inventario;
using Negocio;
using Negocio.Logica.Seguridad;

namespace API.Controllers
{
    public class MedidaController : ApiController
    {
        CatalogoMedida GestionMedida = new CatalogoMedida();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoMedida")]
        public object IngresoMedida(Medida Medida)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Medida.encriptada == null || string.IsNullOrEmpty(Medida.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Medida.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Medida.Descripcion == null || string.IsNullOrEmpty(Medida.Descripcion.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Por favor ingrese la descripcion a guardar";
                        }
                        else
                        {
                            Medida DatoMedida = new Medida();
                            DatoMedida = GestionMedida.ConsultarMedidaPorDescripcion(Medida.Descripcion).FirstOrDefault();
                            if (DatoMedida == null)
                            {
                                DatoMedida = new Medida();
                                DatoMedida = GestionMedida.InsertarMedida(Medida);
                                if (DatoMedida.IdMedida == null || string.IsNullOrEmpty(DatoMedida.IdMedida))
                                {
                                    mensaje = "Ocurrio un error al ingresar la medida";
                                    codigo = "500";
                                }
                                else
                                {
                                    mensaje = "EXITO";
                                    codigo = "200";
                                    respuesta = DatoMedida;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                mensaje = "La medida " + DatoMedida.Descripcion + " ya existe";
                                codigo = "500";
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
        [Route("api/Inventario/EliminarMedida")]
        public object EliminarMedida(Medida Medida)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Medida.encriptada == null || string.IsNullOrEmpty(Medida.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Medida.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Medida.IdMedida == null || string.IsNullOrEmpty(Medida.IdMedida.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id de la medida a eliminar";
                        }
                        else
                        {
                            Medida.IdMedida = Seguridad.DesEncriptar(Medida.IdMedida);
                            Medida DatoMedida = new Medida();
                            DatoMedida = GestionMedida.ConsultarMedidaPorId(int.Parse(Medida.IdMedida)).FirstOrDefault();
                            if (DatoMedida == null)
                            {
                                codigo = "500";
                                mensaje = "La medida que intenta eliminar no existe";
                            }
                            else if (DatoMedida.MedidaUtilizado == "1")
                            {
                                codigo = "500";
                                mensaje = "La medida que intenta eliminar ya esta siendo usada";
                            }
                            else
                            {
                                if (GestionMedida.EliminarMedida(int.Parse(Medida.IdMedida)) == true)
                                {
                                    mensaje = "EXITO";
                                    codigo = "200";
                                }
                                else
                                {
                                    mensaje = "Ocurrio un error al intentar eliminar la medida";
                                    codigo = "500";
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
        [Route("api/Inventario/ActualizarMedida")]
        public object ActualizarMedida(Medida Medida)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Medida.encriptada == null || string.IsNullOrEmpty(Medida.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Medida.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Medida.IdMedida == null || string.IsNullOrEmpty(Medida.IdMedida.Trim()))
                        {
                            mensaje = "Ingrese el id medida";
                            codigo = "418";
                        }
                        else if (Medida.Descripcion == null || string.IsNullOrEmpty(Medida.Descripcion.Trim()))
                        {
                            mensaje = "Ingrese la medida";
                            codigo = "418";
                        }
                        else
                        {
                            Medida DatoMedida = new Medida();
                            DatoMedida = GestionMedida.ConsultarMedidaPorDescripcion(Medida.Descripcion).FirstOrDefault();
                            if (DatoMedida == null)
                            {
                                DatoMedida = new Medida();
                                Medida.IdMedida = Seguridad.DesEncriptar(Medida.IdMedida);
                                DatoMedida = GestionMedida.ConsultarMedidaPorId(int.Parse(Medida.IdMedida)).FirstOrDefault();
                                if (DatoMedida == null)
                                {
                                    mensaje = "La medida que desea actualizar no existe";
                                    codigo = "500";
                                }
                                else
                                {
                                    DatoMedida = new Medida();
                                    DatoMedida = GestionMedida.ModificarMedida(Medida);
                                    if (DatoMedida.IdMedida == null || string.IsNullOrEmpty(DatoMedida.IdMedida.Trim()))
                                    {
                                        mensaje = "Ocurrio un error al tratar de modificar la medida";
                                        codigo = "500";
                                    }
                                    else
                                    {
                                        codigo = "200";
                                        mensaje = "EXITO";
                                        respuesta = DatoMedida;
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                            }
                            else
                            {
                                mensaje = "La medida " + DatoMedida.Descripcion + " ya existe";
                                codigo = "418";
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
        [Route("api/Inventario/ListaMedidas")]
        public object ListaMedidas([FromBody] Tokens Tokens)
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
                        respuesta = GestionMedida.ListarMedidas();
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
