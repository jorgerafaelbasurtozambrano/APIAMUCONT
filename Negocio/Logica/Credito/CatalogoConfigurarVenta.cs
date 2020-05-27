﻿using System;
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
        CatalogoAsignarSeguro _GestionSeguro = new CatalogoAsignarSeguro();
        public ConfigurarVenta InsertarConfigurarVenta(ConfigurarVenta ConfigurarVenta)
        {
            ConfigurarVenta DataConfigurarVenta = new ConfigurarVenta();
            foreach (var item in ConexionBD.sp_CrearConfigurarVenta(int.Parse(ConfigurarVenta.IdCabeceraFactura), int.Parse(ConfigurarVenta.IdPersona), ConfigurarVenta.EstadoConfVenta, null, ConfigurarVenta.Efectivo, null, ConfigurarVenta.FechaFinalCredito, ConfigurarVenta.AplicaSeguro, ConfigurarVenta.ValorSeguro, ConfigurarVenta.SeguroCancelado))
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
        public ConfigurarVenta ConsultarConfigurarVentaPorFactura(int id_Factura)
        {
            ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
            foreach (var item3 in ConexionBD.sp_ConsultarConfigurarVentaPorIdCabeceraFactura(id_Factura))
            {
                _ConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item3.IdConfigurarVenta.ToString());
                _ConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item3.IdCabeceraFactura.ToString());
                _ConfigurarVenta.IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString());
                _ConfigurarVenta.EstadoConfVenta = item3.IdConfigurarVenta.ToString();
                PersonaEntidad _PersonaEntidad = new PersonaEntidad();
                _PersonaEntidad = GestionPersona.BuscarPersona(item3.IdPersona);
                _ConfigurarVenta._PersonaEntidad = _PersonaEntidad;
                _ConfigurarVenta.FechaFinalCredito = item3.FechaFinCredito;
                _ConfigurarVenta.Efectivo = item3.Efectivo.ToString();
                if (item3.IdConfiguracionInteres != null)
                {
                    _ConfigurarVenta.IdConfiguracionInteres = Seguridad.Encriptar(item3.IdConfiguracionInteres.ToString());
                }
                _ConfigurarVenta.Descuento = item3.Descuento;
                _ConfigurarVenta._AsignarSeguro = _GestionSeguro.ConsultarAsignarSeguroPorConfigurarVenta(item3.IdConfigurarVenta).FirstOrDefault();
            }
            return _ConfigurarVenta;
        }
    }
}
