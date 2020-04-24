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
    public class DetalleFacturaController : ApiController
    {
        CatalogoDetalleFactura GestionDetalleFactura = new CatalogoDetalleFactura();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Factura/IngresoDetalleFactura")]
        public object IngresoDetalleFactura(DetalleFactura DetalleFactura)
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
                string ClavePutEncripBD = p.desencriptar(DetalleFactura.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                DetalleFactura.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleFactura.IdCabeceraFactura);
                DetalleFactura.IdAsignarProductoLote = Seguridad.DesEncriptar(DetalleFactura.IdAsignarProductoLote);
                respuesta = GestionDetalleFactura.InsertarDetalleFactura(DetalleFactura);
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
        [Route("api/Factura/AumentarDetalleFactura")]
        public object AumentarDetalleFactura(DetalleFactura DetalleFactura)
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
                string ClavePutEncripBD = p.desencriptar(DetalleFactura.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                DetalleFactura.IdDetalleFactura = Seguridad.DesEncriptar(DetalleFactura.IdDetalleFactura);
                respuesta = GestionDetalleFactura.AumentarDetalle(int.Parse(DetalleFactura.IdDetalleFactura),DetalleFactura.Cantidad);
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
        [Route("api/Factura/EliminarDetalleFactura")]
        public object EliminarDetalleFactura(DetalleFactura DetalleFactura)
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
                string ClavePutEncripBD = p.desencriptar(DetalleFactura.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                DetalleFactura.IdDetalleFactura = Seguridad.DesEncriptar(DetalleFactura.IdDetalleFactura);
                DetalleFactura.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleFactura.IdCabeceraFactura);
                respuesta = GestionDetalleFactura.EliminarDetalleFactura(int.Parse(DetalleFactura.IdDetalleFactura), DetalleFactura.IdCabeceraFactura);
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
