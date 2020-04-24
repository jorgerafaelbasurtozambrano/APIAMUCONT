using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades;
using Datos;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoModulo
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        static List<Modulo> ListaModulos;
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public CatalogoModulo()
        {
            ListaModulos = new List<Modulo>();
            foreach (var item in ConexionBD.sp_ConsultarModulo())
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
                });
            }
        }

        public List<Modulo> ObtenerListaModulos()
        {
            return ListaModulos;
        }
    }
}
