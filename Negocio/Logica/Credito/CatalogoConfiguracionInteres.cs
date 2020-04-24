using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Credito
{
    public class CatalogoConfiguracionInteres
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        List<ConfiguracionInteres> ListaConfiguracionInteres;
        public object InsertarInteres(ConfiguracionInteres ConfiguracionInteres)
        {
            try
            {
                ConfiguracionInteres DataConfiguracionInteres = new ConfiguracionInteres();
                DataConfiguracionInteres = ListarConfiguracionInteres().Where(p => Seguridad.DesEncriptar(p.IdTipoInteres) == ConfiguracionInteres.IdTipoInteres && p.TasaInteres == ConfiguracionInteres.TasaInteres).FirstOrDefault();
                if (DataConfiguracionInteres != null)
                {
                    foreach (var item in ConexionBD.sp_CrearConfiguracionInteres(int.Parse(ConfiguracionInteres.IdTipoInteres), ConfiguracionInteres.TasaInteres, ConfiguracionInteres.PlazoMeses))
                    {
                        ConfiguracionInteres.IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString());
                        ConfiguracionInteres.IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString());
                        ConfiguracionInteres.Estado = item.Estado.ToString();
                        ConfiguracionInteres.PlazoMeses = item.PlazoMeses;
                    }
                }
                else
                {
                    ConfiguracionInteres = DataConfiguracionInteres;
                }
                return ConfiguracionInteres;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CargarDatos()
        {
            ListaConfiguracionInteres = new List<ConfiguracionInteres>();
            foreach (var item in ConexionBD.sp_ConsultarConfiguracionInteres())
            {
                ListaConfiguracionInteres.Add(new ConfiguracionInteres()
                {
                    IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString()),
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    Estado = item.Estado.ToString(),
                    PlazoMeses = item.PlazoMeses,
                });
            }
        }
        public List<ConfiguracionInteres> ListarConfiguracionInteres()
        {
            CargarDatos();
            return ListaConfiguracionInteres.Where(p=>p.Estado != "false").ToList();
        }
    }
}
