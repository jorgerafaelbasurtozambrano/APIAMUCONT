using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoTipoTelefono
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<TipoTelefono> TipoTelefono;
        public void CargarDatos()
        {
            TipoTelefono = new List<Entidades.TipoTelefono>();
            foreach (var item in ConexionBD.sp_ConsultarTipoTelefono())
            {
                TipoTelefono.Add(new Entidades.TipoTelefono()
                {
                      IdTipoTelefono = Seguridad.Encriptar(item.IdTipoTelefono.ToString()),
                      Descripcion = item.Descripcion,
                      Identificador = item.Identificador,
                      FechaCreacion = item.FechaCreacion,
                      Estado = item.Estado,
                });
            }
        }
        public List<TipoTelefono> ListarTelefonos()
        {
            CargarDatos();
            return TipoTelefono;
        }
    }
}
