using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
using Datos;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoAsignacionTipoUsuario
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        CatalogoTipoUsuario GestionTipoUsuario = new CatalogoTipoUsuario();
        public bool crearAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
        {
            try
            {
                ConexionBD.sp_CrearAsignacionTU(int.Parse(AsignacionTipoUsuarioEntidad.IdUsuario), int.Parse(AsignacionTipoUsuarioEntidad.IdTipoUsuario));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool modificarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarAsignacionTU(int.Parse(AsignacionTipoUsuarioEntidad.IdAsignacionTU),int.Parse(AsignacionTipoUsuarioEntidad.IdUsuario), int.Parse(AsignacionTipoUsuarioEntidad.IdTipoUsuario));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool eliminarAsignacionTipoUsuario(int IdAsignacionTU)
        {
            try
            {
                var AsignacionTU = ConexionBD.sp_ConsultarAsignacionTU().Where(p => p.IdAsignacionTU == IdAsignacionTU).FirstOrDefault();
                ConexionBD.sp_EliminarAsignacionTU(IdAsignacionTU);
                if (GestionTipoUsuario.ObtenerListaTipoUsuarioDeUnUsuario(AsignacionTU.IdUsuario).Count() == 0)
                {
                    ConexionBD.sp_EliminarUsuario(AsignacionTU.IdUsuario);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
