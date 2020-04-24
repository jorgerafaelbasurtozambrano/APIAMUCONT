using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
using Datos;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoPrivilegios
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        List<Privilegios> ListaPrivilegio;
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public List<Privilegios> ListarPrivilegios()
        {
            ListaPrivilegio = new List<Privilegios>();
            foreach (var item in ConexionBD.sp_ConsultarPrivilegios())
            {
                ListaPrivilegio.Add(new Privilegios()
                {
                    IdPrivilegios =Seguridad.Encriptar( item.IdPrivilegios.ToString()),
                    Descripcion = item.Descripcion,
                    Identificador = item.Identificador,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaPrivilegio;
        }

        public List<Privilegios> ListarPrivilegiosDeUnModulo(int IdModuloTipo)
        {
            ListaPrivilegio = new List<Privilegios>();
            foreach (var item in ConexionBD.sp_ConsultarPrivilegiosDeUnModulo(IdModuloTipo))
            {
                ListaPrivilegio.Add(new Privilegios()
                {
                    IdPrivilegios = Seguridad.Encriptar(item.IdPrivilegios.ToString()),
                    Descripcion = item.Descripcion,
                    Identificador = item.Identificador,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaPrivilegio;
        }
    }
}
