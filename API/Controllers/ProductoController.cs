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
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePost = ListaClaves.Where(c => c.Identificador == 1).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(Producto.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (Producto.IdTipoProducto == null || string.IsNullOrEmpty(Producto.IdTipoProducto.Trim()))
                {
                    codigo = "418";
                    mensaje = "Por favor ingrese el id tipo producto";
                }
                else if(Producto.Nombre == null || string.IsNullOrEmpty(Producto.Nombre.Trim()))
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
                        mensaje = "El producto "+DatoProducto.Nombre + " ya existe";
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
        [Route("api/Inventario/EliminarProducto")]
        public object EliminarProducto(Producto Producto)
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
                string ClavePutEncripBD = p.desencriptar(Producto.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                Producto.IdProducto = Seguridad.DesEncriptar(Producto.IdProducto);
                respuesta = GestionProducto.EliminarProducto(int.Parse(Producto.IdProducto));
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
        [Route("api/Inventario/ActualizarProducto")]
        public object ActualizarProducto(Producto Producto)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePut = ListaClaves.Where(c => c.Identificador == 2).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(Producto.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                Producto.IdTipoProducto = Seguridad.DesEncriptar(Producto.IdTipoProducto);
                Producto.IdProducto = Seguridad.DesEncriptar(Producto.IdProducto);
                respuesta = GestionProducto.ModificarProducto(Producto);

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
        [Route("api/Inventario/ListaProductos")]
        public object ListaProductos([FromBody] Tokens Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveGet = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClaveGetEncripBD = p.desencriptar(Tokens.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                respuesta = GestionProducto.ListarProductos();
                //}
                //else
                //{
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



    }
}
