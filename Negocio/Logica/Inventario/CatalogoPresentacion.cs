using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoPresentacion
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Presentacion> ListaPresentaciones;
        public string InsertarPresentacion(Presentacion Presentacion)
        {
            Presentacion Presentacion1 = ListarPresentaciones().Where(p => p.Descripcion == Presentacion.Descripcion.ToUpper()).FirstOrDefault();
            try
            {
                if (Presentacion1==null)
                {
                    ConexionBD.sp_CrearPresentacion(Presentacion.Descripcion.ToUpper());
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
        public bool EliminarPresentacion(int IdPresentacion)
        {
            try
            {
                ConexionBD.sp_EliminarPresentacion(IdPresentacion);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string ModificarPresentacion(Presentacion Presentacion)
        {
            Presentacion Presentacion1 = ListarPresentaciones().Where(p => p.Descripcion == Presentacion.Descripcion.ToUpper()).FirstOrDefault();
            try
            {
                if (Presentacion1 == null)
                {
                    ConexionBD.sp_ModificarPresentacion(int.Parse(Presentacion.IdPresentacion), Presentacion.Descripcion.ToUpper());
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
        public void CargarPresentaciones()
        {
            ListaPresentaciones = new List<Presentacion>();
            foreach (var item in ConexionBD.sp_ConsultarPresentacion())
            {
                ListaPresentaciones.Add(new Presentacion()
                {
                    IdPresentacion = Seguridad.Encriptar(item.IdPresentacion.ToString()),
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaActualizacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    PresentacionUtilizado = item.PresentacionUtilizado,
                });
            }
        }
        public List<Presentacion> ListarPresentaciones()
        {
            CargarPresentaciones();
            return ListaPresentaciones.GroupBy(a => a.IdPresentacion).Select(grp => grp.First()).Where(p => p.Estado != false).ToList();
        }
    }
}
