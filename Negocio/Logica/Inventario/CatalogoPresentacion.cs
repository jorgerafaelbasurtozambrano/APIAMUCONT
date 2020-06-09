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
        public Presentacion InsertarPresentacion(Presentacion Presentacion)
        {
            foreach (var item in ConexionBD.sp_CrearPresentacion(Presentacion.Descripcion.ToUpper()))
            {
                Presentacion.IdPresentacion = Seguridad.Encriptar(item.IdPresentacion.ToString());
                Presentacion.Descripcion = item.Descripcion;
                Presentacion.FechaActualizacion = item.FechaActualizacion;
                Presentacion.FechaCreacion = item.FechaCreacion;
                Presentacion.Estado = item.Estado;
                Presentacion.PresentacionUtilizado = "0";
            }
            return Presentacion;
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
        public Presentacion ModificarPresentacion(Presentacion Presentacion)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarPresentacion(int.Parse(Presentacion.IdPresentacion), Presentacion.Descripcion.ToUpper()))
                {
                    Presentacion.IdPresentacion = Seguridad.Encriptar(item.IdPresentacion.ToString());
                    Presentacion.Descripcion = item.Descripcion;
                    Presentacion.FechaActualizacion = item.FechaActualizacion;
                    Presentacion.FechaCreacion = item.FechaCreacion;
                    Presentacion.Estado = item.Estado;
                    Presentacion.PresentacionUtilizado = item.PresentacionUtilizado;
                }
                return Presentacion;
            }
            catch (Exception)
            {
                Presentacion.IdPresentacion = null;
                return Presentacion;
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
        public List<Presentacion> ConsultarPresentacionPorDescripcion(string Descripcion)
        {
            ListaPresentaciones = new List<Presentacion>();
            foreach (var item in ConexionBD.sp_ConsultarSiExistePresentacion(Descripcion))
            {
                ListaPresentaciones.Add(new Presentacion()
                {
                    IdPresentacion = Seguridad.Encriptar(item.IdPresentacion.ToString()),
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaActualizacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado
                });
            }
            return ListaPresentaciones;
        }
        public List<Presentacion> ConsultarPresentacionPorId(int idPresentacion)
        {
            ListaPresentaciones = new List<Presentacion>();
            foreach (var item in ConexionBD.sp_ConsultarPresentacionPorId(idPresentacion))
            {
                ListaPresentaciones.Add(new Presentacion()
                {
                    IdPresentacion = Seguridad.Encriptar(item.IdPresentacion.ToString()),
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaActualizacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    PresentacionUtilizado = item.PresentacionUtilizado
                });
            }
            return ListaPresentaciones;
        }
    }
}
