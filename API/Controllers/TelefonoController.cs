using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class TelefonoController : ApiController
    {
        CatalogoTelefono GestionTelefono = new CatalogoTelefono();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoTelefono")]
        public object IngresoTelefono(TelefonoEntidad TelefonoEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (TelefonoEntidad.encriptada == null || string.IsNullOrEmpty(TelefonoEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(TelefonoEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (TelefonoEntidad.IdTipoTelefono == null || string.IsNullOrEmpty(TelefonoEntidad.IdTipoTelefono.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingres el id tipo telefono";
                        }
                        else if (TelefonoEntidad.IdPersona == null || string.IsNullOrEmpty(TelefonoEntidad.IdPersona.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingres el id persona";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            TelefonoEntidad.IdTipoTelefono = Seguridad.DesEncriptar(TelefonoEntidad.IdTipoTelefono);
                            TelefonoEntidad.IdPersona = Seguridad.DesEncriptar(TelefonoEntidad.IdPersona);
                            respuesta = GestionTelefono.IngresarTelefono(TelefonoEntidad);
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/TalentoHumano/EliminarTelefono")]
        public object EliminarTelefono(TelefonoEntidad TelefonoEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveDelete = ListaClaves.Where(c => c.Identificador == 3).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(TelefonoEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    TelefonoEntidad.IdTelefono = Seguridad.DesEncriptar(TelefonoEntidad.IdTelefono);
                   
                    respuesta = GestionTelefono.EliminarTelefono(int.Parse(TelefonoEntidad.IdTelefono));
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje, respuesta };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/TalentoHumano/ActualizarTelefono")]
        public object ActualizarTelefono(TelefonoEntidad TelefonoEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePut = ListaClaves.Where(c => c.Identificador == 2).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(TelefonoEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    TelefonoEntidad.IdTelefono = Seguridad.DesEncriptar(TelefonoEntidad.IdTelefono);
                    TelefonoEntidad.IdTipoTelefono = Seguridad.DesEncriptar(TelefonoEntidad.IdTipoTelefono);
                    TelefonoEntidad.IdPersona = Seguridad.DesEncriptar(TelefonoEntidad.IdPersona);
                    
                    respuesta = GestionTelefono.ModificarTelefono(TelefonoEntidad);
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje, respuesta };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
    }
}
