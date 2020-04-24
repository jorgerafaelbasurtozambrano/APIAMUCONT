using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Factura;
using Negocio;
using Negocio.Logica.Seguridad;
namespace API.Controllers
{
    public class AsignarProductoLoteController : ApiController
    {
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        [HttpPost]
        [Route("api/Factura/IngresoAsignarProductoLote")]
        public object IngresoAsignarProductoLote(AsignarProductoLote AsignarProductoLote)
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
                string ClavePutEncripBD = p.desencriptar(AsignarProductoLote.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                if (AsignarProductoLote.IdLote!=null)
                {
                    AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                    AsignarProductoLote.IdLote = Seguridad.DesEncriptar(AsignarProductoLote.IdLote);
                }
                else
                {
                    AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                }
                respuesta = GestionAsignarProductoLote.InsertarAsignarProductoLote(AsignarProductoLote);
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
        [Route("api/Factura/BuscarInformacionDeUnDetalle")]
        public object BuscarInformacionDeUnDetalle(AsignarProductoLote AsignarProductoLote)
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
                string ClaveGetEncripBD = p.desencriptar(AsignarProductoLote.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
              
                if (AsignarProductoLote.IdCabeceraFactura == null || AsignarProductoLote.IdRelacionLogica == null)
                {
                    respuesta = "false";
                }
                else
                {
                    AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                    AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                    respuesta = GestionAsignarProductoLote.BuscarInformacionDetalle(AsignarProductoLote).FirstOrDefault();
                    if (respuesta == null)
                    {
                        respuesta = "false";
                    }
                }
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
