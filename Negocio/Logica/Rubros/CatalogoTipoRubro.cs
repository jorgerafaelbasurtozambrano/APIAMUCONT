using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;

namespace Negocio.Logica.Rubros
{
    public class CatalogoTipoRubro
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        List<TipoRubro> _ListaTipoRubro = new List<TipoRubro>();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public List<TipoRubro> ConsultarTipoRubro()
        {
            _ListaTipoRubro = new List<TipoRubro>();
            foreach (var item in ConexionBD.sp_ConsultarTipoRubro())
            {
                _ListaTipoRubro.Add(new TipoRubro() { 
                    IdTipoRubro = Seguridad.Encriptar(item.IdTipoRubro.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    FechaCreacion = item.FechaCreacion,
                    Identificador = item.Identificador
                });
            }
            return _ListaTipoRubro;
        }
    }
}
