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
        CatalogoTipoInteres GestionTipoInteres = new CatalogoTipoInteres();
        public ConfiguracionInteres InsertarInteres(ConfiguracionInteres ConfiguracionInteres)
        {
            foreach (var item in ConexionBD.sp_CrearConfiguracionInteres(int.Parse(ConfiguracionInteres.IdTipoInteres), ConfiguracionInteres.TasaInteres, int.Parse(ConfiguracionInteres.IdTipoInteresMora), ConfiguracionInteres.TasaInteresMora))
            {
                ConfiguracionInteres.IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString());
                ConfiguracionInteres.IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString());
                ConfiguracionInteres.TasaInteres = item.TasaInteres;
                ConfiguracionInteres.Estado = item.Estado;
                ConfiguracionInteres.IdTipoInteresMora = Seguridad.Encriptar(item.IdTipoInteresMora.ToString());
                ConfiguracionInteres.TasaInteresMora = item.TasaInteresMora;
                ConfiguracionInteres.utilizado = "0";
                //ConfiguracionInteres.TipoInteres = GestionTipoInteres.ConsultarTipoInteresPorId(item.IdTipoInteres).FirstOrDefault();
                //ConfiguracionInteres.TipoInteresMora = GestionTipoInteres.ConsultarTipoInteresPorId(item.IdTipoInteresMora).FirstOrDefault();
            }
            return ConfiguracionInteres;
        }
        public ConfiguracionInteres ModificarConfiguracionInteres(ConfiguracionInteres ConfiguracionInteres)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarConfiguracionInteres(int.Parse(ConfiguracionInteres.IdConfiguracionInteres),int.Parse(ConfiguracionInteres.IdTipoInteres), ConfiguracionInteres.TasaInteres, int.Parse(ConfiguracionInteres.IdTipoInteresMora), ConfiguracionInteres.TasaInteresMora))
                {
                    ConfiguracionInteres.IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString());
                    ConfiguracionInteres.IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString());
                    ConfiguracionInteres.TasaInteres = item.TasaInteres;
                    ConfiguracionInteres.Estado = item.Estado;
                    ConfiguracionInteres.IdTipoInteresMora = Seguridad.Encriptar(item.IdTipoInteresMora.ToString());
                    ConfiguracionInteres.TasaInteresMora = item.TasaInteresMora;
                    //ConfiguracionInteres.TipoInteres = GestionTipoInteres.ConsultarTipoInteresPorId(item.IdTipoInteres).FirstOrDefault();
                    //ConfiguracionInteres.TipoInteresMora = GestionTipoInteres.ConsultarTipoInteresPorId(item.IdTipoInteresMora).FirstOrDefault();
                }
                return ConfiguracionInteres;
            }
            catch (Exception)
            {
                ConfiguracionInteres.IdConfiguracionInteres = null;
                return ConfiguracionInteres;
            }
        }
        public void CargarDatos()
        {
            List<TipoInteres> ListaTipoInteres = new List<TipoInteres>();
            ListaTipoInteres = GestionTipoInteres.ListarTipoInteres();
            ListaConfiguracionInteres = new List<ConfiguracionInteres>();
            foreach (var item in ConexionBD.sp_ConsultarConfiguracionInteres())
            {
                ListaConfiguracionInteres.Add(new ConfiguracionInteres()
                {
                    IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString()),
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    TasaInteres = item.TasaInteres,
                    IdTipoInteresMora = Seguridad.Encriptar(item.IdTipoInteresMora.ToString()),
                    TasaInteresMora = item.TasaInteresMora,
                    Estado = item.Estado,
                    //TipoInteres = ListaTipoInteres.Where(p=> Seguridad.DesEncriptar(p.IdTipoInteres) == item.IdTipoInteres.ToString()).FirstOrDefault(),
                    //TipoInteresMora = ListaTipoInteres.Where(p => Seguridad.DesEncriptar(p.IdTipoInteres) == item.IdTipoInteresMora.ToString()).FirstOrDefault(),
                    utilizado = item.ConfigurarVentaUtilizado
                });
            }
        }
        public List<ConfiguracionInteres> ListarConfiguracionInteres()
        {
            CargarDatos();
            return ListaConfiguracionInteres;
        }
        public List<ConfiguracionInteres> ConsultarConfiguracionInteresPorId(int IdConfiguracionInteres)
        {
            List<TipoInteres> ListaTipoInteres = new List<TipoInteres>();
            ListaTipoInteres = GestionTipoInteres.ListarTipoInteres();
            ListaConfiguracionInteres = new List<ConfiguracionInteres>();
            foreach (var item in ConexionBD.sp_ConsultarConfiguracionInteresPorId(IdConfiguracionInteres))
            {
                ListaConfiguracionInteres.Add(new ConfiguracionInteres()
                {
                    IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString()),
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    TasaInteres = item.TasaInteres,
                    IdTipoInteresMora = Seguridad.Encriptar(item.IdTipoInteresMora.ToString()),
                    TasaInteresMora = item.TasaInteresMora,
                    Estado = item.Estado,
                    //TipoInteres = ListaTipoInteres.Where(p => Seguridad.DesEncriptar(p.IdTipoInteres) == item.IdTipoInteres.ToString()).FirstOrDefault(),
                    //TipoInteresMora = ListaTipoInteres.Where(p => Seguridad.DesEncriptar(p.IdTipoInteres) == item.IdTipoInteresMora.ToString()).FirstOrDefault(),
                    utilizado = item.ConfigurarInteresUtilizado
                });
            }
            return ListaConfiguracionInteres;
        }
        public List<ConfiguracionInteres> ConsultarConfiguracionInteresExiste(ConfiguracionInteres _ConfiguracionInteres)
        {
            List<TipoInteres> ListaTipoInteres = new List<TipoInteres>();
            //ListaTipoInteres = GestionTipoInteres.ListarTipoInteres();
            ListaConfiguracionInteres = new List<ConfiguracionInteres>();
            foreach (var item in ConexionBD.sp_ConsultarExisteConfiguracionInteres(int.Parse(_ConfiguracionInteres.IdTipoInteres), _ConfiguracionInteres.TasaInteres,int.Parse(_ConfiguracionInteres.IdTipoInteresMora), _ConfiguracionInteres.TasaInteresMora))
            {
                ListaConfiguracionInteres.Add(new ConfiguracionInteres()
                {
                    IdConfiguracionInteres = Seguridad.Encriptar(item.IdConfiguracionInteres.ToString()),
                    IdTipoInteres = Seguridad.Encriptar(item.IdTipoInteres.ToString()),
                    TasaInteres = item.TasaInteres,
                    IdTipoInteresMora = Seguridad.Encriptar(item.IdTipoInteresMora.ToString()),
                    TasaInteresMora = item.TasaInteresMora,
                    Estado = item.Estado,
                    //TipoInteres = ListaTipoInteres.Where(p => Seguridad.DesEncriptar(p.IdTipoInteres) == item.IdTipoInteres.ToString()).FirstOrDefault(),
                    //TipoInteresMora = ListaTipoInteres.Where(p => Seguridad.DesEncriptar(p.IdTipoInteres) == item.IdTipoInteresMora.ToString()).FirstOrDefault(),
                    utilizado = item.ConfigurarInteresUtilizado
                });
            }
            return ListaConfiguracionInteres;
        }
        public bool EliminarConfiguracionInteres(int _IdConfiguracionInteres)
        {
            try
            {
                ConexionBD.sp_EliminarConfiguracionInteres(_IdConfiguracionInteres);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool HabilitarInteres(int _IdConfiguracionInteres)
        {
            try
            {
                ConexionBD.sp_HabilitarInteres(_IdConfiguracionInteres);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DesHabilitarInteres(int _IdConfiguracionInteres)
        {
            try
            {
                ConexionBD.sp_DesHabilitarInteres(_IdConfiguracionInteres);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
