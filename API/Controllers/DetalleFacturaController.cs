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
                if (DetalleFactura.encriptada == null || string.IsNullOrEmpty(DetalleFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleFactura.IdCabeceraFactura == null || string.IsNullOrEmpty(DetalleFactura.IdCabeceraFactura.Trim()))
                        {
                            mensaje = "Por favor ingrese el id de la cabecera factura";
                            codigo = "418";
                        }
                        else if (DetalleFactura.IdAsignarProductoLote == null || string.IsNullOrEmpty(DetalleFactura.IdAsignarProductoLote.Trim()))
                        {
                            mensaje = "Por favor ingrese el id asignar producto lote";
                            codigo = "418";
                        }
                        else if (DetalleFactura.Cantidad == null || string.IsNullOrEmpty(DetalleFactura.Cantidad.ToString().Trim()))
                        {
                            mensaje = "Por favor ingrese la cantidad";
                            codigo = "418";
                        }
                        else
                        {
                            DetalleFactura.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleFactura.IdCabeceraFactura);
                            DetalleFactura.IdAsignarProductoLote = Seguridad.DesEncriptar(DetalleFactura.IdAsignarProductoLote);
                            DetalleFactura DataDetalleFactura = new DetalleFactura();
                            DataDetalleFactura = GestionDetalleFactura.InsertarDetalleFactura(DetalleFactura);
                            if (DataDetalleFactura.IdDetalleFactura == null || string.IsNullOrEmpty(DataDetalleFactura.IdDetalleFactura.Trim()))
                            {
                                mensaje = "Ocurrio un error al agregar el detalle";
                                codigo = "500";
                            }
                            else
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                                respuesta = DataDetalleFactura;
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
        [Route("api/Factura/AumentarDetalleFactura")]
        public object AumentarDetalleFactura(DetalleFactura DetalleFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleFactura.encriptada == null || string.IsNullOrEmpty(DetalleFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleFactura.IdDetalleFactura == null || string.IsNullOrEmpty(DetalleFactura.IdDetalleFactura.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id detalle de la factura";
                        }
                        else if (DetalleFactura.Cantidad == null || string.IsNullOrEmpty(DetalleFactura.Cantidad.ToString().Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese la cantidad a aumentar";
                        }
                        else
                        {
                            if (DetalleFactura.Cantidad <= 0)
                            {
                                codigo = "418";
                                mensaje = "La cantidad no puede ser menor o igual a cero";
                            }
                            else
                            {
                                DetalleFactura.IdDetalleFactura = Seguridad.DesEncriptar(DetalleFactura.IdDetalleFactura);
                                DetalleFactura DataDetalleFactura = new DetalleFactura();
                                DataDetalleFactura = GestionDetalleFactura.ConsultarDetalleFacturaCompraPorId(int.Parse(DetalleFactura.IdDetalleFactura.Trim())).FirstOrDefault();
                                if (DataDetalleFactura == null)
                                {
                                    codigo = "500";
                                    mensaje = "El detalle que intenta aumentar no existe";
                                }
                                else
                                {
                                    if (GestionDetalleFactura.AumentarDetalle(int.Parse(DetalleFactura.IdDetalleFactura), DetalleFactura.Cantidad) == true)
                                    {
                                        codigo = "200";
                                        mensaje = "EXITO";
                                    }
                                    else
                                    {
                                        codigo = "200";
                                        mensaje = "Ocurrio un error al intentar aumentar el detalle";
                                    }
                                }
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
        [Route("api/Factura/EliminarDetalleFactura")]
        public object EliminarDetalleFactura(DetalleFactura DetalleFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleFactura.encriptada == null || string.IsNullOrEmpty(DetalleFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleFactura.IdDetalleFactura == null || string.IsNullOrEmpty(DetalleFactura.IdDetalleFactura.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Por favor ingrese el id del detalle de la factura a eliminar";
                        }
                        else
                        {
                            DetalleFactura.IdDetalleFactura = Seguridad.DesEncriptar(DetalleFactura.IdDetalleFactura);
                            DetalleFacturaVenta DataDetalleFacturaVenta = new DetalleFacturaVenta();
                            DataDetalleFacturaVenta = GestionDetalleFactura.ConsultarDetalleFacturaPorId(int.Parse(DetalleFactura.IdDetalleFactura)).FirstOrDefault();
                            if (DataDetalleFacturaVenta == null)
                            {
                                codigo = "500";
                                mensaje = "El detalle de la factura a eliminar no existe";
                            }
                            else
                            {
                                if (DataDetalleFacturaVenta.CabeceraFactura.Finalizado == true)
                                {
                                    codigo = "500";
                                    mensaje = "No se puede eliminar el detalle porque la factura ya esta finalizada";
                                }
                                else
                                {
                                    if (GestionDetalleFactura.EliminarDetalleFactura(DataDetalleFacturaVenta) == true)
                                    {
                                        int cantidadDetalle = GestionDetalleFactura.CantidadDetalleFactura(int.Parse(Seguridad.DesEncriptar(DataDetalleFacturaVenta.CabeceraFactura.IdCabeceraFactura)));
                                        if (cantidadDetalle == 0)
                                        {
                                            if (GestionDetalleFactura.EliminarCabeceraFactura(int.Parse(Seguridad.DesEncriptar(DataDetalleFacturaVenta.CabeceraFactura.IdCabeceraFactura))) == true)
                                            {
                                                mensaje = "EXITO";
                                                codigo = "201";
                                            }
                                            else
                                            {
                                                mensaje = "Se elimino el detalle pero hubo problema al eliminar la cabecera";
                                                codigo = "500";
                                            }
                                        }
                                        else
                                        {
                                            mensaje = "EXITO";
                                            codigo = "200";
                                        }
                                    }
                                    else
                                    {
                                        mensaje = "Ocurrio error al intentar eliminar el detalle";
                                        codigo = "500";
                                    }
                                }
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


    }
}
