using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoTipoDocumento
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        List<TipoDocumento> ListaTipoDocumento;
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public List<TipoDocumento> ListarTiposDocumentos()
        {
            ListaTipoDocumento = new List<TipoDocumento>();
            foreach (var item in ConexionBD.sp_ConsultarTipoDocumento())
            {
                ListaTipoDocumento.Add(new TipoDocumento() {
                    //IdTipoDocumento = Seguridad.Encriptar(item.IdTipoDocumento.ToString()),
                    IdTipoDocumento = Seguridad.Encriptar(item.IdTipoDocumento.ToString()),
                    Documento = item.Descripcion,
                    Identificador = item.Identificador,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaTipoDocumento;
        }
    }
}
