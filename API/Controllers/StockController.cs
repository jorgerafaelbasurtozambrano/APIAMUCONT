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
using Negocio.Logica.Inventario;

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
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveGet = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClaveGetEncripBD = p.desencriptar(Tokens.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                //respuesta = GestionCabeceraFactura.ListarStock();
                respuesta = GestionStock.ListarStock();
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
