using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Credito
{
    public class CatalogoConfigurarVenta
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();

        public object InsertarConfigurarVenta(ConfigurarVenta ConfigurarVenta)
        {
            try
            {
                return Seguridad.Encriptar(ConexionBD.sp_CrearConfigurarVenta(int.Parse(ConfigurarVenta.IdCabeceraFactura), int.Parse(ConfigurarVenta.IdPersona), int.Parse(ConfigurarVenta.IdSembrio), Convert.ToBoolean(ConfigurarVenta.EstadoConfVenta), int.Parse(ConfigurarVenta.IdConfiguracionInteres), Convert.ToBoolean(ConfigurarVenta.Efectivo), ConfigurarVenta.Descuento).Select(e => e.Value.ToString()).FirstOrDefault());
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
