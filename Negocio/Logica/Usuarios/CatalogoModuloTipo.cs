using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
using Datos;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoModuloTipo
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public void IngresarModuloTipo(ModuloTipoEntidad ModuloTipoEntidad)
        {
            ConexionBD.sp_CrearModuloTipo(ModuloTipoEntidad.IdTipoUsuario, ModuloTipoEntidad.IdModulo);
        }

        public void EliminarModuloTipo(int IdModuloTipo)
        {
            ConexionBD.sp_EliminarModuloTipo(IdModuloTipo);
        }

        public void ModificarModuloTipo(ModuloTipoEntidad ModuloTipoEntidad)
        {
            ConexionBD.sp_ModificarModuloTipo(ModuloTipoEntidad.IdModuloTipo, ModuloTipoEntidad.IdTipoUsuario, ModuloTipoEntidad.IdModulo);
        }

        public List<Modulo> ObtenerModulosDeUnTipoDeUsuario(int IdTipoUsuario)
        {
            List<Modulo> ListaModulos = new List<Modulo>();
            foreach (var item in ConexionBD.sp_ConsultarModulosDeUnaTipoDeUsuario(IdTipoUsuario))
            {
                ListaModulos.Add(new Modulo()
                {
                    IdModulo = Seguridad.Encriptar(item.IdModulo.ToString()),
                    Descripcion = item.Descripcion,
                    Controlador = item.Controlador,
                    Metodo = item.Metodo,
                    Identificador = item.Identificador,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    IdModuloTipo = item.IdModuloTipo,
                });
            }
            return ListaModulos;
        }

    }
}
