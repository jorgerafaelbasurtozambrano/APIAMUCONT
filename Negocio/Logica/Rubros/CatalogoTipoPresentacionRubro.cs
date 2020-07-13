using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;

namespace Negocio.Logica.Rubros
{
    public class CatalogoTipoPresentacionRubro
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        List<TipoPresentacionRubro> _ListaTipoPresentacionRubro = new List<TipoPresentacionRubro>();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public List<TipoPresentacionRubro> ConsultarTipoPresentacionRubro()
        {
            _ListaTipoPresentacionRubro = new List<TipoPresentacionRubro>();
            foreach (var item in ConexionBD.sp_ConsultarTipoPresentacionRubro())
            {
                _ListaTipoPresentacionRubro.Add(new TipoPresentacionRubro()
                {
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.IdTipoPresentacionRubros.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    FechaCreacion = item.FechaCreacion,
                    Identificador = item.Identificador
                });
            }
            return _ListaTipoPresentacionRubro;
        }
        public List<TipoPresentacionRubro> ConsultarTipoPresentacionRubroPorId(int _IdTipoPresentacion)
        {
            _ListaTipoPresentacionRubro = new List<TipoPresentacionRubro>();
            foreach (var item in ConexionBD.sp_ConsultarTipoPresentacionRubroPorId(_IdTipoPresentacion))
            {
                _ListaTipoPresentacionRubro.Add(new TipoPresentacionRubro()
                {
                    IdTipoPresentacionRubro = Seguridad.Encriptar(item.IdTipoPresentacionRubros.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    FechaCreacion = item.FechaCreacion,
                    Identificador = item.Identificador
                });
            }
            return _ListaTipoPresentacionRubro;
        }
    }
}
