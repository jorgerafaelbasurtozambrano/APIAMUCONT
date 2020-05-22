using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
using Negocio.Logica.Factura;
namespace Negocio.Logica.Inventario
{
    public class CatalogoAsignarProductoKit
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoKit GestionKit = new CatalogoKit();
        static List<AsignarProductosKits> ListaAsignarProductosKits;
        public bool InsertarAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
        {
            try
            {
                ConexionBD.sp_CrearAsignarProductoKit(int.Parse(AsignarProductoKit.IdConfigurarProducto), int.Parse(AsignarProductoKit.IdAsignarDescuentoKit));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarAsignarProductoKit(int IdAsignarProductoKit)
        {
            try
            {
                ConexionBD.sp_EliminarAsignacionProductoKit(IdAsignarProductoKit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ModificarAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
        {
            try
            {
                ConexionBD.sp_ModificarAsignacionProductoKit(int.Parse(AsignarProductoKit.IdAsignarProductoKit), int.Parse(AsignarProductoKit.IdConfigurarProducto), int.Parse(AsignarProductoKit.IdAsignarDescuentoKit));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<AsignarProductoLote> CargarDatosAsignarProductoLote()
        {
            List<AsignarProductoLote>  ListaAsignarProductoLote = new List<AsignarProductoLote>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarProductoLote().Where(p=> p.PerteneceKit == true).ToList())
            {
                ListaAsignarProductoLote.Add(new AsignarProductoLote()
                {
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    IdRelacionLogica = Seguridad.Encriptar(item.IdRelacionLogica.ToString()),
                    PerteneceKit = item.PerteneceKit.ToString(),
                    FechaExpiracion = item.FechaExpiracion,
                    ValorUnitario = item.ValorUnitario,
                });
            }
            return ListaAsignarProductoLote;
        }

        public void CargarAsignarProductoKit()
        {
            ListaAsignarProductosKits = new List<AsignarProductosKits>();
            var ListaKit = GestionKit.ListarKit();
            foreach (var item in ListaKit)
            {
                List<AsignarProductoKit> ListaAsignarProductoKit = new List<AsignarProductoKit>();
                foreach (var item1 in ConexionBD.sp_ConsultarAsignarProductoKit(int.Parse(Seguridad.DesEncriptar(item.IdKit))))
                {
                    ListaAsignarProductoKit.Add(new AsignarProductoKit()
                    {
                        IdAsignarProductoKit = Seguridad.Encriptar(item1.AsignarProductoKitIdAsignarProductoKit.ToString()),
                        IdConfigurarProducto = Seguridad.Encriptar(item1.AsignarProductoKitIdConfigurarProducto.ToString()),
                        IdAsignarDescuentoKit = Seguridad.Encriptar(item1.AsignarDescuentoKitIdAsignarDescuentoKit.ToString()),
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
                                    estado = item1.TipoProductoEstado,
                                },
                            },
                            Medida = new Medida()
                            {
                                IdMedida = Seguridad.Encriptar(item1.MedidaIdMedida.ToString()),
                                Descripcion = item1.MedidaDescripcion,
                                FechaActualizacion = item1.MedidaFechaActualizacion,
                                FechaCreacion = item1.MedidaFechaCreacion,
                                Estado = item1.MedidaEstado,
                            },
                            Presentacion = new Presentacion()
                            {
                                IdPresentacion = Seguridad.Encriptar(item1.PresentacionIdPresentacion.ToString()),
                                Descripcion = item1.PresentacionDescripcion,
                                FechaActualizacion = item1.PresentacionActualizacion,
                                FechaCreacion = item1.PresentacionFechaCreacion,
                                Estado = item1.PresentacionEstado,
                            },
                            Iva = item1.ConfigurarProductoIva
                        },
                        Kit = ListaKit.Where(p=>Seguridad.DesEncriptar(p.IdKit)== item1.AsignarDescuentoKitIdKit.ToString()).FirstOrDefault(),
                    });
                }
                ListaAsignarProductosKits.Add(new AsignarProductosKits()
                {
                    IdKit = item.IdKit,
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado,
                    ListaAsignarProductoKit = ListaAsignarProductoKit,
                });
            }
        }
        public List<AsignarProductosKits> ListarAsignarProductosKit()
        {
            CargarAsignarProductoKit();
            return ListaAsignarProductosKits;
        }
        public List<AsignarProductosKits> ListarProductosDeUnKit(int IdKit)
        {
            //CargarAsignarProductoKit();
            var ListaKit = GestionKit.ListarKit();
            ListaAsignarProductosKits = new List<AsignarProductosKits>();
            var ListaAsignarProductoLote = CargarDatosAsignarProductoLote();
            foreach (var item in ListaKit.Where(p=> Seguridad.DesEncriptar(p.IdKit) == IdKit.ToString()).ToList())
            {
                List<AsignarProductoKit> ListaAsignarProductoKit = new List<AsignarProductoKit>();
                foreach (var item1 in ConexionBD.sp_ConsultarAsignarProductoKit(int.Parse(Seguridad.DesEncriptar(item.IdKit))))
                {
                    string estado = "0";
                    if (ListaAsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdRelacionLogica) == item1.AsignarProductoKitIdAsignarProductoKit.ToString()).FirstOrDefault() != null)
                    {
                        estado = "1";
                    }
                    else
                    {
                        estado = "0";
                    }

                    ListaAsignarProductoKit.Add(new AsignarProductoKit()
                    {
                        IdAsignarProductoKit = Seguridad.Encriptar(item1.AsignarProductoKitIdAsignarProductoKit.ToString()),
                        IdConfigurarProducto = Seguridad.Encriptar(item1.AsignarProductoKitIdConfigurarProducto.ToString()),
                        IdAsignarDescuentoKit = Seguridad.Encriptar(item1.AsignarDescuentoKitIdAsignarDescuentoKit.ToString()),
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
                            ConfigurarProductosUtilizado = estado,
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
                                    estado = item1.TipoProductoEstado,
                                },
                            },
                            Medida = new Medida()
                            {
                                IdMedida = Seguridad.Encriptar(item1.MedidaIdMedida.ToString()),
                                Descripcion = item1.MedidaDescripcion,
                                FechaActualizacion = item1.MedidaFechaActualizacion,
                                FechaCreacion = item1.MedidaFechaCreacion,
                                Estado = item1.MedidaEstado,
                            },
                            Presentacion = new Presentacion()
                            {
                                IdPresentacion = Seguridad.Encriptar(item1.PresentacionIdPresentacion.ToString()),
                                Descripcion = item1.PresentacionDescripcion,
                                FechaActualizacion = item1.PresentacionActualizacion,
                                FechaCreacion = item1.PresentacionFechaCreacion,
                                Estado = item1.PresentacionEstado,
                            },
                            Iva = item1.ConfigurarProductoIva
                        },
                        Kit = ListaKit.Where(p => Seguridad.DesEncriptar(p.IdKit) == item1.AsignarDescuentoKitIdKit.ToString()).FirstOrDefault(),
                    });
                }
                ListaAsignarProductosKits.Add(new AsignarProductosKits()
                {
                    IdKit = item.IdKit,
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado,
                    ListaAsignarProductoKit = ListaAsignarProductoKit,
                });
            }
            return ListaAsignarProductosKits.GroupBy(a => a.IdKit).Select(grp => grp.First()).Where(p=> int.Parse(Seguridad.DesEncriptar(p.IdKit)) == IdKit).ToList();
        }
    }
}
