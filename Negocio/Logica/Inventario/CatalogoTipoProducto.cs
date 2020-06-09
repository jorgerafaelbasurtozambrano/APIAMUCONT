using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoTipoProducto
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<TipoProducto> ListaTipoProducto;
        public TipoProducto IngresarTipoProducto(TipoProducto TipoProducto)
        {
            foreach (var item in ConexionBD.sp_CrearTipoProducto(TipoProducto.Descripcion.ToUpper()))
            {
                TipoProducto.IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString());
                TipoProducto.Descripcion = item.Descripcion;
                TipoProducto.FechaCreacion = item.FechaCreacion;
                TipoProducto.FechaModificacion = item.FechaActualizacion;
                TipoProducto.estado = item.estado;
                TipoProducto.TipoProductoUtilizado = "0";
            }
            return TipoProducto;
        }
        public bool EliminarTipoProducto(int IdTipoProducto)
        {
            try
            {
                ConexionBD.sp_EliminarTipoProducto(IdTipoProducto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public TipoProducto ModificarTipoProducto(TipoProducto TipoProducto)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarTipoProducto(int.Parse(TipoProducto.IdTipoProducto), TipoProducto.Descripcion.ToUpper()))
                {
                    TipoProducto.IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString());
                    TipoProducto.Descripcion = item.Descripcion;
                    TipoProducto.FechaCreacion = item.FechaCreacion;
                    TipoProducto.FechaModificacion = item.FechaActualizacion;
                    TipoProducto.estado = item.Estado;
                    TipoProducto.TipoProductoUtilizado = item.TipoProductoUtilizado;
                }
                return TipoProducto;
            }
            catch (Exception)
            {
                TipoProducto.IdTipoProducto = null;
                return TipoProducto;
            }
        }
        public void CargarTipoProductos()
        {
            ListaTipoProducto = new List<TipoProducto>();
            foreach (var item in ConexionBD.sp_ConsultarTipoProducto())
            {
                ListaTipoProducto.Add(new TipoProducto()
                {
                      IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString()),
                      Descripcion = item.Descripcion,
                      FechaCreacion = item.FechaCreacion,
                      FechaModificacion = item.FechaActualizacion,
                      estado = item.Estado,
                      TipoProductoUtilizado =item.TipoProductoUtilizado,
                });
            }
        }
        public List<TipoProducto> ListarTipoProductos()
        {
            CargarTipoProductos();
            return ListaTipoProducto.Where(p=>p.estado != false).GroupBy(a => a.IdTipoProducto).Select(grp => grp.First()).ToList();
        }
        public List<TipoProducto> ConsultarTipoProductoPorDescripcion(string Descripcion)
        {
            ListaTipoProducto = new List<TipoProducto>();
            foreach (var item in ConexionBD.sp_ConsultarSiExisteTipoProducto(Descripcion))
            {
                ListaTipoProducto.Add(new TipoProducto()
                {
                    IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaModificacion = item.FechaActualizacion,
                    estado = item.estado,
                });
            }
            return ListaTipoProducto;
        }
        public List<TipoProducto> ConsultarTipoProductoPorId(int idTipoProducto)
        {
            ListaTipoProducto = new List<TipoProducto>();
            foreach (var item in ConexionBD.sp_ConsultarTipoProductoPorId(idTipoProducto))
            {
                ListaTipoProducto.Add(new TipoProducto()
                {
                    IdTipoProducto = Seguridad.Encriptar(item.IdTipoProducto.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaModificacion = item.FechaActualizacion,
                    estado = item.Estado,
                    TipoProductoUtilizado = item.TipoProductoUtilizado
                });
            }
            return ListaTipoProducto;
        }
    }
}
