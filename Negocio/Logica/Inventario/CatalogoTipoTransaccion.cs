using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoTipoTransaccion
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<TipoTransaccion> ListatiposTransaccion;
        public void CargarTiposTransaccion()
        {
            ListatiposTransaccion = new List<TipoTransaccion>();
            foreach (var item in ConexionBD.sp_ConsultarTiposTransaccion())
            {
                ListatiposTransaccion.Add(new TipoTransaccion()
                {
                    IdTipoTransaccion = Seguridad.Encriptar(item.IdTipoTransaccion.ToString()),
                    Identificador = item.Identificador,
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaModificacion,
                    FechaCreacion = item.FechaCreacion,
                });
            }
        }
        public List<TipoTransaccion> ListarTiposTransaccion()
        {
            CargarTiposTransaccion();
            return ListatiposTransaccion;
        }
    }
}
