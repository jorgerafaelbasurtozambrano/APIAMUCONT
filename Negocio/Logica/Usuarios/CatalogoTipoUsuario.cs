using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
using Datos;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoTipoUsuario
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        static List<TipoUsuario> ListaTipoUsuario = new List<TipoUsuario>();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public CatalogoTipoUsuario()
        {
            ListaTipoUsuario = new List<TipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarTipoUsuario())
            {
                ListaTipoUsuario.Add(new TipoUsuario()
                {
                    IdTipoUsuario = Seguridad.Encriptar(item.IdTipoUsuario.ToString()),
                    Descripcion = item.Descripcion,
                    Identificacion = item.Identificacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
        }

        public List<TipoUsuario> ObtenerListaTipoUsuario()
        {
            return ListaTipoUsuario;
        }

        public List<TipoUsuario> ObtenerListaTipoUsuarioDeUnUsuario(int IdUsuario)
        {
            List<TipoUsuario> ListaTipoUsuario = new List<TipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarTiposUsuarioDeUnaPersona(IdUsuario))
            {
                ListaTipoUsuario.Add(new TipoUsuario()
                {
                    IdTipoUsuario = item.IdTipoUsuario.ToString(),
                    Descripcion = item.Descripcion,
                    Identificacion = item.IdentificacionTipoUsuario,
                    Estado = null,
                    IdAsignacionTu =Seguridad.Encriptar(item.IdAsignacionTU.ToString()),
                });
            }
            return ListaTipoUsuario;
        }

    }
}
