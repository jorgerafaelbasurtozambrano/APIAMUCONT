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
        public string IngresarTipoProducto(TipoProducto TipoProducto)
        {
            try
            {
                TipoProducto TipoProducto1 = ListarTipoProductos().Where(p => p.Descripcion.Contains(TipoProducto.Descripcion.ToUpper())).FirstOrDefault();
                if (TipoProducto1 == null)
                {
                    ConexionBD.sp_CrearTipoProducto(TipoProducto.Descripcion.ToUpper());
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
        public string ModificarTipoProducto(TipoProducto TipoProducto)
        {
            try
            {
                TipoProducto TipoProducto1 = ListarTipoProductos().Where(p => Seguridad.DesEncriptar(p.IdTipoProducto)== Seguridad.DesEncriptar(TipoProducto.IdTipoProducto)).FirstOrDefault();
                if (TipoProducto1 == null)
                {                    
                    return "false";
                }
                else
                {
                    if (TipoProducto1.Descripcion.Contains(TipoProducto.Descripcion))
                    {
                        ConexionBD.sp_ModificarTipoProducto(int.Parse(TipoProducto.IdTipoProducto), TipoProducto.Descripcion.ToUpper());
                        return "true";
                    }
                    if(ListarTipoProductos().Where(p => p.Descripcion.Contains(TipoProducto.Descripcion.ToUpper())).FirstOrDefault() == null)
                    {
                        return "true";
                    }
                    else
                    {
                        return "400";
                    }
                }
            }
            catch (Exception)
            {
                return "false";
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
                      TipoUsuarioUtilizado =item.TipoProductoUtilizado,
                });
            }
        }
        public List<TipoProducto> ListarTipoProductos()
        {
            CargarTipoProductos();
            return ListaTipoProducto.Where(p=>p.estado != false).GroupBy(a => a.IdTipoProducto).Select(grp => grp.First()).ToList();
        }
    }
}
