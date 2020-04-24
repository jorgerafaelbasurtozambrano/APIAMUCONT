using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio;
using Negocio.Logica.Credito;
namespace API.Controllers
{
    public class ConfigurarVentaController : ApiController
    {
        CatalogoConfigurarVenta GestionConfigurarVenta = new CatalogoConfigurarVenta();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Factura/IngresoConfigurarVenta")]
        public object IngresoConfigurarVenta(ConfigurarVenta ConfigurarVenta)
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
                string ClavePutEncripBD = p.desencriptar(ConfigurarVenta.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                ConfigurarVenta.IdCabeceraFactura = Seguridad.DesEncriptar(ConfigurarVenta.IdCabeceraFactura);
                ConfigurarVenta.IdPersona = Seguridad.DesEncriptar(ConfigurarVenta.IdPersona);
                if (ConfigurarVenta.IdSembrio != null)
                {
                    ConfigurarVenta.IdSembrio = Seguridad.DesEncriptar(ConfigurarVenta.IdSembrio);
                }
                ConfigurarVenta.IdConfiguracionInteres = Seguridad.DesEncriptar(ConfigurarVenta.IdConfiguracionInteres);
                respuesta = GestionConfigurarVenta.InsertarConfigurarVenta(ConfigurarVenta);
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
