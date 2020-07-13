using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Factura;
using Negocio.Logica.Inventario;
using Negocio.Logica.Seguridad;
using System;
using System.Linq;
using System.Web.Http;

namespace API.Controllers
{
    public class StockController : ApiController
    {
        CatalogoStock GestionStock = new CatalogoStock();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        CatalogoCabeceraFactura GestionCabeceraFactura = new CatalogoCabeceraFactura();

        [HttpPost]
        [Route("api/Factura/ListarStock")]
        public object ListarStock([FromBody] Tokens Tokens)
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
                        respuesta = GestionStock.ListarStock();
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
        [Route("api/Stock/ListaAsignarProductoKitEnStock")]
        public object ListaAsignarProductoKitEnStock(Kit Kit)
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
                        if (Kit.IdKit == null || string.IsNullOrEmpty(Kit.IdKit.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id kit";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            Kit.IdKit = Seguridad.DesEncriptar(Kit.IdKit);
                            respuesta = GestionStock.ListarProductosDeUnKitEnEstock(int.Parse(Kit.IdKit));
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


    }
}
