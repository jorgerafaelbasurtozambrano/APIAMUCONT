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
                if (AsignarProductoLote.IdCabeceraFactura == null || string.IsNullOrEmpty(AsignarProductoLote.IdCabeceraFactura.Trim()))
                {
                    mensaje = "Ingrese el id cabecera factura";
                    codigo = "418";
                }
                else if(AsignarProductoLote.IdRelacionLogica == null || string.IsNullOrEmpty(AsignarProductoLote.IdRelacionLogica.Trim()))
                {
                    mensaje = "Ingrese el id relacion logica";
                    codigo = "418";
                }
                else if(AsignarProductoLote.PerteneceKit == null || string.IsNullOrEmpty(AsignarProductoLote.PerteneceKit.Trim()))
                {
                    mensaje = "Ingrese si pertenece a kit o no";
                    codigo = "418";
                }
                else if(AsignarProductoLote.ValorUnitario == null || string.IsNullOrEmpty(AsignarProductoLote.ValorUnitario.ToString().Trim()))
                {
                    mensaje = "Ingrese el valor unitario";
                    codigo = "418";
                }
                else
                {
                    if (AsignarProductoLote.Cantidad <= 0)
                    {
                        mensaje = "Ingrese una cantidad valida";
                        codigo = "500";
                    }
                    else if(AsignarProductoLote.IdLote != null)
                    {
                        AsignarProductoLote.IdLote = Seguridad.DesEncriptar(AsignarProductoLote.IdLote);
                        Lote DatoLote = new Lote();
                        DatoLote = GestionAsignarProductoLote.ConsultarLotePorId(int.Parse(AsignarProductoLote.IdLote)).FirstOrDefault();
                        if (DatoLote == null)
                        {
                            mensaje = "El lote a asignar no existe";
                            codigo = "418";
                        }
                        else if(DatoLote.Estado == false)
                        {
                            mensaje = "El lote a asignar existe pero esta deshabilitado por lo tanto no lo puede asignar";
                            codigo = "500";
                        }
                        else if (AsignarProductoLote.FechaExpiracion == null)
                        {
                            mensaje = "Ingrese la fecha de expiracion";
                            codigo = "500";
                        }
                        else if(DatoLote.LoteUtilizado == "1")
                        {
                            AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                            AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                            AsignarProductoLote DataAsingarProductoLote = new AsignarProductoLote();
                            DataAsingarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote().Where(p => Seguridad.DesEncriptar(p.IdLote) == AsignarProductoLote.IdLote && Seguridad.DesEncriptar(p.IdRelacionLogica) == AsignarProductoLote.IdRelacionLogica && p.PerteneceKit == AsignarProductoLote.PerteneceKit && p.Lote.FechaExpiracion.ToString() == AsignarProductoLote.FechaExpiracion.ToString()).FirstOrDefault();

                            if (DataAsingarProductoLote == null)
                            {
                                AsignarProductoLote DatoAguarguadarConLote = new AsignarProductoLote();
                                DatoAguarguadarConLote = GestionAsignarProductoLote.CrearAsignarProductoLoteConLote(AsignarProductoLote);
                                if (DatoAguarguadarConLote.IdAsignarProductoLote == null)
                                {
                                    mensaje = "Ocurrio un error al crear el asignar prodcuto lote";
                                    codigo = "500";
                                }
                                else
                                {
                                    mensaje = "Exito";
                                    codigo = "200";
                                    respuesta = DatoAguarguadarConLote;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                DetalleFactura DataDetalleFactura = new DetalleFactura();
                                DataDetalleFactura = GestionAsignarProductoLote.ConsultarDetalleFacturaPorFacturaYAsignarProductoLote(new DetalleFactura() { IdCabeceraFactura = AsignarProductoLote.IdCabeceraFactura, IdAsignarProductoLote = Seguridad.DesEncriptar(DataAsingarProductoLote.IdAsignarProductoLote) }).FirstOrDefault();
                                if (DataDetalleFactura != null)
                                {
                                    DataDetalleFactura.Cantidad = DataDetalleFactura.Cantidad + AsignarProductoLote.Cantidad;
                                    if (GestionAsignarProductoLote.AumentarDetalleFactura(DataDetalleFactura) == true)
                                    {
                                        codigo = "201";
                                        mensaje = "Detalle VentaAumentado";
                                        objeto = new { codigo, mensaje};
                                        return objeto;
                                    }
                                    else
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al aumentar el detalle venta";
                                    }
                                }
                                else
                                {
                                    codigo = "200";
                                    mensaje = "Ya existe el asignar producto lote pero no a sido asignado en esta factura";
                                    respuesta = DataAsingarProductoLote;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                        }
                        else
                        {
                            AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                            AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                            AsignarProductoLote DatoAguarguadarConLote = new AsignarProductoLote();
                            DatoAguarguadarConLote = GestionAsignarProductoLote.CrearAsignarProductoLoteConLote(AsignarProductoLote);
                            if (DatoAguarguadarConLote.IdAsignarProductoLote == null)
                            {
                                mensaje = "Ocurrio un error al crear el asignar prodcuto lote";
                                codigo = "500";
                            }
                            else
                            {
                                mensaje = "Exito";
                                codigo = "200";
                                respuesta = DatoAguarguadarConLote;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                    else
                    {
                        AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                        AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                        AsignarProductoLote.PerteneceKit = Convert.ToBoolean(AsignarProductoLote.PerteneceKit).ToString();
                        AsignarProductoLote DatoAsignarProductoLote = new AsignarProductoLote();
                        DatoAsignarProductoLote = GestionAsignarProductoLote.ConsultarSiExisteAsignarProductoLote(AsignarProductoLote).FirstOrDefault();
                        if (DatoAsignarProductoLote == null)
                        {
                            DatoAsignarProductoLote = new AsignarProductoLote();
                            DatoAsignarProductoLote = GestionAsignarProductoLote.CrearAsignarProductoLote(AsignarProductoLote);
                            if (DatoAsignarProductoLote.IdAsignarProductoLote == null)
                            {
                                codigo = "500";
                                mensaje = "OCURRIO UN ERROR AL INTENTAR GUARAR EL ASIGNAR PRODUCTO LOTE";
                            }
                            else
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                                respuesta = DatoAsignarProductoLote;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                        else
                        {
                            DetalleFactura DataDetalleFactura = new DetalleFactura();
                            DataDetalleFactura = GestionAsignarProductoLote.ConsultarDetalleFacturaPorFacturaYAsignarProductoLote(new DetalleFactura() { IdCabeceraFactura = AsignarProductoLote.IdCabeceraFactura, IdAsignarProductoLote = Seguridad.DesEncriptar(DatoAsignarProductoLote.IdAsignarProductoLote) }).FirstOrDefault();
                            if (DataDetalleFactura != null)
                            {
                                DataDetalleFactura.Cantidad = DataDetalleFactura.Cantidad + AsignarProductoLote.Cantidad;
                                if (GestionAsignarProductoLote.AumentarDetalleFactura(DataDetalleFactura) == true)
                                {
                                    codigo = "201";
                                    mensaje = "Detalle VentaAumentado";
                                    objeto = new { codigo, mensaje};
                                    return objeto;
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al aumentar el detalle venta";
                                }
                            }
                            else
                            {
                                codigo = "200";
                                mensaje = "Ya existe el asignar producto lote pero no a sido asignado en esta factura";
                                respuesta = DatoAsignarProductoLote;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                        //AsignarProductoLote.IdLote = null;
                        //AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                        //AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                        //respuesta = GestionAsignarProductoLote.InsertarAsignarProductoLote(AsignarProductoLote);
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
                AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                AsignarProductoLote.IdRelacionLogica = Seguridad.DesEncriptar(AsignarProductoLote.IdRelacionLogica);
                //respuesta = GestionAsignarProductoLote.BuscarInformacionDetalle(AsignarProductoLote).FirstOrDefault();
                AsignarProductoLote DatoAsignarProductoLote = new AsignarProductoLote();
                DatoAsignarProductoLote = GestionAsignarProductoLote.ConsultarSiExisteAsignarProductoLote(AsignarProductoLote).FirstOrDefault();
                if (DatoAsignarProductoLote.IdAsignarProductoLote == null)
                {
                    mensaje = "No existe";
                    codigo = "404";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    respuesta = DatoAsignarProductoLote;
                    objeto = new { codigo, mensaje, respuesta };
                    return objeto;
                }
                //}
                //else
                //{
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
