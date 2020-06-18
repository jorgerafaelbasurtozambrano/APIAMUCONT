using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Inventario;
using Negocio;
using Negocio.Logica.Seguridad; 
namespace API.Controllers
{
    public class CabeceraFacturaController : ApiController
    {
        CatalogoCabeceraFactura GestionCabeceraFactura = new CatalogoCabeceraFactura();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        [HttpPost]
        [Route("api/Factura/IngresoCabeceraFactura")]
        public object IngresoCabeceraFactura(CabeceraFactura CabeceraFactura)
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
                string ClavePutEncripBD = p.desencriptar(CabeceraFactura.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (CabeceraFactura.IdTipoTransaccion == null || string.IsNullOrEmpty(CabeceraFactura.IdTipoTransaccion.Trim()))
                {
                    mensaje = "Se necesita el id del tipo de transaccion";
                    codigo = "418";
                }
                else if (CabeceraFactura.IdAsignacionTU == null || string.IsNullOrEmpty(CabeceraFactura.IdAsignacionTU.Trim()))
                {
                    mensaje = "Se necesita el id de la la persona quien realiza la factura";
                    codigo = "418";
                }
                else
                {
                    CabeceraFactura.IdTipoTransaccion = Seguridad.DesEncriptar(CabeceraFactura.IdTipoTransaccion);
                    CabeceraFactura.IdAsignacionTU = Seguridad.DesEncriptar(CabeceraFactura.IdAsignacionTU);
                    CabeceraFactura DatoCabeceraFactura = new CabeceraFactura();
                    DatoCabeceraFactura = GestionCabeceraFactura.InsertarCabeceraFactura(CabeceraFactura);
                    if (DatoCabeceraFactura.IdCabeceraFactura == null || string.IsNullOrEmpty(DatoCabeceraFactura.IdCabeceraFactura.Trim()))
                    {
                        mensaje = "Ocurrio un error al intentar crear la factura";
                        codigo = "500";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = DatoCabeceraFactura;
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
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
        [Route("api/Factura/AnularCabeceraFactura")]
        public object AnularCabeceraFactura(CabeceraFactura CabeceraFactura)
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
                string ClavePutEncripBD = p.desencriptar(CabeceraFactura.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                CabeceraFactura.IdAsignacionTU = Seguridad.DesEncriptar(CabeceraFactura.IdAsignacionTU);
                respuesta = GestionCabeceraFactura.AnularFactura(CabeceraFactura);

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
        [Route("api/Factura/FinalizarCabeceraFactura")]
        public object FinalizarCabeceraFactura(CabeceraFactura CabeceraFactura)
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
                string ClavePutEncripBD = p.desencriptar(CabeceraFactura.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (CabeceraFactura.IdCabeceraFactura == null || string.IsNullOrEmpty(CabeceraFactura.IdCabeceraFactura.Trim()))
                {
                    mensaje = "Ingrese el id de la factura a finalizar";
                    codigo = "418";
                }
                else
                {
                    CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura.Trim());
                    if (GestionCabeceraFactura.FinalizarCabeceraFactura(int.Parse(CabeceraFactura.IdCabeceraFactura.Trim())) == true)
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                    }
                    else
                    {
                        mensaje = "Ocurrio un error al intentar finalizar la factura";
                        codigo = "500";
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
        [Route("api/Factura/ListaFacturasFinalizadas")]
        public object ListaFacturasFinalizadas([FromBody] Tokens Tokens)
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
                respuesta = GestionCabeceraFactura.ListarCabeceraFacturaFinalizadas();
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
        [Route("api/Factura/ListaFacturasNoFinalizadas")]
        public object ListaFacturasNoFinalizadas([FromBody] Tokens Tokens)
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
                respuesta = GestionCabeceraFactura.ListarCabeceraFacturaNoFinalizadas();
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
        [Route("api/Factura/ListaFacturaDetalle")]
        public object ListaFacturaDetalle(CabeceraFactura CabeceraFactura)
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
                string ClaveGetEncripBD = p.desencriptar(CabeceraFactura.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                respuesta = GestionCabeceraFactura.ConsultarFactura(CabeceraFactura.IdCabeceraFactura);
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
        [Route("api/Factura/ListaFacturasFinalizadasVenta")]
        public object ListaFacturasFinalizadasVenta([FromBody] Tokens Tokens)
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
                respuesta = GestionCabeceraFactura.ListarCabeceraFacturaVentaFinalizadas();
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
        [Route("api/Factura/FacturasNoFinalizadasVenta")]
        public object FacturasNoFinalizadasVenta([FromBody] Tokens Tokens)
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
                respuesta = GestionCabeceraFactura.FacturaVentaNoFinalizadas();
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
                mensaje = e.Message;
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Factura/ListaFacturaVenta")]
        public object ListaFacturaVenta(CabeceraFactura CabeceraFactura)
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
                string ClaveGetEncripBD = p.desencriptar(CabeceraFactura.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                var lista = GestionCabeceraFactura.ListarCabeceraFacturaVentaNoFinalizadas(int.Parse(CabeceraFactura.IdCabeceraFactura)).FirstOrDefault();
                respuesta = lista;
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
        [Route("api/Factura/FinalizarCabeceraFacturaVenta")]
        public object FinalizarCabeceraFacturaVenta(CabeceraFactura CabeceraFactura)
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
                string ClavePutEncripBD = p.desencriptar(CabeceraFactura.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (CabeceraFactura.IdCabeceraFactura == null)
                {
                    mensaje = "Se necesita el id de la factura a finalizar";
                    codigo = "418";
                }
                else
                {
                    ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
                    CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                    _ConfigurarVenta = GestionCabeceraFactura.ConsultarConfigurarVentaPorFactura(int.Parse(CabeceraFactura.IdCabeceraFactura));
                    if (_ConfigurarVenta.IdConfigurarVenta == null)
                    {
                        mensaje = "No se a asignado la factura a ningun cliente";
                        codigo = "418";
                    }
                    else
                    {
                        CabeceraFactura Factura = new CabeceraFactura();
                        Factura = GestionCabeceraFactura.ListarCabeceraFacturaVentaNoFinalizadas(int.Parse(CabeceraFactura.IdCabeceraFactura)).FirstOrDefault();
                        if (Factura == null)
                        {
                            mensaje = "La factura que intenta finalizar no existe";
                            codigo = "500";
                        }
                        else if(Factura.DetalleVenta.Count == 0)
                        {
                            mensaje = "No se puede finalizar la factura porque no contiene ningun detalle";
                            codigo = "500";
                        }
                        else if (Factura.DetalleVenta.Where(p=>p.PermitirVender == false).ToList().Count > 0)
                        {
                            mensaje = "No se puede finalizar la factura porque existe producto dentro de la factura que no hay disponible";
                            codigo = "500";
                        }
                        else
                        {
                            if (GestionCabeceraFactura.FinalizarFacturaVenta(Factura) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                mensaje = "Ocurrio un error al intentar finalizar la factura";
                                codigo = "500";
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [Route("api/Credito/ConsultarFacturasPendientesPorPersona")]
        public object ConsultarFacturasPendientesPorPersona(Persona _Persona)
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
                string ClaveGetEncripBD = p.desencriptar(_Persona.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_Persona.NumeroDocumento == null || string.IsNullOrEmpty(_Persona.NumeroDocumento.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el numero de documento de la persona";
                }
                else
                {
                    respuesta = GestionCabeceraFactura.FacturasPendientePorPagarPorPersona(_Persona.NumeroDocumento.Trim());
                    mensaje = "EXITO";
                    codigo = "200";
                    objeto = new { codigo, mensaje,respuesta};
                    return objeto;
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
    }
}
