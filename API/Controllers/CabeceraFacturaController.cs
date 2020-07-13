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
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                    }
                }
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
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                        CabeceraFactura.IdAsignacionTU = Seguridad.DesEncriptar(CabeceraFactura.IdAsignacionTU);
                        respuesta = GestionCabeceraFactura.AnularFactura(CabeceraFactura);
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje};
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
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                    }
                }
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
                        respuesta = GestionCabeceraFactura.ListarCabeceraFacturaFinalizadas();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
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
        [Route("api/Factura/ListaFacturasNoFinalizadas")]
        public object ListaFacturasNoFinalizadas([FromBody] Tokens Tokens)
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
                        respuesta = GestionCabeceraFactura.ListarCabeceraFacturaNoFinalizadas();
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
        [Route("api/Factura/ListaFacturaDetalle")]
        public object ListaFacturaDetalle(CabeceraFactura CabeceraFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                        respuesta = GestionCabeceraFactura.ConsultarFactura(CabeceraFactura.IdCabeceraFactura);
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
        [Route("api/Factura/ListaFacturasFinalizadasVenta")]
        public object ListaFacturasFinalizadasVenta([FromBody] Tokens Tokens)
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
                        respuesta = GestionCabeceraFactura.ListarCabeceraFacturaVentaFinalizadas();
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
        [Route("api/Factura/FacturasNoFinalizadasVenta")]
        public object FacturasNoFinalizadasVenta([FromBody] Tokens Tokens)
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
                        respuesta = GestionCabeceraFactura.FacturaVentaNoFinalizadas();
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
        [Route("api/Factura/ListaFacturaVenta")]
        public object ListaFacturaVenta(CabeceraFactura CabeceraFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                        var lista = GestionCabeceraFactura.ListarCabeceraFacturaVentaNoFinalizadas(int.Parse(CabeceraFactura.IdCabeceraFactura)).FirstOrDefault();
                        respuesta = lista;
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
        [Route("api/Factura/FinalizarCabeceraFacturaVenta")]
        public object FinalizarCabeceraFacturaVenta(CabeceraFactura CabeceraFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                                else if (Factura.DetalleVenta.Count == 0)
                                {
                                    mensaje = "No se puede finalizar la factura porque no contiene ningun detalle";
                                    codigo = "500";
                                }
                                else if (Factura.DetalleVenta.Where(p => p.PermitirVender == false).ToList().Count > 0)
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
        [Route("api/Credito/ConsultarFacturasPendientesPorPersona")]
        public object ConsultarFacturasPendientesPorPersona(Persona _Persona)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Persona.encriptada == null || string.IsNullOrEmpty(_Persona.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Persona.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                }
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
        [Route("api/Factura/EliminarFactura")]
        public object EliminarFactura(CabeceraFactura CabeceraFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (CabeceraFactura.encriptada == null || string.IsNullOrEmpty(CabeceraFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(CabeceraFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (CabeceraFactura.IdCabeceraFactura == null)
                        {
                            mensaje = "Se necesita el id de la factura a finalizar";
                            codigo = "418";
                        }
                        else
                        {
                            CabeceraFactura.IdCabeceraFactura = Seguridad.DesEncriptar(CabeceraFactura.IdCabeceraFactura);
                            CabeceraFactura DatoFactura = new CabeceraFactura();
                            DatoFactura = GestionCabeceraFactura.ConsultarFacturaPorId(int.Parse(CabeceraFactura.IdCabeceraFactura)).FirstOrDefault();
                            if (DatoFactura == null)
                            {
                                codigo = "418";
                                mensaje = "La factura que desea eliminar no existe";
                            }
                            else
                            {
                                if (DatoFactura.Finalizado == true)
                                {
                                    codigo = "418";
                                    mensaje = "La factura que desea eliminar ya esta finalizada";
                                }
                                else
                                {
                                    DatoFactura.IdCabeceraFactura = Seguridad.DesEncriptar(DatoFactura.IdCabeceraFactura);
                                    DatoFactura.IdTipoTransaccion = Seguridad.DesEncriptar(DatoFactura.IdTipoTransaccion);
                                    if (GestionCabeceraFactura.EliminarFacturaPorId(DatoFactura) == true)
                                    {
                                        codigo = "200";
                                        mensaje = "EXITO";
                                    }
                                    else
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al tratar de eliminar la factura";
                                    }
                                }
                            }
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
