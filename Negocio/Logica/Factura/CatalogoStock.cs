using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Inventario;
using Negocio.Logica.Credito;
namespace Negocio.Logica.Factura
{
    class StockProducto
    {
        public bool Estado { get; set; }
        public int Cantidad { get; set; }
    }
    public class CatalogoStock
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        CatalogoAsignarProductoKit GestionApk = new CatalogoAsignarProductoKit();
        //CatalogoDetalleVenta GestionDetalleVenta = new CatalogoDetalleVenta();
        List<Stock> ListaStock;
        public bool IngresarStock(Stock Stock)
        {
            try
            {
                ConexionBD.sp_CrearStock(Stock.Cantidad, int.Parse(Stock.IdAsignarProductoLote));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CargarStock()
        {
            ListaStock = new List<Stock>();
            var ListaAsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            foreach (var item in ConexionBD.sp_ConsultarStock())
            {
                ListaStock.Add(new Stock()
                {
                    IdStock = Seguridad.Encriptar(item.IdStock.ToString()),
                    Cantidad = item.Cantidad,
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    FechaActualizacion = item.FechaActualizacion,
                    AsignarProductoLote = ListaAsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == item.IdAsignarProductoLote.ToString()).FirstOrDefault(),
                    //AsignarProductoLote = GestionAsignarProductoLote.ConsultarDatosAsignarProductoLote(item.IdAsignarProductoLote.ToString()),
                });
            }
        }
        public List<Stock> ListarStock()
        {
            CargarStock();
            List<Stock> ListaUnida = new List<Stock>();
            List<Stock> Lista1 = new List<Stock>();
            List<Stock> Lista2 = new List<Stock>();
            Lista1 = ListaStock.Where(p =>  p.AsignarProductoLote.IdLote!= "" && p.AsignarProductoLote.Lote.Estado == true).ToList();
            Lista2 = ListaStock.Where(p => p.Cantidad > 0 == true && p.AsignarProductoLote.IdLote=="").ToList();
            ListaUnida = Lista1.Union(Lista2).ToList();
            return ListaUnida;
            //return ListaStock.Where(p=> p.AsignarProductoLote.Lote.Estado == true || p.Cantidad >=0).ToList();
        }
        public List<Stock> ListarStockPorIdAsignarProductoLote(int idAsignarProductoLote)
        {
            List<Stock> _DatosStock = new List<Stock>();
            foreach (var item in ConexionBD.sp_BuscarStockPorAsignarProductoLote(idAsignarProductoLote).Where(p=>p.Cantidad > 0))
            {
                _DatosStock.Add(new Stock()
                {
                    IdStock = Seguridad.Encriptar(item.IdStock.ToString()),
                    Cantidad = item.Cantidad,
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    FechaActualizacion = item.FechaActualizacion,
                    AsignarProductoLote = GestionAsignarProductoLote.filtrarAsignarProductoLote(item.IdAsignarProductoLote).FirstOrDefault()
                });
            }
            List<Stock> ListaUnida = new List<Stock>();
            List<Stock> Lista1 = new List<Stock>();
            List<Stock> Lista2 = new List<Stock>();
            Lista1 = _DatosStock.Where(p => p.AsignarProductoLote.IdLote != "" && p.AsignarProductoLote.Lote.Estado == true).ToList();
            Lista2 = _DatosStock.Where(p => p.Cantidad > 0 == true && p.AsignarProductoLote.IdLote == "").ToList();
            ListaUnida = Lista1.Union(Lista2).ToList();
            return ListaUnida;
        }
        public List<AsignarProductosKits> ListarProductosDeUnKitEnEstock(int IdKit)
        {
            List<AsignarProductosKits> ListaAsignarProductoKit = new List<AsignarProductosKits>();
            List<StockProducto> Disponible = new List<StockProducto>();
            var listaStock = ListarStock();
            ListaAsignarProductoKit = GestionApk.ListarProductosDeUnKit(IdKit);
            for (int i = 0; i < ListaAsignarProductoKit[0].ListaAsignarProductoKit.Count; i++)
            {
                var ProductoEnStock = listaStock.Where(p => Seguridad.DesEncriptar(p.AsignarProductoLote.IdRelacionLogica) == Seguridad.DesEncriptar(ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].IdAsignarProductoKit) && p.Cantidad > 0).FirstOrDefault();
                if (ProductoEnStock == null)
                {
                    Disponible.Add(new StockProducto()
                    {
                        Estado = false,
                        Cantidad = 0
                    });
                }
                else
                {
                    ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].Stock = ProductoEnStock;
                    ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].Stock.AsignarProductoLote.ConfigurarProductos = null;
                    ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].Stock.AsignarProductoLote.AsignarProductoKit = null;
                    Disponible.Add(new StockProducto()
                    {
                        Estado = true,
                        Cantidad = ProductoEnStock.Cantidad
                    });
                }
            }
            if (Disponible.Count(p=>p.Estado == false) == 0)
            {
                ListaAsignarProductoKit[0].PermitirAnadir = true;
            }
            else
            {
                ListaAsignarProductoKit[0].PermitirAnadir = false;
            }
            ListaAsignarProductoKit[0].CantidadMaxima = Disponible.Min(p=>p.Cantidad);
            return ListaAsignarProductoKit;
        }
        public List<PrecioConfigurarProducto> CargarDatosPrecioPorProducto()
        {
            List<PrecioConfigurarProducto> ListaPrecioConfigurarProducto = new List<PrecioConfigurarProducto>();
            foreach (var item in ConexionBD.sp_ConsultarPrecioConfiguracionProducto().Where(p=>p.Estado == true).ToList())
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
            return ListaPrecioConfigurarProducto;
        }

        public string IngresoDetalleVentaPorKit(DetalleVenta _DetalleVenta)
        {
            List<AsignarProductosKits> ListaAsignarProductoKit = new List<AsignarProductosKits>();
            List<StockProducto> Disponible = new List<StockProducto>();
            var listaStock = ListarStock();
            ListaAsignarProductoKit = GestionApk.ListarProductosDeUnKit(int.Parse(_DetalleVenta.IdKit));
            for (int i = 0; i < ListaAsignarProductoKit[0].ListaAsignarProductoKit.Count; i++)
            {
                var ProductoEnStock = listaStock.Where(p => Seguridad.DesEncriptar(p.AsignarProductoLote.IdRelacionLogica) == Seguridad.DesEncriptar(ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].IdAsignarProductoKit) && p.Cantidad > 0).FirstOrDefault();
                if (ProductoEnStock == null)
                {
                    Disponible.Add(new StockProducto()
                    {
                        Estado = false,
                        Cantidad = 0
                    });
                }
                else
                {
                    ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].Stock = ProductoEnStock;
                    Disponible.Add(new StockProducto()
                    {
                        Estado = true,
                        Cantidad = ProductoEnStock.Cantidad
                    });
                }
            }
            if (Disponible.Count(p => p.Estado == false) == 0)
            {
                ListaAsignarProductoKit[0].PermitirAnadir = true;
            }
            else
            {
                ListaAsignarProductoKit[0].PermitirAnadir = false;
            }
            ListaAsignarProductoKit[0].CantidadMaxima = Disponible.Min(p => p.Cantidad);
            if(_DetalleVenta.Cantidad <= ListaAsignarProductoKit[0].CantidadMaxima)
            {
                List<PrecioConfigurarProducto> Precio = new List<PrecioConfigurarProducto>();
                Precio = CargarDatosPrecioPorProducto();
                for (int i = 0; i < ListaAsignarProductoKit[0].ListaAsignarProductoKit.Count; i++)
                {
                    PrecioConfigurarProducto dataPrecio = new PrecioConfigurarProducto();
                    dataPrecio = Precio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].Stock.AsignarProductoLote.AsignarProductoKit.IdConfigurarProducto)).FirstOrDefault();
                    InsertarDetalleVenta(new DetalleVenta()
                    {
                        IdCabeceraFactura = _DetalleVenta.IdCabeceraFactura,
                        IdAsignarProductoLote = Seguridad.DesEncriptar(ListaAsignarProductoKit[0].ListaAsignarProductoKit[i].Stock.AsignarProductoLote.IdAsignarProductoLote),
                        AplicaDescuento = "1",
                        Faltante = "0",
                        Cantidad = _DetalleVenta.Cantidad
                    }, dataPrecio.Precio);
                }
                //foreach (var item in ListaAsignarProductoKit[0].ListaAsignarProductoKit)
                //{
                //    PrecioConfigurarProducto dataPrecio = new PrecioConfigurarProducto();
                //    dataPrecio = Precio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(item.Stock.AsignarProductoLote.AsignarProductoKit.IdConfigurarProducto)).FirstOrDefault();
                //    InsertarDetalleVenta(new DetalleVenta()
                //    {
                //        IdCabeceraFactura = _DetalleVenta.IdCabeceraFactura,
                //        IdAsignarProductoLote = Seguridad.DesEncriptar(item.Stock.AsignarProductoLote.IdAsignarProductoLote),
                //        AplicaDescuento = "1",
                //        Faltante = "0",
                //        Cantidad = _DetalleVenta.Cantidad
                //    }, dataPrecio.Precio);
                //}
                return "true";
            }
            else
            {
                return "No Hay La Cantidad Solicitada De = " + _DetalleVenta.Cantidad.ToString();
            }
        }
        public string InsertarDetalleVenta(DetalleVenta DetalleVenta,decimal Precio)
        {
            try
            {
                var DataDetalleVenta = ListarDetalleVenta().Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == DetalleVenta.IdCabeceraFactura && Seguridad.DesEncriptar(p.IdAsignarProductoLote) == DetalleVenta.IdAsignarProductoLote && p.AplicaDescuento == "True" && p.PerteneceKitCompleto == true).FirstOrDefault();
                var Stock = ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == DetalleVenta.IdAsignarProductoLote).FirstOrDefault();
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
                            if (Stock.AsignarProductoLote.PerteneceKit == "True")
                            {
                                ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, Stock.AsignarProductoLote.AsignarProductoKit.Kit.AsignarDescuentoKit.Descuento.Porcentaje, Precio,"1", Stock.AsignarProductoLote.AsignarProductoKit.ListaProductos.Iva);
                            }
                            else
                            {
                                ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, null, Precio,"0", Stock.AsignarProductoLote.AsignarProductoKit.ListaProductos.Iva);
                            }
                            //ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad);
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
        public List<DetalleVenta> ListarDetalleVenta()
        {
            List<DetalleVenta> ListaDetalleVenta = new List<DetalleVenta>();
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
            return ListaDetalleVenta;
        }

    }
}
