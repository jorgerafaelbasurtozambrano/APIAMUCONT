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
        public ConfigurarProducto InsertarConfigurarProducto(ConfigurarProducto ConfigurarProducto)
        {
            foreach (var item in ConexionBD.sp_CrearConfigurarProducto(int.Parse(ConfigurarProducto.IdAsignacionTu), int.Parse(ConfigurarProducto.IdProducto), int.Parse(ConfigurarProducto.IdMedida), int.Parse(ConfigurarProducto.IdPresentacion), ConfigurarProducto.Codigo, ConfigurarProducto.CantidadMedida, ConfigurarProducto.Iva))
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
        public List<ConfigurarProductos> ConsultarConfigurarProductoPorId(int IdConfigurarProducto)
        {
            List<ConfigurarProductos> ListaConfiguracion = new List<ConfigurarProductos>();
            foreach (var item in ConexionBD.sp_ConsultarConfigurarProductoPorId(IdConfigurarProducto))
            {
                string estado = "0";
                if (item.ConfiguracionUtilizado == "1" || item.ConfiguracionUtilizado1 == "1")
                {
                    estado = "1";
                }
                else
                {
                    estado = "0";
                }
                ListaConfiguracion.Add(new ConfigurarProductos()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.ConfigurarProductoIdConfigurarProducto.ToString()),
                    CantidadMedida = item.ConfigurarProductoCantidadMedida,
                    FechaCreacion = item.ConfigurarProductoFechaCreacion,
                    FechaActualizacion = item.ConfigurarProductoFechaActualizacion,
                    estado = item.ConfigurarProductoEstado,
                    ConfigurarProductosUtilizado = estado,
                    IdAsignacionTu = Seguridad.Encriptar(item.ConfigurarProductoIdAsignacionTU.ToString()),
                    Codigo = item.ConfigurarProductoCodigo,
                    PrecioConfigurarProducto = new PrecioConfigurarProducto()
                    {
                        IdPrecioConfigurarProducto = Seguridad.Encriptar(item.PrecioConfiguracionProductoIdPrecioConfiguracionProducto.ToString()),
                        IdConfigurarProducto = Seguridad.Encriptar(item.PrecioConfiguracionProductoIdConfigurarProducto.ToString()),
                        FechaRegistro = item.PrecioConfiguracionProductoFechaRegistro,
                        Precio = item.PrecioConfiguracionProductoPrecio,
                        Estado = item.PrecioConfiguracionProductoEstado.ToString()
                    },
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
                            estado = item.TipoProductoestado,
                        },
                    },
                    Medida = new Medida()
                    {
                        IdMedida = Seguridad.Encriptar(item.MedidaIdMedida.ToString()),
                        Descripcion = item.MedidaDescripcion,
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
                    Iva = item.ConfigurarProductoIva
                });
            }
            return ListaConfiguracion;
        }
        public List<Producto> ConsultarProductoPorId(int idProducto)
        {
            List<Producto> ListaProducto = new List<Producto>();
            foreach (var item in ConexionBD.sp_ConsultarProductoPorId(idProducto))
            {
                ListaProducto.Add(new Producto()
                {
                    IdProducto = Seguridad.Encriptar(item.IdProducto.ToString()),
                    ProductoUtilizado = item.ProductoUtilizado
                });
            }
            return ListaProducto;
        }
        public bool EliminarConfigurarProducto(ConfigurarProductos _ConfigurarProducto)
        {
            try
            {
                ConexionBD.sp_EliminarConfigurarProducto(int.Parse(Seguridad.DesEncriptar(_ConfigurarProducto.IdConfigurarProducto)));
                Producto DatoProducto = new Producto();
                DatoProducto = ConsultarProductoPorId(int.Parse(Seguridad.DesEncriptar(_ConfigurarProducto.Producto.IdProducto))).FirstOrDefault();
                if (DatoProducto.ProductoUtilizado == "0")
                {
                    GestionProducto.EliminarProducto(int.Parse(Seguridad.DesEncriptar(_ConfigurarProducto.Producto.IdProducto)));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ConfigurarProductos ModificarConfigurarProducto(ConfigurarProducto ConfigurarProducto)
        {
            ConfigurarProductos DatoConfigurarProducto = new ConfigurarProductos();
            try
            {
                foreach (var item in ConexionBD.sp_ModificarConfigurarProducto(int.Parse(ConfigurarProducto.IdConfigurarProducto), int.Parse(ConfigurarProducto.IdAsignacionTu), int.Parse(ConfigurarProducto.IdProducto), int.Parse(ConfigurarProducto.IdMedida), int.Parse(ConfigurarProducto.IdPresentacion), ConfigurarProducto.Codigo, ConfigurarProducto.CantidadMedida, ConfigurarProducto.Iva))
                {
                    string estado = "0";
                    if (item.ConfiguracionProductoUtilizado == "1" || item.ConfiguracionUtilizado1 == "1")
                    {
                        estado = "1";
                    }
                    else
                    {
                        estado = "0";
                    }
                    DatoConfigurarProducto.IdConfigurarProducto = Seguridad.Encriptar(item.ConfigurarProductoIdConfigurarProducto.ToString());
                    DatoConfigurarProducto.CantidadMedida = item.ConfigurarProductoCantidadMedida;
                    DatoConfigurarProducto.FechaCreacion = item.ConfigurarProductoFechaCreacion;
                    DatoConfigurarProducto.FechaActualizacion = item.ConfigurarProductoFechaActualizacion;
                    DatoConfigurarProducto.estado = item.ConfigurarProductoEstado;
                    DatoConfigurarProducto.ConfigurarProductosUtilizado = estado;
                    DatoConfigurarProducto.IdAsignacionTu = Seguridad.Encriptar(item.ConfigurarProductoIdAsignacionTU.ToString());
                    DatoConfigurarProducto.Codigo = item.ConfigurarProductoCodigo;
                    DatoConfigurarProducto.PrecioConfigurarProducto = new PrecioConfigurarProducto()
                    {
                        IdPrecioConfigurarProducto = Seguridad.Encriptar(item.PrecioConfiguracionProductoIdPrecioConfiguracionProducto.ToString()),
                        IdConfigurarProducto = Seguridad.Encriptar(item.PrecioConfiguracionProductoIdConfigurarProducto.ToString()),
                        FechaRegistro = item.PrecioConfiguracionProductoFechaRegistro,
                        Precio = item.PrecioConfiguracionProductoPrecio,
                        Estado = item.PrecioConfiguracionProductoEstado.ToString()
                    };
                    DatoConfigurarProducto.Producto = new Producto()
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
                        },
                    };
                    DatoConfigurarProducto.Medida = new Medida()
                    {
                        IdMedida = Seguridad.Encriptar(item.MedidaIdMedida.ToString()),
                        Descripcion = item.MedidaDescripcion,
                        FechaCreacion = item.MedidaFechaCreacion,
                        Estado = item.MedidaEstado,
                    };
                    DatoConfigurarProducto.Presentacion = new Presentacion()
                    {
                        IdPresentacion = Seguridad.Encriptar(item.PresentacionIdPresentacion.ToString()),
                        Descripcion = item.PresentacionDescripcion,
                        FechaActualizacion = item.PresentacionFechaActualizacion,
                        FechaCreacion = item.PresentacionFechaCreacion,
                        Estado = item.PresentacionEstado,
                    };
                    DatoConfigurarProducto.Iva = item.ConfigurarProductoIva;
                }
                return DatoConfigurarProducto;
            }
            catch (Exception)
            {
                DatoConfigurarProducto.IdConfigurarProducto = null;
                return DatoConfigurarProducto;
            }
        }
        public void CargarConfigurarProductos()
        {
            ListaConfigurarProductos = new List<ConfigurarProductos>();
            foreach (var item in ConexionBD.sp_ConsultarConfigurarProducto())
            {
                string estado = "0";
                if (item.ConfiguracionProductoUtilizado == "1" || item.ConfiguracionUtilizado1 == "1")
                {
                    estado = "1";
                }
                else
                {
                    estado = "0";
                }
                ListaConfigurarProductos.Add(new ConfigurarProductos()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.ConfigurarProductoIdConfigurarProducto.ToString()),
                    CantidadMedida = item.ConfigurarProductoCantidadMedida,
                    FechaCreacion = item.ConfigurarProductoFechaCreacion,
                    FechaActualizacion = item.ConfigurarProductoFechaActualizacion,
                    estado = item.ConfigurarProductoEstado,
                    ConfigurarProductosUtilizado = estado,
                    IdAsignacionTu = Seguridad.Encriptar(item.ConfigurarProductoIdAsignacionTU.ToString()),
                    Codigo =item.ConfigurarProductoCodigo,
                    PrecioConfigurarProducto = new PrecioConfigurarProducto()
                    {
                        IdPrecioConfigurarProducto = Seguridad.Encriptar(item.PrecioConfiguracionProductoIdPrecioConfiguracionProducto.ToString()),
                        IdConfigurarProducto = Seguridad.Encriptar(item.PrecioConfiguracionProductoIdConfigurarProducto.ToString()),
                        FechaRegistro = item.PrecioConfiguracionProductoFechaRegistro,
                        Precio = item.PrecioConfiguracionProductoPrecio,
                        Estado = item.PrecioConfiguracionProductoEstado.ToString()
                    },
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
                    Iva = item.ConfigurarProductoIva
                });
            }
        }
        public List<AsignarProductoLote> CargarDatosAsignarProductoLoteQueNoPerteneceaAkit()
        {

            List<AsignarProductoLote> ListaAsignarProductoLote = new List<AsignarProductoLote>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarProductoLote().Where(p=>p.PerteneceKit == false).ToList())
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
        public List<ConfigurarProductos> ListarConfigurarProductos()
        {
            CargarConfigurarProductos();
            //return ListaConfigurarProductos.Where(p => p.IdConfigurarProducto != "" && p.estado != false && p.Medida.Estado != false && p.Presentacion.Estado != false && p.Producto.Estado != false && p.Producto.TipoProducto.estado != false).GroupBy(a => a.Producto.IdProducto).Select(grp => grp.First()).ToList();
            //return ListaConfigurarProductos.Where(p => p.IdConfigurarProducto != "").GroupBy(a => a.Producto.IdProducto).Select(grp => grp.First()).ToList();
            return ListaConfigurarProductos;
        }
        public List<ConfigurarProductos> ListarConfigurarProductosTodos()
        {
            CargarConfigurarProductos();
            //return ListaConfigurarProductos;
            return ListaConfigurarProductos.Where(p => p.IdConfigurarProducto != "" && p.estado != false && p.Medida.Estado != false && p.Presentacion.Estado != false && p.Producto.Estado != false && p.Producto.TipoProducto.estado != false).GroupBy(a => a.IdConfigurarProducto).Select(grp => grp.First()).ToList();
        }
        public List<ConfigurarProductos> CargarConfigurarProductosQueNoTieneUnKit(int IdKit)
        {
            var listaAsignarProductoLote = CargarDatosAsignarProductoLoteQueNoPerteneceaAkit();
            ListaConfigurarProductos = new List<ConfigurarProductos>();
            foreach (var item in ConexionBD.sp_ConsultarConfigurarProductoQueNoTieneUnKit(IdKit))
            {
                string estado = "0";
                if (listaAsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdRelacionLogica) == item.ConfigurarProductoIdConfigurarProducto.ToString()).FirstOrDefault() != null)
                {
                    estado = "1";
                }
                else
                {
                    estado = "0";
                }

                ListaConfigurarProductos.Add(new ConfigurarProductos()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.ConfigurarProductoIdConfigurarProducto.ToString()),
                    CantidadMedida = item.ConfigurarProductoCantidadMedida,
                    FechaCreacion = item.ConfigurarProductoFechaCreacion,
                    FechaActualizacion = item.ConfigurarProductoFechaActualizacion,
                    estado = item.ConfigurarProductoEstado,
                    ConfigurarProductosUtilizado = estado,
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
                    Iva = item.ConfigurarProductoIva
                });
            }
            return ListaConfigurarProductos.GroupBy(a => a.IdConfigurarProducto).Select(grp => grp.First()).ToList(); ;
        }
        public List<ConfigurarProductos> ConsultarConfiguracionProductoPorCodigo(string Codigo)
        {
            List<ConfigurarProductos> DatoConfigurarProducto = new List<ConfigurarProductos>();
            foreach (var item in ConexionBD.spConsultarConfigurarProductoPorCodigo(Codigo.Trim()))
            {
                DatoConfigurarProducto.Add(new ConfigurarProductos()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.IdConfigurarProducto.ToString()),
                    CantidadMedida = item.CantidadMedida,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    estado = item.Estado,
                    IdAsignacionTu = Seguridad.Encriptar(item.IdAsignacionTU.ToString()),
                    Codigo = item.Codigo,
                });
            }
            return DatoConfigurarProducto;
        }
        public List<ConfigurarProducto> ConsultarSiExisteYaUnaConfiguracion(ConfigurarProducto _ConfigurarProductos)
        {
            List<ConfigurarProducto> DatoConfigurarProducto = new List<ConfigurarProducto>();
            foreach (var item in ConexionBD.spConsultarSiExisteYaUnaConfiguracion(int.Parse(_ConfigurarProductos.IdProducto),int.Parse(_ConfigurarProductos.IdMedida),int.Parse(_ConfigurarProductos.IdPresentacion), _ConfigurarProductos.CantidadMedida))
            {
                DatoConfigurarProducto.Add(new ConfigurarProducto()
                {
                    IdConfigurarProducto = Seguridad.Encriptar(item.IdConfigurarProducto.ToString()),
                    CantidadMedida = item.CantidadMedida,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    estado = item.Estado,
                    IdAsignacionTu = Seguridad.Encriptar(item.IdAsignacionTU.ToString()),
                    Codigo = item.Codigo,
                });
            }
            return DatoConfigurarProducto;
        }
    }
}