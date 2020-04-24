using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Datos;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoPrivilegioModuloTipo
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        public void IngresarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad PrivilegioModuloTipoEntidad)
        {
            ConexionBD.sp_CrearPrivilegioMT(PrivilegioModuloTipoEntidad.IdPrivilegio, PrivilegioModuloTipoEntidad.IdModuloTipo);
        }
        public void EliminarPrivilegioModuloTipo(int IdPrivilegioModuloTipo)
        {
            ConexionBD.sp_EliminarPrivilaegioMT(IdPrivilegioModuloTipo);
        }
        public void ModificarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad PrivilegioModuloTipoEntidad)
        {
            ConexionBD.sp_ModificarPrivilegioMT(PrivilegioModuloTipoEntidad.IdPrivilegioModuloTipo, PrivilegioModuloTipoEntidad.IdPrivilegio, PrivilegioModuloTipoEntidad.IdModuloTipo);
        }
    }
}
