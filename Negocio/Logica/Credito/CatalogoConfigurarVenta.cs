using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.Credito
{
    public class CatalogoConfigurarVenta
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        ConsultarUsuariosYPersonas GestionPersona = new ConsultarUsuariosYPersonas();
        public ConfigurarVenta InsertarConfigurarVenta(ConfigurarVenta ConfigurarVenta)
        {
            ConfigurarVenta DataConfigurarVenta = new ConfigurarVenta();
            int? IdConfiguracionInteres = null;
            if (ConfigurarVenta.IdConfiguracionInteres!=null)
            {
                IdConfiguracionInteres = int.Parse(ConfigurarVenta.IdConfiguracionInteres);
            }
            foreach (var item in ConexionBD.sp_CrearConfigurarVenta(int.Parse(ConfigurarVenta.IdCabeceraFactura), int.Parse(ConfigurarVenta.IdPersona), ConfigurarVenta.EstadoConfVenta,IdConfiguracionInteres, ConfigurarVenta.Efectivo, null, ConfigurarVenta.FechaFinalCredito, ConfigurarVenta.AplicaSeguro))
            {
                DataConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item.IdConfigurarVenta.ToString());
                DataConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                DataConfigurarVenta.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                DataConfigurarVenta.EstadoConfVenta = item.EstadoConfVenta.ToString();
                DataConfigurarVenta.Efectivo = item.Efectivo.ToString();
                DataConfigurarVenta.Descuento = null;
            }
            return DataConfigurarVenta;
        }
        CatalogoConfiguracionInteres GestionInteres = new CatalogoConfiguracionInteres();
        public List<SaldoPendiente> ConsultarSaldoPendiente(int IdConfigurarVenta)
        {
            List<SaldoPendiente> Saldo = new List<SaldoPendiente>();
            foreach (var item in ConexionBD.sp_ConsultarSaldoPorConfigurarVenta(IdConfigurarVenta))
            {
                Saldo.Add(new SaldoPendiente()
                {
                    IdSaldoPendiente = Seguridad.Encriptar(item.IdSaldoPendiente.ToString()),
                    IdConfigurarVenta = Seguridad.Encriptar(item.IdConfiguracionVenta.ToString()),
                    Pendiente = item.Pendiente,
                    TotalFactura = item.TotalFactura,
                    TotalInteres = item.TotalInteres,
                    FechaRegistro = item.FechaRegistro,
                });
            }
            return Saldo;
        }
        public ConfigurarVenta ConsultarConfigurarVentaPorFactura(int id_Factura)
        {
            ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
            foreach (var item3 in ConexionBD.sp_ConsultarConfigurarVentaPorIdCabeceraFactura(id_Factura))
            {
                _ConfigurarVenta._SaldoPendiente = ConsultarSaldoPendiente(item3.IdConfigurarVenta).FirstOrDefault();
                _ConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item3.IdConfigurarVenta.ToString());
                _ConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item3.IdCabeceraFactura.ToString());
                _ConfigurarVenta.IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString());
                _ConfigurarVenta.EstadoConfVenta = item3.IdConfigurarVenta.ToString();
                PersonaEntidad _PersonaEntidad = new PersonaEntidad();
                _PersonaEntidad = GestionPersona.BuscarPersona(item3.IdPersona);
                _ConfigurarVenta._PersonaEntidad = _PersonaEntidad;
                _ConfigurarVenta.FechaFinalCredito = item3.FechaFinCredito;
                _ConfigurarVenta.Efectivo = item3.Efectivo.ToString();
                _ConfigurarVenta.AplicaSeguro = item3.AplicaSeguro.ToString();
                if (item3.IdConfiguracionInteres != null)
                {
                    _ConfigurarVenta.IdConfiguracionInteres = Seguridad.Encriptar(item3.IdConfiguracionInteres.ToString());
                    _ConfigurarVenta.ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item3.IdConfiguracionInteres).FirstOrDefault();
                }
                _ConfigurarVenta.Descuento = item3.Descuento;
            }
            return _ConfigurarVenta;
        }
        public ConfigurarVenta ConsultarConfigurarVentaPorId(int idConfigurarVenta)
        {
            ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
            foreach (var item3 in ConexionBD.sp_ConsultarConfigurarVentaPorId(idConfigurarVenta))
            {
                string estado = "0";
                if (item3.EstadoConfVenta == true)
                {
                    estado = "1";
                }
                else
                {
                    estado = "0";
                }
                _ConfigurarVenta._SaldoPendiente = ConsultarSaldoPendiente(item3.IdConfigurarVenta).FirstOrDefault();
                _ConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item3.IdConfigurarVenta.ToString());
                _ConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item3.IdCabeceraFactura.ToString());
                _ConfigurarVenta.IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString());
                _ConfigurarVenta.EstadoConfVenta = estado;
                PersonaEntidad _PersonaEntidad = new PersonaEntidad();
                _PersonaEntidad = GestionPersona.BuscarPersona(item3.IdPersona);
                _ConfigurarVenta._PersonaEntidad = _PersonaEntidad;
                _ConfigurarVenta.FechaFinalCredito = item3.FechaFinCredito;
                _ConfigurarVenta.Efectivo = item3.Efectivo.ToString();
                _ConfigurarVenta.AplicaSeguro = item3.AplicaSeguro.ToString();
                if (item3.IdConfiguracionInteres != null)
                {
                    _ConfigurarVenta.IdConfiguracionInteres = Seguridad.Encriptar(item3.IdConfiguracionInteres.ToString());
                    _ConfigurarVenta.ConfiguracionInteres = GestionInteres.ConsultarConfiguracionInteresPorId(item3.IdConfiguracionInteres).FirstOrDefault();
                }
                _ConfigurarVenta.Descuento = item3.Descuento;
            }
            return _ConfigurarVenta;
        }
    }
}
