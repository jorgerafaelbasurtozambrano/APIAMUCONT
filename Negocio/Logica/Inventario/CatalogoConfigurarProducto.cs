using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
using System.Globalization;

namespace Negocio.Logica.Inventario
{
    public class CatalogoConfigurarProducto
    {
        CatalogoProducto GestionProducto = new CatalogoProducto();
        CatalogoPrecioConfigurarProducto GestionPrecioConfigurarProducto = new CatalogoPrecioConfigurarProducto();
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<ConfigurarProductos> ListaConfigurarProductos;
        public object InsertarConfigurarProducto(ConfigurarProducto ConfigurarProducto)
        {
            try
            {
                var ListaProductos = ListarConfigurarProductosTodos();
                if (ListaProductos.Where(p => p.Codigo == ConfigurarProducto.Codigo).FirstOrDefault() == null)
                {
                    if (ListaProductos.Where(p => Seguridad.DesEncriptar(p.Producto.IdProducto) == ConfigurarProducto.IdProducto && Seguridad.DesEncriptar(p.Medida.IdMedida) == ConfigurarProducto.IdMedida && Seguridad.DesEncriptar(p.Presentacion.IdPresentacion) == ConfigurarProducto.IdPresentacion && p.CantidadMedida == ConfigurarProducto.CantidadMedida).FirstOrDefault() == null)
                    {
                        foreach (var item in ConexionBD.sp_CrearConfigurarProducto(int.Parse(ConfigurarProducto.IdAsignacionTu), int.Parse(ConfigurarProducto.IdProducto), int.Parse(ConfigurarProducto.IdMedida), int.Parse(ConfigurarProducto.IdPresentacion), ConfigurarProducto.Codigo, ConfigurarProducto.CantidadMedida))
                        {
                            ConfigurarProducto.IdConfigurarProducto = Seguridad.Encriptar(item.IdConfigurarProducto.ToString());
                            ConfigurarProducto.IdAsignacionTu = Seguridad.Encriptar(item.IdAsignacionTU.ToString());
                            ConfigurarProducto.IdProducto = Seguridad.Encriptar(item.IdProducto.ToString());
                            ConfigurarProducto.IdMedida = Seguridad.Encriptar(item.IdMedida.ToString());
                            ConfigurarProducto.IdPresentacion = Seguridad.Encriptar(item.IdPresentacion.ToString());
                            ConfigurarProducto.Codigo = item.Codigo;
                            ConfigurarProducto.CantidadMedida = item.CantidadMedida;
                            ConfigurarProducto.FechaCreacion = item.FechaCreacion;
                            ConfigurarProducto.FechaActualizacion = item.FechaActualizacion;
                            ConfigurarProducto.estado = item.Estado;
                        }
                        return ConfigurarProducto;
                    }
                    else
                    {
                        return "400";
                    }
                }
                else
                {
                    return "400";
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarConfigurarProducto(int IdConfigurarProducto,int IdProducto)
        {
            try
            {
                var listaPrecio = GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == IdConfigurarProducto.ToString()).ToList();
                foreach (var item in listaPrecio)
                {
                    ConexionBD.sp_EliminarTotalmentePrecioConfiguracionProducto(int.Parse(Seguridad.DesEncriptar(item.IdPrecioConfigurarProducto)));
                }
                if (ConexionBD.sp_EliminarConfiguracionYProducto(IdProducto).Count()==1)
                {
                    ConexionBD.sp_EliminarConfigurarProducto(IdConfigurarProducto);
                    GestionProducto.EliminarProducto(IdProducto);
                    return true;
                }
                else
                {
                    ConexionBD.sp_EliminarConfigurarProducto(IdConfigurarProducto);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ModificarConfigurarProducto(ConfigurarProducto ConfigurarProducto)
        {
            try
            {
                ConexionBD.sp_ModificarConfigurarProducto(int.Parse(ConfigurarProducto.IdConfigurarProducto), int.Parse(ConfigurarProducto.IdAsignacionTu), int.Parse(ConfigurarProducto.IdProducto), int.Parse(ConfigurarProducto.IdMedida), int.Parse(ConfigurarProducto.IdPresentacion),ConfigurarProducto.Codigo, ConfigurarProducto.CantidadMedida);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CargarConfigurarProductos()
        {
            ListaConfigurarProductos = new List<ConfigurarProductos>();
            var ListaPrecios = GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto();
            foreach (var item in ConexionBD.sp_ConsultarConfigurarProducto())
            {
                ListaConfigurarProductos.Add(new ConfigurarProductos()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.ConfigurarProductoIdConfigurarProducto.ToString()),
                    CantidadMedida = item.ConfigurarProductoCantidadMedida,
                    FechaCreacion = item.ConfigurarProductoFechaCreacion,
                    FechaActualizacion = item.ConfigurarProductoFechaActualizacion,
                    estado = item.ConfigurarProductoEstado,
                    ConfigurarProductosUtilizado = item.ConfiguracionProductoUtilizado,
                    IdAsignacionTu = Seguridad.Encriptar(item.ConfigurarProductoIdAsignacionTU.ToString()),
                    Codigo =item.ConfigurarProductoCodigo,
                    PrecioConfigurarProducto = ListaPrecios.Where(p=> Seguridad.DesEncriptar(p.IdConfigurarProducto) == item.ConfigurarProductoIdConfigurarProducto.ToString() && p.Estado == "True").FirstOrDefault(),
                    Producto = new Producto()
                    {
                        IdProducto = Seguridad.Encriptar(item.ProductoIdProducto.ToString()),
                        IdTipoProducto = Seguridad.Encriptar(item.ProductoIdTipoProducto.ToString()),
                        Descripcion = item.ProductoDescripcion,
                        Nombre = item.ProductoNombre,
                        FechaCreacion = item.ProductoFechaCreacion,
                        FechaActualizacion = item.TipoProductoFechaActualizacion,
                        Estado = item.ProductoEstado,
                        TipoProducto = new TipoProducto()
                        {
                            IdTipoProducto = Seguridad.Encriptar(item.TipoProductoIdTipoProducto.ToString()),
                            Descripcion = item.TipoProductoDescripcion,
                            FechaCreacion = item.TipoProductoFechaCreacion,
                            FechaModificacion = item.TipoProductoFechaActualizacion,
                            estado = item.TipoProductoEstado,
                        },
                    },
                    Medida = new Medida()
                    {
                        IdMedida = Seguridad.Encriptar(item.MedidaIdMedida.ToString()),
                        Descripcion = item.MedidaDescripcion,
                        FechaActualizacion = item.MedidaFechaActualizacion,
                        FechaCreacion = item.MedidaFechaCreacion,
                        Estado = item.MedidaEstado,
                    },
                    Presentacion = new Presentacion()
                    {
                        IdPresentacion = Seguridad.Encriptar(item.PresentacionIdPresentacion.ToString()),
                        Descripcion = item.PresentacionDescripcion,
                        FechaActualizacion = item.PresentacionFechaActualizacion,
                        FechaCreacion = item.PresentacionFechaCreacion,
                        Estado = item.PresentacionEstado,
                    },
                });
            }
        }
        public List<ConfigurarProductos> ListarConfigurarProductos()
        {
            CargarConfigurarProductos();
            //return ListaConfigurarProductos.Where(p => p.IdConfigurarProducto != "" && p.estado != false && p.Medida.Estado != false && p.Presentacion.Estado != false && p.Producto.Estado != false && p.Producto.TipoProducto.estado != false).GroupBy(a => a.Producto.IdProducto).Select(grp => grp.First()).ToList();
            return ListaConfigurarProductos.Where(p => p.IdConfigurarProducto != "").GroupBy(a => a.Producto.IdProducto).Select(grp => grp.First()).ToList();
        }
        public List<ConfigurarProductos> ListarConfigurarProductosTodos()
        {
            CargarConfigurarProductos();
            //return ListaConfigurarProductos;
            return ListaConfigurarProductos.Where(p => p.IdConfigurarProducto != "" && p.estado != false && p.Medida.Estado != false && p.Presentacion.Estado != false && p.Producto.Estado != false && p.Producto.TipoProducto.estado != false).GroupBy(a => a.IdConfigurarProducto).Select(grp => grp.First()).ToList();
        }
        public List<ConfigurarProductos> CargarConfigurarProductosQueNoTieneUnKit(int IdKit)
        {
            ListaConfigurarProductos = new List<ConfigurarProductos>();
            foreach (var item in ConexionBD.sp_ConsultarConfigurarProductoQueNoTieneUnKit(IdKit))
            {
                ListaConfigurarProductos.Add(new ConfigurarProductos()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.ConfigurarProductoIdConfigurarProducto.ToString()),
                    CantidadMedida = item.ConfigurarProductoCantidadMedida,
                    FechaCreacion = item.ConfigurarProductoFechaCreacion,
                    FechaActualizacion = item.ConfigurarProductoFechaActualizacion,
                    estado = item.ConfigurarProductoEstado,
                    IdAsignacionTu = Seguridad.Encriptar(item.ConfigurarProductoIdAsignacionTU.ToString()),
                    Codigo = item.ConfigurarProductoCodigo,
                    Producto = new Producto()
                    {
                        IdProducto = Seguridad.Encriptar(item.ProductoIdProducto.ToString()),
                        IdTipoProducto = Seguridad.Encriptar(item.ProductoIdTipoProducto.ToString()),
                        Descripcion = item.ProductoDescripcion,
                        Nombre = item.ProductoNombre,
                        FechaCreacion = item.ProductoFechaCreacion,
                        FechaActualizacion = item.TipoProductoFechaActualizacion,
                        Estado = item.ProductoEstado,
                        TipoProducto = new TipoProducto()
                        {
                            IdTipoProducto = Seguridad.Encriptar(item.TipoProductoIdTipoProducto.ToString()),
                            Descripcion = item.TipoProductoDescripcion,
                            FechaCreacion = item.TipoProductoFechaCreacion,
                            FechaModificacion = item.TipoProductoFechaActualizacion,
                            estado = item.TipoProductoEstado,
                        },
                    },
                    Medida = new Medida()
                    {
                        IdMedida = Seguridad.Encriptar(item.MedidaIdMedida.ToString()),
                        Descripcion = item.MedidaDescripcion,
                        FechaActualizacion = item.MedidaFechaActualizacion,
                        FechaCreacion = item.MedidaFechaCreacion,
                        Estado = item.MedidaEstado,
                    },
                    Presentacion = new Presentacion()
                    {
                        IdPresentacion = Seguridad.Encriptar(item.PresentacionIdPresentacion.ToString()),
                        Descripcion = item.PresentacionDescripcion,
                        FechaActualizacion = item.PresentacionFechaActualizacion,
                        FechaCreacion = item.PresentacionFechaCreacion,
                        Estado = item.PresentacionEstado,
                    },
                });
            }
            return ListaConfigurarProductos.GroupBy(a => a.IdConfigurarProducto).Select(grp => grp.First()).ToList(); ;
        }
    }
}