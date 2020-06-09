using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoProducto
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Producto> ListaProductos;
        public Producto IngresarProducto(Producto Producto)
        {
            if (Producto.Descripcion == null)
            {
                foreach (var item in ConexionBD.sp_CrearProducto(int.Parse(Producto.IdTipoProducto), null, Producto.Nombre.ToUpper()))
                {
                    Producto.IdProducto = Seguridad.Encriptar(item.IdProducto.ToString());
                    Producto.IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString());
                    Producto.Nombre = item.Nombre;
                    Producto.Descripcion = item.Descripcion;
                    Producto.FechaActualizacion = item.FechaActualizacion;
                    Producto.FechaCreacion = item.FechaCreacion;
                    Producto.Estado = item.Estado;
                }
            }
            else
            {
                foreach (var item in ConexionBD.sp_CrearProducto(int.Parse(Producto.IdTipoProducto), Producto.Descripcion.ToUpper(), Producto.Nombre.ToUpper()))
                {
                    Producto.IdProducto = Seguridad.Encriptar(item.IdProducto.ToString());
                    Producto.IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString());
                    Producto.Nombre = item.Nombre;
                    Producto.Descripcion = item.Descripcion;
                    Producto.FechaActualizacion = item.FechaActualizacion;
                    Producto.FechaCreacion = item.FechaCreacion;
                    Producto.Estado = item.Estado;
                }                
            }
            return Producto;
        }
        public List<Producto> ConsultarProductoPorNombre(string Nombre)
        {
            List<Producto> ListaProducto = new List<Producto>();
            foreach (var item in ConexionBD.sp_ConsultarProductoPorNombre(Nombre))
            {
                ListaProducto.Add(new Producto()
                {
                    IdProducto = Seguridad.Encriptar(item.IdProducto.ToString()),
                    IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString()),
                    Descripcion = item.Descripcion,
                    Nombre = item.Nombre,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado
                });
            }
            return ListaProducto;
        }
        public bool EliminarProducto(int IdProducto)
        {
            try
            {
                ConexionBD.sp_EliminarProducto(IdProducto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string ModificarProducto(Producto Producto)
        {
            Producto Producto1 = ListarProductos().Where(p => p.Nombre == Producto.Nombre.ToUpper()).FirstOrDefault();
            try
            {
                if (Producto1 == null)
                {
                    ConexionBD.sp_ModificarProducto(int.Parse(Producto.IdProducto), int.Parse(Producto.IdTipoProducto), Producto.Descripcion.ToUpper(), Producto.Nombre.ToUpper());
                    return "true";
                }
                else
                {
                    return "400";
                }

            }
            catch (Exception)
            {
                return "false";
            }
        }
        public void CargarProductos()
        {
            ListaProductos = new List<Producto>();
            foreach (var item in ConexionBD.sp_ConsultarProductos())
            {
                ListaProductos.Add(new Producto()
                {
                      IdProducto = Seguridad.Encriptar(item.IdProducto.ToString()),
                      IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString()),
                      Descripcion = item.Descripcion,
                      Nombre = item.Nombre,
                      FechaCreacion = item.FechaCreacion,
                      FechaActualizacion = item.FechaActualizacion,
                      Estado = item.Estado,
                      ProductoUtilizado = item.ProductoUtilizado,
                      TipoProducto = new TipoProducto()
                      {
                            IdTipoProducto = Seguridad.Encriptar(item.TipoProductoIdTipoProducto.ToString()),
                            Descripcion = item.TipoProductoDescripcion,
                            FechaCreacion = item.TipoProductoFechaCreacion,
                            FechaModificacion = item.TipoProductoFechaActualizacion,
                            estado = item.TipoProductoEstado,
                      },
                });
            }
        }
        public List<Producto> ListarProductos()
        {
            CargarProductos();
            return ListaProductos.Where(p => p.Estado != false && p.TipoProducto.estado != false).ToList();
        }
    }
}
