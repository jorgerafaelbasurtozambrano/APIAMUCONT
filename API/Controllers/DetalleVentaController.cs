using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica.Credito;
using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio.Logica.Factura;
using Negocio.Logica.Inventario;
using Negocio.Entidades.DatoUsuarios;

namespace API.Controllers
{
    public class DetalleVentaController : ApiController
    {
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoDetalleVenta GestionDetalleVenta = new CatalogoDetalleVenta();
        CatalogoStock GestionStock = new CatalogoStock();
        CatalogoCabeceraFactura GestionCabeceraFactura = new CatalogoCabeceraFactura();
        [HttpPost]
        [Route("api/Credito/IngresoDetalleVenta")]
        public object IngresoDetalleVenta(DetalleVenta DetalleVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleVenta.encriptada == null || string.IsNullOrEmpty(DetalleVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleVenta.IdCabeceraFactura == null || string.IsNullOrEmpty(DetalleVenta.IdCabeceraFactura.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id cabecera factura";
                        }
                        else if (DetalleVenta.IdAsignarProductoLote == null || string.IsNullOrEmpty(DetalleVenta.IdAsignarProductoLote.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el producto a agregar";
                        }
                        else if (DetalleVenta.AplicaDescuento == null || string.IsNullOrEmpty(DetalleVenta.AplicaDescuento.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta si aplica o no descuento";
                        }
                        else if (DetalleVenta.Cantidad == null || string.IsNullOrEmpty(DetalleVenta.Cantidad.ToString().Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta la cantidad a comprar";
                        }
                        else if (DetalleVenta.Cantidad <= 0)
                        {
                            codigo = "400";
                            mensaje = "La cantidad no puede ser menor o igual a cero";
                        }
                        else
                        {
                            DetalleVenta.IdAsignarProductoLote = Seguridad.DesEncriptar(DetalleVenta.IdAsignarProductoLote);
                            DetalleVenta.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleVenta.IdCabeceraFactura);
                            Stock DatoStock = new Stock();
                            DatoStock = GestionDetalleVenta.ListarStockPorIdAsignarProductoLote(int.Parse(DetalleVenta.IdAsignarProductoLote)).FirstOrDefault();
                            if (DatoStock == null)
                            {
                                codigo = "500";
                                mensaje = "El producto que quiere insertar no existe";
                            }
                            else
                            {
                                DetalleVenta _ObjDetalleVenta = new DetalleVenta();
                                _ObjDetalleVenta = GestionDetalleVenta.FiltrarDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, "0").FirstOrDefault();
                                int? CantidadDetalle = DetalleVenta.Cantidad;
                                if (_ObjDetalleVenta != null)
                                {
                                    CantidadDetalle = CantidadDetalle + _ObjDetalleVenta.Cantidad;
                                }
                                if (CantidadDetalle > DatoStock.Cantidad)
                                {
                                    codigo = "500";
                                    mensaje = "No hay esta cantidad disponible, existe en stock solo " + DatoStock.Cantidad.ToString() + " unidades";
                                }
                                else
                                {
                                    if (DetalleVenta.AplicaDescuento == "1")
                                    {
                                        if (DetalleVenta.PorcentajeDescuento == null || string.IsNullOrEmpty(DetalleVenta.PorcentajeDescuento.ToString()))
                                        {
                                            codigo = "500";
                                            mensaje = "Por favor elija el porcentaje de descuento";
                                        }
                                        else
                                        {
                                            DetalleVenta DataDetalleVenta = new DetalleVenta();
                                            DataDetalleVenta = GestionDetalleVenta.InsertarDetalleVenta(DetalleVenta);
                                            if (DataDetalleVenta.IdDetalleVenta == null || string.IsNullOrEmpty(DataDetalleVenta.IdDetalleVenta))
                                            {
                                                codigo = "500";
                                                mensaje = "Ocurrio un error al intentar añadir el detalle";
                                            }
                                            else
                                            {
                                                codigo = "200";
                                                mensaje = "EXITO";
                                                respuesta = DataDetalleVenta;
                                                objeto = new { codigo, mensaje, respuesta };
                                                return objeto;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DetalleVenta DataDetalleVenta = new DetalleVenta();
                                        DataDetalleVenta = GestionDetalleVenta.InsertarDetalleVenta(DetalleVenta);
                                        if (DataDetalleVenta.IdDetalleVenta == null || string.IsNullOrEmpty(DataDetalleVenta.IdDetalleVenta))
                                        {
                                            codigo = "500";
                                            mensaje = "Ocurrio un error al intentar añadir el detalle";
                                        }
                                        else
                                        {
                                            codigo = "200";
                                            mensaje = "EXITO";
                                            respuesta = DataDetalleVenta;
                                            objeto = new { codigo, mensaje, respuesta };
                                            return objeto;
                                        }
                                    }
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
        [Route("api/Credito/AumentarDetalleVenta")]
        public object AumentarDetalleFactura(DetalleVenta DetalleVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleVenta.encriptada == null || string.IsNullOrEmpty(DetalleVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleVenta.IdDetalleVenta == null || string.IsNullOrEmpty(DetalleVenta.IdDetalleVenta.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id del detalle de la venta";
                        }
                        else if (DetalleVenta.Cantidad == null || string.IsNullOrEmpty(DetalleVenta.Cantidad.ToString().Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese la cantidad a aumentar";
                        }
                        else
                        {
                            if (DetalleVenta.Cantidad <= 0)
                            {
                                codigo = "500";
                                mensaje = "Ingrese una cantidad valida mayor a cero";
                            }
                            else
                            {
                                DetalleVenta.IdDetalleVenta = Seguridad.DesEncriptar(DetalleVenta.IdDetalleVenta);
                                DetalleVenta DataDetalleVenta = new DetalleVenta();
                                DataDetalleVenta = GestionDetalleVenta.ConsultarDetalleVentaPorId(int.Parse(DetalleVenta.IdDetalleVenta.Trim())).FirstOrDefault();
                                if (DataDetalleVenta == null)
                                {
                                    codigo = "500";
                                    mensaje = "El detalle de venta a aumentar no existe";
                                }
                                else
                                {
                                    Stock DataStock = new Stock();
                                    DataStock = GestionDetalleVenta.ListarStockPorIdAsignarProductoLote(int.Parse(Seguridad.DesEncriptar(DataDetalleVenta.IdAsignarProductoLote))).FirstOrDefault();
                                    if (DataStock == null)
                                    {
                                        codigo = "500";
                                        mensaje = "El producto que desea aumentar no existe";
                                    }
                                    else
                                    {
                                        if (DetalleVenta.Cantidad > DataStock.Cantidad)
                                        {
                                            codigo = "418";
                                            mensaje = "No Hay esta cantidad disponible, existe en stock solo " + DataStock.Cantidad.ToString() + " Unidades";
                                        }
                                        else
                                        {
                                            if (GestionDetalleVenta.AumentarDetalleVenta(DetalleVenta) == true)
                                            {
                                                codigo = "200";
                                                mensaje = "EXITO";
                                            }
                                            else
                                            {
                                                codigo = "200";
                                                mensaje = "EXITO";
                                            }
                                        }
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

        [HttpPost]
        [Route("api/Credito/EliminarDetalleVenta")]
        public object EliminarDetalleVenta(DetalleVenta DetalleVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleVenta.encriptada == null || string.IsNullOrEmpty(DetalleVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleVenta.IdDetalleVenta == null || string.IsNullOrEmpty(DetalleVenta.IdDetalleVenta.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Ingrese el id del detalle de la venta a eliminar";
                        }
                        else
                        {
                            DetalleVenta.IdDetalleVenta = Seguridad.DesEncriptar(DetalleVenta.IdDetalleVenta);
                            if (GestionDetalleVenta.EliminarDetalleVenta(int.Parse(DetalleVenta.IdDetalleVenta)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                mensaje = "Ocurrio un error al intentar eliminar el detalle";
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
        [Route("api/Credito/IngresoDetalleVentaPorKit")]
        public object IngresoDetalleVentaPorKit(DetalleVenta DetalleVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleVenta.encriptada == null || string.IsNullOrEmpty(DetalleVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleVenta.IdCabeceraFactura == null || string.IsNullOrEmpty(DetalleVenta.IdCabeceraFactura.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id cabecera factura";
                        }
                        else if (DetalleVenta.IdKit == null || string.IsNullOrEmpty(DetalleVenta.IdKit.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id del kit";
                        }
                        else if (DetalleVenta.Cantidad == null || string.IsNullOrEmpty(DetalleVenta.Cantidad.ToString().Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta la cantidad a agregar";
                        }
                        else
                        {
                            DetalleVenta.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleVenta.IdCabeceraFactura);
                            DetalleVenta.IdKit = Seguridad.DesEncriptar(DetalleVenta.IdKit);
                            AsignarProductosKits DatoKit = new AsignarProductosKits();
                            DatoKit = GestionStock.ConsultarAsginarProductokits(int.Parse(DetalleVenta.IdKit)).FirstOrDefault();
                            if (DatoKit == null)
                            {
                                codigo = "500";
                                mensaje = "El kit que desea ingresar no existe";
                            }
                            else
                            {
                                DetalleVenta DataDetalleVenta = new DetalleVenta();
                                DataDetalleVenta = GestionDetalleVenta.CargarKitDeUnaFactura(new DetalleVenta() { IdCabeceraFactura = DetalleVenta.IdCabeceraFactura, IdKit = DetalleVenta.IdKit }).FirstOrDefault();
                                int? cantidad = DetalleVenta.Cantidad;
                                if (DataDetalleVenta != null)
                                {
                                    cantidad = cantidad + DataDetalleVenta.Cantidad;
                                }
                                if (cantidad > DatoKit.CantidadMaxima)
                                {
                                    codigo = "500";
                                    mensaje = "Solo existe en stock la cantidad de " + DatoKit.CantidadMaxima.ToString() + " unidades";
                                }
                                else
                                {
                                    if (GestionStock.IngresoDetalleVentaPorKit(DetalleVenta) == true)
                                    {
                                        mensaje = "EXITO";
                                        codigo = "200";
                                    }
                                    else
                                    {
                                        mensaje = "Ocurrio un eror al intentar ingresar el kit";
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

        [HttpPost]
        [Route("api/Credito/EliminarDetalleVentaPorKitCompleto")]
        public object EliminarDetalleVentaPorKitCompleto(DetalleVenta DetalleVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (DetalleVenta.encriptada == null || string.IsNullOrEmpty(DetalleVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(DetalleVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (DetalleVenta.IdCabeceraFactura == null || string.IsNullOrEmpty(DetalleVenta.IdCabeceraFactura.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id cabecera factura";
                        }
                        else if (DetalleVenta.IdKit == null || string.IsNullOrEmpty(DetalleVenta.IdKit.Trim()))
                        {
                            codigo = "400";
                            mensaje = "Falta el id del kit";
                        }
                        else
                        {
                            DetalleVenta.IdKit = Seguridad.DesEncriptar(DetalleVenta.IdKit);
                            DetalleVenta.IdCabeceraFactura = Seguridad.DesEncriptar(DetalleVenta.IdCabeceraFactura);
                            if (GestionCabeceraFactura.ListarDetalleVentaPorKit(int.Parse(DetalleVenta.IdKit), int.Parse(DetalleVenta.IdCabeceraFactura)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                mensaje = "Ocurrio un error al eliminar el kit porfavor intentar de nuevo";
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

    }
}
