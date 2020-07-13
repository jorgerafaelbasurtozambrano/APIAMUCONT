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
    public class TipoProductoController : ApiController
    {
        CatalogoTipoProducto GestionTipoProducto = new CatalogoTipoProducto();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoTipoProducto")]
        public object IngresoTipoProducto(TipoProducto TipoProducto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (TipoProducto.encriptada == null || string.IsNullOrEmpty(TipoProducto.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(TipoProducto.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (TipoProducto.Descripcion == null || string.IsNullOrEmpty(TipoProducto.Descripcion.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el tipo de producto";
                        }
                        else
                        {
                            TipoProducto DatoTipoProducto = new TipoProducto();
                            DatoTipoProducto = GestionTipoProducto.ConsultarTipoProductoPorDescripcion(TipoProducto.Descripcion).FirstOrDefault();
                            if (DatoTipoProducto == null)
                            {
                                DatoTipoProducto = new TipoProducto();
                                DatoTipoProducto = GestionTipoProducto.IngresarTipoProducto(TipoProducto);
                                if (DatoTipoProducto.IdTipoProducto == null || string.IsNullOrEmpty(DatoTipoProducto.IdTipoProducto.Trim()))
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al ingresar el tipo de producto";
                                }
                                else
                                {
                                    mensaje = "EXITO";
                                    codigo = "200";
                                    respuesta = DatoTipoProducto;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                codigo = "418";
                                mensaje = "El tipo de producto " + DatoTipoProducto.Descripcion + " ya existe";
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
        [Route("api/Inventario/EliminarTipoProducto")]
        public object EliminarTipoProducto(TipoProducto TipoProducto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (TipoProducto.encriptada == null || string.IsNullOrEmpty(TipoProducto.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(TipoProducto.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (TipoProducto.IdTipoProducto == null || string.IsNullOrEmpty(TipoProducto.IdTipoProducto.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id del tipo de producto a eliminar";
                        }
                        else
                        {
                            TipoProducto.IdTipoProducto = Seguridad.DesEncriptar(TipoProducto.IdTipoProducto);
                            TipoProducto DatoTipoProducto = new TipoProducto();
                            DatoTipoProducto = GestionTipoProducto.ConsultarTipoProductoPorId(int.Parse(TipoProducto.IdTipoProducto)).FirstOrDefault();
                            if (DatoTipoProducto == null)
                            {
                                codigo = "500";
                                mensaje = "El tipo de producto a eliminar no existe";
                            }
                            else
                            {
                                if (DatoTipoProducto.TipoProductoUtilizado == "1")
                                {
                                    mensaje = "No se puede eliminar el tipo producto porque esta siendo usado";
                                    codigo = "500";
                                }
                                else
                                {
                                    if (GestionTipoProducto.EliminarTipoProducto(int.Parse(TipoProducto.IdTipoProducto)) == true)
                                    {
                                        mensaje = "EXITO";
                                        codigo = "200";
                                    }
                                    else
                                    {
                                        mensaje = "Ocurrio un error al eliminar el tipo de promocion";
                                        codigo = "500";
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
        [Route("api/Inventario/ActualizarTipoProducto")]
        public object ActualizarSembrio(TipoProducto TipoProducto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (TipoProducto.encriptada == null || string.IsNullOrEmpty(TipoProducto.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(TipoProducto.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (TipoProducto.IdTipoProducto == null || string.IsNullOrEmpty(TipoProducto.IdTipoProducto.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tipo de producto";
                        }
                        else if (TipoProducto.Descripcion == null || string.IsNullOrEmpty(TipoProducto.Descripcion.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el tipo de producto";
                        }
                        else
                        {
                            TipoProducto DatoTipoProducto = new TipoProducto();
                            DatoTipoProducto = GestionTipoProducto.ConsultarTipoProductoPorDescripcion(TipoProducto.Descripcion).FirstOrDefault();
                            if (DatoTipoProducto == null)
                            {
                                TipoProducto.IdTipoProducto = Seguridad.DesEncriptar(TipoProducto.IdTipoProducto);
                                DatoTipoProducto = new TipoProducto();
                                DatoTipoProducto = GestionTipoProducto.ConsultarTipoProductoPorId(int.Parse(TipoProducto.IdTipoProducto)).FirstOrDefault();
                                if (DatoTipoProducto == null)
                                {
                                    codigo = "500";
                                    mensaje = "El tipo de producto a eliminar no existe";
                                }
                                else
                                {
                                    DatoTipoProducto = new TipoProducto();
                                    DatoTipoProducto = GestionTipoProducto.ModificarTipoProducto(TipoProducto);
                                    if (DatoTipoProducto.IdTipoProducto == null || string.IsNullOrEmpty(DatoTipoProducto.IdTipoProducto.Trim()))
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al modificar el tipo de producto";
                                    }
                                    else
                                    {
                                        mensaje = "EXITO";
                                        codigo = "200";
                                        respuesta = DatoTipoProducto;
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                            }
                            else
                            {
                                codigo = "418";
                                mensaje = "El tipo producto " + DatoTipoProducto.Descripcion + " ya existe";
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
        [Route("api/Inventario/ListaTipoProductos")]
        public object ListaTipoProductos([FromBody] Tokens Tokens)
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
                        respuesta = GestionTipoProducto.ListarTipoProductos();
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
