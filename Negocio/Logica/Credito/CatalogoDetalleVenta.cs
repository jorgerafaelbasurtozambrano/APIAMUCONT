using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.Factura;
namespace Negocio.Logica.Credito
{
    public class CatalogoDetalleVenta
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoStock GestionStock = new CatalogoStock();
        List<DetalleVenta> ListaDetalleVenta;
        public PrecioConfigurarProducto CargarDatosPrecioPorProducto(int IdConfiguracionProducto)
        {
            List<PrecioConfigurarProducto> ListaPrecioConfigurarProducto = new List<PrecioConfigurarProducto>();
            foreach (var item in ConexionBD.sp_ConsultarPrecioConfiguracionProducto().Where(p=>p.IdConfigurarProducto == IdConfiguracionProducto && p.Estado == true).ToList())
            {
                ListaPrecioConfigurarProducto.Add(new PrecioConfigurarProducto()
                {
                    IdPrecioConfigurarProducto = Seguridad.Encriptar(item.IdPrecioConfiguracionProducto.ToString()),
                    IdConfigurarProducto = Seguridad.Encriptar(item.IdConfigurarProducto.ToString()),
                    FechaRegistro = item.FechaRegistro,
                    Precio = item.Precio,
                    Estado = item.Estado.ToString()
                });
            }
            return ListaPrecioConfigurarProducto.FirstOrDefault();
        }
        public string InsertarDetalleVenta(DetalleVenta DetalleVenta)
        {
            try
            {
                //var DataDetalleVenta = ListarDetalleVenta().Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == DetalleVenta.IdCabeceraFactura && Seguridad.DesEncriptar(p.IdAsignarProductoLote) == DetalleVenta.IdAsignarProductoLote && p.AplicaDescuento == (DetalleVenta.AplicaDescuento == "1" ? "True": "False") && p.PerteneceKitCompleto == false).FirstOrDefault();
                var DataDetalleVenta = FiltrarDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote),DetalleVenta.AplicaDescuento, "0").FirstOrDefault();
                //var Stock = GestionStock.ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == DetalleVenta.IdAsignarProductoLote).FirstOrDefault();
                var Stock = GestionStock.ListarStockPorIdAsignarProductoLote(int.Parse(DetalleVenta.IdAsignarProductoLote)).FirstOrDefault();
                if (Stock == null)
                {
                    return "No hay stock de este producto";
                }
                else
                {
                    if (DetalleVenta.Cantidad <= Stock.Cantidad)
                    {
                        if (DataDetalleVenta == null)
                        {
                            PrecioConfigurarProducto Precio = new PrecioConfigurarProducto();
                            if (Stock.AsignarProductoLote.PerteneceKit == "True")
                            {
                                Precio = CargarDatosPrecioPorProducto(int.Parse(Seguridad.DesEncriptar(Stock.AsignarProductoLote.AsignarProductoKit.IdConfigurarProducto)));
                                if (DetalleVenta.AplicaDescuento == "1")
                                {
                                    ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad,DetalleVenta.PorcentajeDescuento, Precio.Precio, "0", Stock.AsignarProductoLote.AsignarProductoKit.ListaProductos.Iva);
                                    //Stock.AsignarProductoLote.AsignarProductoKit.Kit.AsignarDescuentoKit.Descuento.Porcentaje
                                }
                                else
                                {
                                    ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, null, Precio.Precio, "0", Stock.AsignarProductoLote.AsignarProductoKit.ListaProductos.Iva);
                                }
                            }
                            else
                            {
                                Precio = CargarDatosPrecioPorProducto(int.Parse(Seguridad.DesEncriptar(Stock.AsignarProductoLote.ConfigurarProductos.IdConfigurarProducto)));
                                if (DetalleVenta.AplicaDescuento == "1")
                                {
                                    ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad,DetalleVenta.PorcentajeDescuento, Precio.Precio, "0", Stock.AsignarProductoLote.ConfigurarProductos.Iva);
                                }
                                else
                                {
                                    ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, null, Precio.Precio, "0", Stock.AsignarProductoLote.ConfigurarProductos.Iva);
                                }
                            }
                        }
                        else
                        {
                            ConexionBD.sp_AumentarDetalleVenta(int.Parse(Seguridad.DesEncriptar(DataDetalleVenta.IdDetalleVenta)), DetalleVenta.Cantidad);
                        }
                        return "true";
                    }
                    else
                    {
                        return "No Hay esta cantidad disponible, existe en stock solo " + Stock.Cantidad.ToString() + " Unidades";
                    }
                }
            }
            catch (Exception)
            {
                return "false";
            }
        }
        public void CargarDatos()
        {
            ListaDetalleVenta = new List<DetalleVenta>();
            foreach (var item in ConexionBD.sp_ConsultarDetalleVenta())
            {
                ListaDetalleVenta.Add(new DetalleVenta()
                {
                    IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    Cantidad = item.Cantidad,
                    AplicaDescuento = item.AplicaDescuento.ToString(),
                    Faltante = item.Faltante.ToString(),
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                    ValorUnitario = item.PrecioUnitario,
                    PorcentajeDescuento = item.PorcentajeDescuento,
                    PerteneceKitCompleto = item.PerteneceKitCompleto
                });
            }
        }
        public List<DetalleVenta> ListarDetalleVenta()
        {
            CargarDatos();
            return ListaDetalleVenta;
        }
        public object AumentarDetalleVenta(DetalleVenta DetalleVenta)
        {
            try
            {
                var DataDetalleVenta = ListarDetalleVenta().Where(p => Seguridad.DesEncriptar(p.IdDetalleVenta) == DetalleVenta.IdDetalleVenta).FirstOrDefault();
                var Stock = GestionStock.ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(DataDetalleVenta.IdAsignarProductoLote)).FirstOrDefault();
                if (Stock == null)
                {
                    return "No hay stock de este producto";
                }
                else
                {
                    if (DetalleVenta.Cantidad <= Stock.Cantidad)
                    {
                        ConexionBD.sp_SetCantidadDetalleVenta(int.Parse(DetalleVenta.IdDetalleVenta), DetalleVenta.Cantidad);
                        return "true";
                    }
                    else
                    {
                        return "No Hay esta cantidad disponible, existe en stock solo " + Stock.Cantidad.ToString() + " Unidades";
                    }
                }
            }
            catch (Exception)
            {
                return "false";
            }
            
        }
        public bool EliminarDetalleVenta(int IdDetalleVenta)
        {
            try
            {
                ConexionBD.sp_EliminarDetalleVenta(IdDetalleVenta);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<DetalleVenta> FiltrarDetalleVenta(int idCabeceraFactura,int idAsignarProductoLote,string aplicaDescuento,string perteneceKitCompleto)
        {
            List<DetalleVenta> _DataDetalleVenta = new List<DetalleVenta>();
            foreach (var item in ConexionBD.sp_BuscarDetalleFacturaVenta(idCabeceraFactura,idAsignarProductoLote,aplicaDescuento,perteneceKitCompleto))
            {
                _DataDetalleVenta.Add(new DetalleVenta()
                {
                    IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    Cantidad = item.Cantidad,
                    AplicaDescuento = item.AplicaDescuento.ToString(),
                    Faltante = item.Faltante.ToString(),
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                    ValorUnitario = item.PrecioUnitario,
                    PorcentajeDescuento = item.PorcentajeDescuento,
                    PerteneceKitCompleto = item.PerteneceKitCompleto
                });
            }
            return _DataDetalleVenta;
        }
        public List<DetalleVenta> ListarDetalleVentaPorFactura(int idCabeceraFactura)
        {
            List<DetalleVenta> _DataDetalleVenta = new List<DetalleVenta>();
            foreach (var item in ConexionBD.sp_ConsultarDetalleVentaPorFactura(idCabeceraFactura))
            {
                _DataDetalleVenta.Add(new DetalleVenta()
                {
                    IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    Cantidad = item.Cantidad,
                    AplicaDescuento = item.AplicaDescuento.ToString(),
                    Faltante = item.Faltante.ToString(),
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                    ValorUnitario = item.PrecioUnitario,
                    PorcentajeDescuento = item.PorcentajeDescuento,
                    PerteneceKitCompleto = item.PerteneceKitCompleto,
                    Iva = item.Iva
                });
            }
            return _DataDetalleVenta;
        }
    }
}
