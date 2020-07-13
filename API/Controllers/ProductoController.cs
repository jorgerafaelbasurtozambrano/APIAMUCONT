using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.Inventario;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class ProductoController : ApiController
    {
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoProducto GestionProducto = new CatalogoProducto();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();


        [HttpPost]
        [Route("api/Inventario/IngresoProducto")]
        public object IngresoProducto(Producto Producto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Producto.encriptada == null || string.IsNullOrEmpty(Producto.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Producto.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Producto.IdTipoProducto == null || string.IsNullOrEmpty(Producto.IdTipoProducto.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Por favor ingrese el id tipo producto";
                        }
                        else if (Producto.Nombre == null || string.IsNullOrEmpty(Producto.Nombre.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Por favor ingrese el nombre del producto";
                        }
                        else
                        {
                            Producto.IdTipoProducto = Seguridad.DesEncriptar(Producto.IdTipoProducto);
                            Producto DatoProducto = new Producto();
                            DatoProducto = GestionProducto.ConsultarProductoPorNombre(Producto.Nombre).FirstOrDefault();
                            if (DatoProducto == null)
                            {
                                DatoProducto = new Producto();
                                DatoProducto = GestionProducto.IngresarProducto(Producto);
                                if (DatoProducto.IdProducto == null || string.IsNullOrEmpty(DatoProducto.IdProducto))
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al intentar guardar el producto";
                                }
                                else
                                {
                                    codigo = "200";
                                    mensaje = "Exito";
                                    respuesta = DatoProducto;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "El producto " + DatoProducto.Nombre + " ya existe";
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
        [Route("api/Inventario/EliminarProducto")]
        public object EliminarProducto(Producto Producto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Producto.encriptada == null || string.IsNullOrEmpty(Producto.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Producto.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Producto.IdProducto == null || string.IsNullOrEmpty(Producto.IdProducto.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id producto";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            Producto.IdProducto = Seguridad.DesEncriptar(Producto.IdProducto);
                            respuesta = GestionProducto.EliminarProducto(int.Parse(Producto.IdProducto));
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
        [Route("api/Inventario/ActualizarProducto")]
        public object ActualizarProducto(Producto Producto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Producto.encriptada == null || string.IsNullOrEmpty(Producto.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Producto.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Producto.IdTipoProducto == null || string.IsNullOrEmpty(Producto.IdTipoProducto.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tipo producto";
                        }
                        else if (Producto.IdTipoProducto == null || string.IsNullOrEmpty(Producto.IdTipoProducto.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id producto";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            Producto.IdTipoProducto = Seguridad.DesEncriptar(Producto.IdTipoProducto);
                            Producto.IdProducto = Seguridad.DesEncriptar(Producto.IdProducto);
                            respuesta = GestionProducto.ModificarProducto(Producto);
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                }
                objeto = new { codigo, mensaje, };
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
        [Route("api/Inventario/ListaProductos")]
        public object ListaProductos([FromBody] Tokens Tokens)
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
                        respuesta = GestionProducto.ListarProductos();
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
