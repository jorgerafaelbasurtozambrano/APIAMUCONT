using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Credito
{
    public class CatalogoTipoInteres
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        List<TipoInteres> ListaTipoInteres;
        public object InsertarTipoInteres(TipoInteres TipoInteres)
        {
            try
            {
                TipoInteres DataTipoInteres = ListaTipoInteres.Where(p => p.Descripcion.Trim(' ').Equals(TipoInteres.Descripcion.Trim(' ').ToUpper())).FirstOrDefault();
                if (DataTipoInteres == null)
                {
                    foreach (var item in ConexionBD.sp_CrearTipoInteres(TipoInteres.Identificacion,TipoInteres.Descripcion))
                    {
                        TipoInteres.IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString());
                        TipoInteres.Identificacion = item.Identificacion;
                        TipoInteres.Descripcion = item.Descripcion;
                    }
                }
                else
                {
                    TipoInteres = DataTipoInteres;
                }
                return DataTipoInteres;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CargarDatos()
        {
            ListaTipoInteres = new List<TipoInteres>();
            foreach (var item in ConexionBD.sp_ConsultarTipoInteres())
            {
                ListaTipoInteres.Add(new TipoInteres()
                {
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    Identificacion = item.Identificacion,
                    Descripcion = item.Descripcion,
                });
            }
        }
        public List<TipoInteres> ListarTipoInteres()
        {
            CargarDatos();
            return ListaTipoInteres;
        }
        public List<TipoInteres> ConsultarTipoInteresPorId(int IdTipoInteres)
        {
            ListaTipoInteres = new List<TipoInteres>();
            foreach (var item in ConexionBD.sp_ConsultarTipoInteresPorId(IdTipoInteres))
            {
                ListaTipoInteres.Add(new TipoInteres()
                {
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    Identificacion = item.Identificacion,
                    Descripcion = item.Descripcion,
                });
            }
            return ListaTipoInteres;
        }
    }
}
