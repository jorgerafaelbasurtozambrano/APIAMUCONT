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
                if (ConfigurarVenta.IdCabeceraFactura != null)
                {
                    ConfigurarVenta.IdSembrio = ConfigurarVenta.IdSembrio.Replace("null", null);
                    if (ConfigurarVenta.IdSembrio.Replace("null",null)!="")
                    {
                        ConfigurarVenta.IdSembrio = Seguridad.DesEncriptar(ConfigurarVenta.IdSembrio);
                    }
                    ConfigurarVenta DataVenta = new ConfigurarVenta();
                    ConfigurarVenta.IdCabeceraFactura = Seguridad.DesEncriptar(ConfigurarVenta.IdCabeceraFactura);
                    DataVenta = GestionConfigurarVenta.ConsultarConfigurarVentaPorFactura(int.Parse(ConfigurarVenta.IdCabeceraFactura));
                    string dato = ConfigurarVenta.IdSembrio.ToString();
                    if (DataVenta.IdConfigurarVenta == null)
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        //ConfigurarVenta.IdCabeceraFactura = Seguridad.DesEncriptar(ConfigurarVenta.IdCabeceraFactura);
                        ConfigurarVenta.IdPersona = Seguridad.DesEncriptar(ConfigurarVenta.IdPersona);
                        //ConfigurarVenta.IdSembrio = Seguridad.DesEncriptar(ConfigurarVenta.IdSembrio);
                        if (ConfigurarVenta.IdConfiguracionInteres != null)
                        {
                            ConfigurarVenta.IdConfiguracionInteres = Seguridad.DesEncriptar(ConfigurarVenta.IdConfiguracionInteres);
                        }
                        respuesta = GestionConfigurarVenta.InsertarConfigurarVenta(ConfigurarVenta);
                        string Trasnformar = respuesta.ToString();
                        if (Trasnformar != "Negocio.Entidades.ConfigurarVenta")
                        {
                            mensaje = "ERROR";
                            codigo = "418";
                            objeto = new { codigo, mensaje };
                            return objeto;
                        }
                        else
                        {
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        mensaje = "La Factura ya se la asignado un cliente";
                        codigo = "418";
                        objeto = new { codigo, mensaje };
                        return objeto;
                    }
                }
                else
                {
                    mensaje = "Ingrese el id cabecera factura";
                    codigo = "418";
                    objeto = new { codigo, mensaje };
                    return objeto;
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
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
