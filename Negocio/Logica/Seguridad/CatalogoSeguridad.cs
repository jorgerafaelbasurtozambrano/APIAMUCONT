using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Seguridad
{
    public class CatalogoSeguridad
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        List<Tokens> ListaTokens;
        public List<Tokens> ListarTokens()
        {
            ListaTokens = new List<Tokens> ();
            foreach (var item in ConexionBD.sp_ConsultarTokens())
            {
                ListaTokens.Add(new Tokens()
                {
                    IdToken = item.TokensIdToken,
                    Descripcion = item.TokensDescripcion,
                    Identificador = item.TokensIdentificador,
                    Estado = item.TokensEstado,
                    Clave = new Clave()
                    {
                        IdClave = item.ClaveIdClave,
                        Descripcion = item.ClaveDescripcion,
                        Estado = item.ClaveEstado,
                    },
                });
            }
            return ListaTokens;

        }
    }
}
