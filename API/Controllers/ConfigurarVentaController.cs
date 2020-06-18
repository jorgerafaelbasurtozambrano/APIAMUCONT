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
        CatalogoConfiguracionInteres GestionInteres = new CatalogoConfiguracionInteres();
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
                if (ConfigurarVenta == null)
                {
                    mensaje = "El dato a registrar llego vacio";
                    codigo = "418";
                }
                else if (ConfigurarVenta.IdCabeceraFactura == null || string.IsNullOrEmpty(ConfigurarVenta.IdCabeceraFactura))
                {
                    mensaje = "Ingrese el id de la factura";
                    codigo = "418";
                }
                else if (ConfigurarVenta.IdPersona == null || string.IsNullOrEmpty(ConfigurarVenta.IdPersona))
                {
                    mensaje = "No a seleccionado ni una persona";
                    codigo = "418";
                }
                else if(ConfigurarVenta.Efectivo == null || string.IsNullOrEmpty(ConfigurarVenta.Efectivo))
                {
                    mensaje = "Ingrese tipo de venta";
                    codigo = "418";
                }
                else
                {
                    ConfigurarVenta _DatoAGuardar = new ConfigurarVenta();
                    ConfigurarVenta.IdPersona = Seguridad.DesEncriptar(ConfigurarVenta.IdPersona);
                    ConfigurarVenta.IdCabeceraFactura = Seguridad.DesEncriptar(ConfigurarVenta.IdCabeceraFactura);
                    ConfigurarVenta DataVenta = new ConfigurarVenta();
                    DataVenta = GestionConfigurarVenta.ConsultarConfigurarVentaPorFactura(int.Parse(ConfigurarVenta.IdCabeceraFactura));
                    if (DataVenta.IdConfigurarVenta == null)
                    {
                        ConfigurarVenta.IdConfiguracionInteres = null;
                        if (ConfigurarVenta.Efectivo == "0")
                        {
                            ConfigurarVenta.EstadoConfVenta = "0";
                            ConfigurarVenta.AplicaSeguro = "1";
                            if (ConfigurarVenta.FechaFinalCredito == null || ConfigurarVenta.FechaFinalCredito.ToString() == "01/01/0001")
                            {
                                mensaje = "Ingrese La fecha de Fin de Credito";
                                codigo = "418";
                            }
                            else
                            {
                                ConfiguracionInteres DatoInteres = new ConfiguracionInteres();
                                DatoInteres = GestionInteres.ConsultarConfiguracionInteresActivo().FirstOrDefault();
                                if (DatoInteres == null)
                                {
                                    mensaje = "No existe ningun interes activo, por favor active";
                                    codigo = "418";
                                }
                                else
                                {
                                    ConfigurarVenta.IdConfiguracionInteres = Seguridad.DesEncriptar(DatoInteres.IdConfiguracionInteres);
                                    _DatoAGuardar = GestionConfigurarVenta.InsertarConfigurarVenta(ConfigurarVenta);
                                    if (_DatoAGuardar.IdConfigurarVenta == null)
                                    {
                                        mensaje = "Ocurrio Un Error Al guardar el registro";
                                        codigo = "418";
                                    }
                                    else
                                    {
                                        respuesta = _DatoAGuardar;
                                        codigo = "200";
                                        mensaje = "EXITO";
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ConfigurarVenta.FechaFinalCredito = null;
                            ConfigurarVenta.EstadoConfVenta = "1";
                            ConfigurarVenta.IdConfiguracionInteres = null;
                            _DatoAGuardar = GestionConfigurarVenta.InsertarConfigurarVenta(ConfigurarVenta);
                            if (_DatoAGuardar.IdConfigurarVenta == null)
                            {
                                mensaje = "Ocurrio Un Error Al guardar el registro";
                                codigo = "418";
                            }
                            else
                            {
                                respuesta = _DatoAGuardar;
                                codigo = "200";
                                mensaje = "EXITO";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                    else
                    {
                        mensaje = "Ya existe una configuracion venta para esta factura";
                        codigo = "418";
                    }
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje };
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
