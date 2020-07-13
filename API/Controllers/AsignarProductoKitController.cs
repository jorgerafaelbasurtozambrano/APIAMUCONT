using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Factura;
using Negocio.Logica.Inventario;
using Negocio;
using Negocio.Logica.Seguridad;
namespace API.Controllers
{
    public class AsignarProductoKitController : ApiController
    {
        CatalogoAsignarProductoKit GestionAsignarProductoKit = new CatalogoAsignarProductoKit();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoStock GestionStock = new CatalogoStock();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoAsignarProductoKit")]
        public object IngresoAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignarProductoKit.encriptada == null || string.IsNullOrEmpty(AsignarProductoKit.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignarProductoKit.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignarProductoKit.IdConfigurarProducto = Seguridad.DesEncriptar(AsignarProductoKit.IdConfigurarProducto);
                        AsignarProductoKit.IdAsignarDescuentoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarDescuentoKit);
                        respuesta = GestionAsignarProductoKit.InsertarAsignarProductoKit(AsignarProductoKit);
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
        [Route("api/Inventario/EliminarAsignarProductoKit")]
        public object EliminarAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignarProductoKit.encriptada == null || string.IsNullOrEmpty(AsignarProductoKit.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignarProductoKit.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignarProductoKit.IdAsignarProductoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarProductoKit);
                        respuesta = GestionAsignarProductoKit.EliminarAsignarProductoKit(int.Parse(AsignarProductoKit.IdAsignarProductoKit));
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
        [Route("api/Inventario/ActualizarAsignarProductoKit")]
        public object ActualizarAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignarProductoKit.encriptada == null || string.IsNullOrEmpty(AsignarProductoKit.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignarProductoKit.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignarProductoKit.IdAsignarProductoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarProductoKit);
                        AsignarProductoKit.IdConfigurarProducto = Seguridad.DesEncriptar(AsignarProductoKit.IdConfigurarProducto);
                        AsignarProductoKit.IdAsignarDescuentoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarDescuentoKit);
                        respuesta = GestionAsignarProductoKit.ModificarAsignarProductoKit(AsignarProductoKit);
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
        [Route("api/Inventario/ListaAsignarProductoKit")]
        public object ListaAsignarProductoKit(Kit Kit)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Kit.encriptada == null || string.IsNullOrEmpty(Kit.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Kit.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        Kit.IdKit = Seguridad.DesEncriptar(Kit.IdKit);
                        respuesta = GestionAsignarProductoKit.ListarProductosDeUnKit(int.Parse(Kit.IdKit));
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
