using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica.Credito;
using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
namespace API.Controllers
{
    public class DetalleVentaController : ApiController
    {
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoDetalleVenta GestionDetalleVenta = new CatalogoDetalleVenta();

        [HttpPost]
        [Route("api/Credito/IngresoDetalleVenta")]
        public object IngresoDetalleVenta(DetalleVenta DetalleVenta)
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
                string ClavePutEncripBD = p.desencriptar(DetalleVenta.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                DetalleVenta.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleVenta.IdCabeceraFactura);
                DetalleVenta.IdAsignarProductoLote = Seguridad.DesEncriptar(DetalleVenta.IdAsignarProductoLote);

                respuesta = GestionDetalleVenta.InsertarDetalleVenta(DetalleVenta);
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
        [Route("api/Credito/AumentarDetalleVenta")]
        public object AumentarDetalleFactura(DetalleVenta DetalleVenta)
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
                string ClavePutEncripBD = p.desencriptar(DetalleVenta.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                DetalleVenta.IdDetalleVenta = Seguridad.DesEncriptar(DetalleVenta.IdDetalleVenta);
                respuesta = GestionDetalleVenta.AumentarDetalleVenta(DetalleVenta);
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
        [Route("api/Credito/EliminarDetalleVenta")]
        public object EliminarDetalleVenta(DetalleVenta DetalleVenta)
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
                string ClavePutEncripBD = p.desencriptar(DetalleVenta.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                DetalleVenta.IdDetalleVenta = Seguridad.DesEncriptar(DetalleVenta.IdDetalleVenta);
                respuesta = GestionDetalleVenta.EliminarDetalleVenta(int.Parse(DetalleVenta.IdDetalleVenta));
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


    }
}
