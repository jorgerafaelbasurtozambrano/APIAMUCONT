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
    public class LoteController : ApiController
    {
        CatalogoLote GestionLote = new CatalogoLote();
        CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Factura/IngresoLote")]
        public object IngresoLote(Lote Lote)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Lote.encriptada == null || string.IsNullOrEmpty(Lote.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Lote.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (Lote.Codigo == null || string.IsNullOrEmpty(Lote.Codigo.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el codigo del lote";
                        }
                        else if (Lote.FechaExpiracion == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese la fecha de expiracion";
                        }
                        else
                        {
                            Lote DatoLote = new Lote();
                            DatoLote = GestionLote.ConsultarLotePorCodigo(Lote.Codigo.Trim()).FirstOrDefault();
                            if (DatoLote == null)
                            {
                                DatoLote = new Lote();
                                DatoLote = GestionLote.IngresarLote(Lote);
                                if (DatoLote.IdLote == null || string.IsNullOrEmpty(DatoLote.IdLote.Trim()))
                                {
                                    mensaje = "Error la intentar guardar el lote";
                                    codigo = "500";
                                }
                                else
                                {
                                    mensaje = "EXITO";
                                    codigo = "200";
                                    respuesta = DatoLote;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                if (DatoLote.Estado == true)
                                {
                                    codigo = "500";
                                    mensaje = "El lote con el codigo " + DatoLote.Codigo + " ya existe";
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "El lote con el codigo " + DatoLote.Codigo + " ya existe y se encuentra deshabilitado";
                                }
                            }
                        }
                    }
                }
                objeto = new {codigo,mensaje};
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
        [Route("api/Factura/ListaLote")]
        public object ListaLote(AsignarProductoLote AsignarProductoLote)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignarProductoLote.encriptada == null || string.IsNullOrEmpty(AsignarProductoLote.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignarProductoLote.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                        AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                        respuesta = GestionLote.ListarLotes(int.Parse(AsignarProductoLote.IdCabeceraFactura), int.Parse(AsignarProductoLote.IdRelacionLogica), AsignarProductoLote.PerteneceKit);
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
        [Route("api/Factura/BuscarLote")]
        public object BuscarLote(Lote Lote)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Lote.encriptada == null || string.IsNullOrEmpty(Lote.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Lote.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        Lote.AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(Lote.AsignarProductoLote.IdRelacionLogica);
                        respuesta = GestionAsignarProductoLote.validarLoteExiste(Lote.Codigo, Lote.AsignarProductoLote.IdRelacionLogica, Lote.AsignarProductoLote.PerteneceKit);
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


    }
}
