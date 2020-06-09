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
        public List<Stock> ListarStockPorIdAsignarProductoLote(int idAsignarProductoLote)
        {
            List<Stock> _DatosStock = new List<Stock>();
            foreach (var item in ConexionBD.sp_ConsultarStockPorIdAsignarProductoLote(idAsignarProductoLote))
            {
                List<ConfigurarProductos> ConfigurarProductos = new List<ConfigurarProductos>();
                List<AsignarProductoKit> AsignarProducoKit = new List<AsignarProductoKit>();
                if (item.AsignarProductoLotePerteneceKit == true)
                {
                    foreach (var item1 in ConexionBD.sp_ConsultarAsignarProductoKitPorId(item.AsignarProductoLoteIdRelacionLogica))
                    {
                        AsignarProducoKit.Add(new AsignarProductoKit()
                        {
                            IdAsignarProductoKit = Seguridad.Encriptar(item1.AsignarProductoKitIdAsignarProductoKit.ToString()),
                            IdConfigurarProducto = Seguridad.Encriptar(item1.AsignarProductoKitIdConfigurarProducto.ToString()),
                            IdAsignarDescuentoKit = Seguridad.Encriptar(item1.AsignarProductoKitIdAsignarDescuentoKit.ToString()),
                            FechaCreacion = item1.AsignarProductoKitFechaCreacion,
                            FechaActualizacion = item1.AsignarProductoKitFechaActualizacion,
                            Estado = item1.AsignarProductoKitEstado,
                            ListaProductos = new ConfigurarProductos()
                            {
                                IdConfigurarProducto = Seguridad.Encriptar(item1.ConfigurarProductoIdConfigurarProducto.ToString()),
                                CantidadMedida = item1.ConfigurarProductoCantidadMedida,
                                FechaCreacion = item1.ConfigurarProductoFechaCreacion,
                                FechaActualizacion = item1.ConfigurarProductoFechaActualizacion,
                                estado = item1.ConfigurarProductoEstado,
                                Codigo = item1.ConfigurarProductoCodigo,
                                Producto = new Producto()
                                {
                                    IdProducto = Seguridad.Encriptar(item1.ProductoIdProducto.ToString()),
                                    IdTipoProducto = Seguridad.Encriptar(item1.ProductoIdTipoProducto.ToString()),
                                    Descripcion = item1.ProductoDescripcion,
                                    Nombre = item1.ProductoNombre,
                                    FechaCreacion = item1.ProductoFechaCreacion,
                                    FechaActualizacion = item1.ProductoFechaActualizacion,
                                    Estado = item1.ProductoEstado,
                                    TipoProducto = new TipoProducto()
                                    {
                                        IdTipoProducto = Seguridad.Encriptar(item1.TipoProductoIdTipoProducto.ToString()),
                                        Descripcion = item1.TipoProductoDescripcion,
                                        FechaCreacion = item1.TipoProductoFechaCreacion,
                                        FechaModificacion = item1.TipoProductoFechaActualizacion,
                                    },
                                },
                                Medida = new Medida()
                                {
                                    IdMedida = Seguridad.Encriptar(item1.MedidaIdMedida.ToString()),
                                    Descripcion = item1.MedidaDescripcion,
                                    FechaCreacion = item1.MedidaFechaCreacion,
                                    Estado = item1.MedidaEstado,
                                },
                                Presentacion = new Presentacion()
                                {
                                    IdPresentacion = Seguridad.Encriptar(item1.PresentacionIdPresentacion.ToString()),
                                    Descripcion = item1.PresentacionDescripcion,
                                    FechaCreacion = item1.PresentacionFechaCreacion,
                                    Estado = item1.PresentacionEstado,
                                },
                                Iva = item1.ConfigurarProductoIva
                            },
                            Kit = new Kit()
                            {
                                IdKit = Seguridad.Encriptar(item1.KitIdKit.ToString()),
                                Codigo = item1.KitCodigo,
                                Descripcion = item1.KitDescripcion,
                                FechaActualizacion = item1.KitFechaActualizacion,
                                FechaCreacion = item1.KitFechaCreacion,
                                AsignarDescuentoKit = new AsignarDescuentoKit()
                                {
                                    IdAsignarDescuentoKit = Seguridad.Encriptar(item1.AsignarDescuentoKitIdAsignarDescuentoKit.ToString()),
                                    IdDescuento = Seguridad.Encriptar(item1.AsignarDescuentoKitIdDescuento.ToString()),
                                    Descuento = new Descuento()
                                    {
                                        IdDescuento = Seguridad.Encriptar(item1.DescuentoIdDescuento.ToString()),
                                        Porcentaje = item1.DescuentoPorcentaje
                                    }
                                }
                            }
                        });
                    }
                }
                else
                {
                    foreach (var item1 in ConexionBD.sp_ConsultarConfigurarProductoPorId(item.AsignarProductoLoteIdRelacionLogica))
                    {
                        ConfigurarProductos.Add(new ConfigurarProductos()
                        {
                            IdConfigurarProducto = Seguridad.Encriptar(item1.ConfigurarProductoIdConfigurarProducto.ToString()),
                            CantidadMedida = item1.ConfigurarProductoCantidadMedida,
                            FechaCreacion = item1.ConfigurarProductoFechaCreacion,
                            FechaActualizacion = item1.ConfigurarProductoFechaActualizacion,
                            estado = item1.ConfigurarProductoEstado,
                            IdAsignacionTu = Seguridad.Encriptar(item1.ConfigurarProductoIdAsignacionTU.ToString()),
                            Codigo = item1.ConfigurarProductoCodigo,
                            PrecioConfigurarProducto = new PrecioConfigurarProducto()
                            {
                                IdPrecioConfigurarProducto = Seguridad.Encriptar(item1.PrecioConfiguracionProductoIdPrecioConfiguracionProducto.ToString()),
                                IdConfigurarProducto = Seguridad.Encriptar(item1.PrecioConfiguracionProductoIdConfigurarProducto.ToString()),
                                FechaRegistro = item1.PrecioConfiguracionProductoFechaRegistro,
                                Precio = item1.PrecioConfiguracionProductoPrecio,
                                Estado = item1.PrecioConfiguracionProductoEstado.ToString()
                            },
                            Producto = new Producto()
                            {
                                IdProducto = Seguridad.Encriptar(item1.ProductoIdProducto.ToString()),
                                IdTipoProducto = Seguridad.Encriptar(item1.ProductoIdTipoProducto.ToString()),
                                Descripcion = item1.ProductoDescripcion,
                                Nombre = item1.ProductoNombre,
                                FechaCreacion = item1.ProductoFechaCreacion,
                                FechaActualizacion = item1.TipoProductoFechaActualizacion,
                                Estado = item1.ProductoEstado,
                                TipoProducto = new TipoProducto()
                                {
                                    IdTipoProducto = Seguridad.Encriptar(item1.TipoProductoIdTipoProducto.ToString()),
                                    Descripcion = item1.TipoProductoDescripcion,
                                    FechaCreacion = item1.TipoProductoFechaCreacion,
                                    FechaModificacion = item1.TipoProductoFechaActualizacion
                                },
                            },
                            Medida = new Medida()
                            {
                                IdMedida = Seguridad.Encriptar(item1.MedidaIdMedida.ToString()),
                                Descripcion = item1.MedidaDescripcion,
                                FechaCreacion = item1.MedidaFechaCreacion,
                                Estado = item1.MedidaEstado,
                            },
                            Presentacion = new Presentacion()
                            {
                                IdPresentacion = Seguridad.Encriptar(item1.PresentacionIdPresentacion.ToString()),
                                Descripcion = item1.PresentacionDescripcion,
                                FechaActualizacion = item1.PresentacionFechaActualizacion,
                                FechaCreacion = item1.PresentacionFechaCreacion,
                                Estado = item1.PresentacionEstado,
                            },
                            Iva = item1.ConfigurarProductoIva
                        });
                    }
                }

                if (item.AsignarProductoLoteIdLote != null)
                {
                    _DatosStock.Add(new Stock()
                    {
                        IdStock = Seguridad.Encriptar(item.StockIdStock.ToString()),
                        Cantidad = item.StockCantidad,
                        IdAsignarProductoLote = Seguridad.Encriptar(item.StockIdAsignarProductoLote.ToString()),
                        FechaActualizacion = item.StockFechaActualizacion,
                        AsignarProductoLote = new AsignarProductoLote()
                        {
                            IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                            FechaExpiracion = item.AsignarProductoLoteFechaExpiracion,
                            IdRelacionLogica = Seguridad.Encriptar(item.AsignarProductoLoteIdRelacionLogica.ToString()),
                            PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                            ValorUnitario = item.AsignarProductoLoteValorUnitario,
                            IdLote = Seguridad.Encriptar(item.AsignarProductoLoteIdLote.ToString()),
                            ConfigurarProductos = ConfigurarProductos.FirstOrDefault(),
                            AsignarProductoKit = AsignarProducoKit.FirstOrDefault(),
                            Lote = new Lote()
                            {
                                IdLote = Seguridad.Encriptar(item.LoteIdLote.ToString()),
                                Capacidad = item.LoteCapacidad,
                                Codigo = item.LoteCodigo,
                                Estado = item.LoteEstado,
                                FechaExpiracion = item.LoteFechaExpiracion,
                            }
                        }
                    });
                }
                else
                {
                    _DatosStock.Add(new Stock()
                    {
                        IdStock = Seguridad.Encriptar(item.StockIdStock.ToString()),
                        Cantidad = item.StockCantidad,
                        IdAsignarProductoLote = Seguridad.Encriptar(item.StockIdAsignarProductoLote.ToString()),
                        FechaActualizacion = item.StockFechaActualizacion,
                        AsignarProductoLote = new AsignarProductoLote()
                        {
                            IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                            FechaExpiracion = item.AsignarProductoLoteFechaExpiracion,
                            IdRelacionLogica = Seguridad.Encriptar(item.AsignarProductoLoteIdRelacionLogica.ToString()),
                            PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                            ValorUnitario = item.AsignarProductoLoteValorUnitario,
                            ConfigurarProductos = ConfigurarProductos.FirstOrDefault(),
                            AsignarProductoKit = AsignarProducoKit.FirstOrDefault(),
                        }
                    });
                }
            }
            return _DatosStock;
        }
        public DetalleVenta InsertarDetalleVenta(DetalleVenta DetalleVenta)
        {
            Stock DatoStock = new Stock();
            DatoStock = ListarStockPorIdAsignarProductoLote(int.Parse(DetalleVenta.IdAsignarProductoLote)).FirstOrDefault();
            DetalleVenta _ObjDetalleVenta = new DetalleVenta();
            _ObjDetalleVenta = FiltrarDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, "0").FirstOrDefault();
            PrecioConfigurarProducto Precio = new PrecioConfigurarProducto();
            if (_ObjDetalleVenta == null)
            {
                if (DatoStock.AsignarProductoLote.PerteneceKit == "True")
                {
                    Precio = CargarDatosPrecioPorProducto(int.Parse(Seguridad.DesEncriptar(DatoStock.AsignarProductoLote.AsignarProductoKit.IdConfigurarProducto)));
                    if (DetalleVenta.AplicaDescuento == "1")
                    {
                        foreach (var item in ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, DetalleVenta.PorcentajeDescuento, Precio.Precio, "0", DatoStock.AsignarProductoLote.AsignarProductoKit.ListaProductos.Iva))
                        {
                            DetalleVenta.IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString());
                            DetalleVenta.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                            DetalleVenta.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                            DetalleVenta.AplicaDescuento = item.AplicaDescuento.ToString();
                            DetalleVenta.Faltante = item.Faltante.ToString();
                            DetalleVenta.Cantidad = item.Cantidad;
                            DetalleVenta.PorcentajeDescuento = item.PorcentajeDescuento;
                            DetalleVenta.ValorUnitario = item.PrecioUnitario;
                            DetalleVenta.PerteneceKitCompleto = item.PerteneceKitCompleto;
                            DetalleVenta.Iva = item.Iva;
                        }
                    }
                    else
                    {      
                        foreach (var item in ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, null, Precio.Precio, "0", DatoStock.AsignarProductoLote.AsignarProductoKit.ListaProductos.Iva))
                        {
                            DetalleVenta.IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString());
                            DetalleVenta.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                            DetalleVenta.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                            DetalleVenta.AplicaDescuento = item.AplicaDescuento.ToString();
                            DetalleVenta.Faltante = item.Faltante.ToString();
                            DetalleVenta.Cantidad = item.Cantidad;
                            DetalleVenta.PorcentajeDescuento = item.PorcentajeDescuento;
                            DetalleVenta.ValorUnitario = item.PrecioUnitario;
                            DetalleVenta.PerteneceKitCompleto = item.PerteneceKitCompleto;
                            DetalleVenta.Iva = item.Iva;
                        }
                    }
                }
                else
                {
                    Precio = CargarDatosPrecioPorProducto(int.Parse(Seguridad.DesEncriptar(DatoStock.AsignarProductoLote.ConfigurarProductos.IdConfigurarProducto)));
                    if (DetalleVenta.AplicaDescuento == "1")
                    {
                        foreach (var item in ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, DetalleVenta.PorcentajeDescuento, Precio.Precio, "0", DatoStock.AsignarProductoLote.ConfigurarProductos.Iva))
                        {
                            DetalleVenta.IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString());
                            DetalleVenta.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                            DetalleVenta.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                            DetalleVenta.AplicaDescuento = item.AplicaDescuento.ToString();
                            DetalleVenta.Faltante = item.Faltante.ToString();
                            DetalleVenta.Cantidad = item.Cantidad;
                            DetalleVenta.PorcentajeDescuento = item.PorcentajeDescuento;
                            DetalleVenta.ValorUnitario = item.PrecioUnitario;
                            DetalleVenta.PerteneceKitCompleto = item.PerteneceKitCompleto;
                            DetalleVenta.Iva = item.Iva;
                        }
                    }
                    else
                    {
                        foreach (var item in ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad, null, Precio.Precio, "0", DatoStock.AsignarProductoLote.ConfigurarProductos.Iva))
                        {
                            DetalleVenta.IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString());
                            DetalleVenta.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                            DetalleVenta.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                            DetalleVenta.AplicaDescuento = item.AplicaDescuento.ToString();
                            DetalleVenta.Faltante = item.Faltante.ToString();
                            DetalleVenta.Cantidad = item.Cantidad;
                            DetalleVenta.PorcentajeDescuento = item.PorcentajeDescuento;
                            DetalleVenta.ValorUnitario = item.PrecioUnitario;
                            DetalleVenta.PerteneceKitCompleto = item.PerteneceKitCompleto;
                            DetalleVenta.Iva = item.Iva;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in ConexionBD.sp_AumentarDetalleVenta(int.Parse(Seguridad.DesEncriptar(_ObjDetalleVenta.IdDetalleVenta)), DetalleVenta.Cantidad))
                {
                    DetalleVenta.IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString());
                    DetalleVenta.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                    DetalleVenta.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                    DetalleVenta.AplicaDescuento = item.AplicaDescuento.ToString();
                    DetalleVenta.Faltante = item.Faltante.ToString();
                    DetalleVenta.Cantidad = item.Cantidad;
                    DetalleVenta.PorcentajeDescuento = item.PorcentajeDescuento;
                    DetalleVenta.ValorUnitario = item.PrecioUnitario;
                    DetalleVenta.PerteneceKitCompleto = item.PerteneceKitCompleto;
                    DetalleVenta.Iva = item.Iva;
                }
            }
            return DetalleVenta;
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
        public List<DetalleVenta> CargarKitDeUnaFactura(DetalleVenta _DetalleVenta)
        {
            List<DetalleVenta> ListaKit = new List<DetalleVenta>();
            foreach (var item in ConexionBD.sp_ConsultarKitPorFactura(int.Parse(_DetalleVenta.IdCabeceraFactura),int.Parse(_DetalleVenta.IdKit)))
            {
                ListaKit.Add(new DetalleVenta()
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
            return ListaKit;
        }
        public List<DetalleVenta> ListarDetalleVenta()
        {
            CargarDatos();
            return ListaDetalleVenta;
        }
        public bool AumentarDetalleVenta(DetalleVenta DetalleVenta)
        {
            try
            {
                ConexionBD.sp_SetCantidadDetalleVenta(int.Parse(DetalleVenta.IdDetalleVenta), DetalleVenta.Cantidad);
                return true;
            }
            catch (Exception)
            {
                return false;
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
        public List<DetalleVenta> ConsultarDetalleVentaPorId(int idDetalleVenta)
        {
            List<DetalleVenta> _DataDetalleVenta = new List<DetalleVenta>();
            foreach (var item in ConexionBD.sp_ConsultarDetalleVentaPorId(idDetalleVenta))
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
