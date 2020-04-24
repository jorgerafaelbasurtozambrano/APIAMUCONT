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
    public class AsignacionPersonaComunidadController : ApiController
    {
        CatalogoAsigancionPersonaComunidad GestionAsignacionPersonaComunidad = new CatalogoAsigancionPersonaComunidad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoAsignacionPersonaParroquia")]
        public object IngresoAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePost = ListaClaves.Where(c => c.Identificador == 1).FirstOrDefault();
                Object resultado = new object();
                string ClavePostEncripBD = p.desencriptar(AsignacionPersonaParroquiaEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                AsignacionPersonaParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdParroquia);
                AsignacionPersonaParroquiaEntidad.IdPersona = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdPersona);
                
                respuesta = GestionAsignacionPersonaComunidad.IngresoAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad);
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
        [Route("api/TalentoHumano/EliminarAsignacionPersonaParroquia")]
        public object EliminarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaComunidadEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionPersonaComunidadEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    AsignacionPersonaComunidadEntidad.IdAsignacionPC = Seguridad.DesEncriptar(AsignacionPersonaComunidadEntidad.IdAsignacionPC);
                    respuesta = GestionAsignacionPersonaComunidad.EliminarAsignacionPersonaComunidad(int.Parse(AsignacionPersonaComunidadEntidad.IdAsignacionPC));

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
        [Route("api/TalentoHumano/ActualizarAsignacionPersonaParroquia")]
        public object ActualizarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionPersonaParroquiaEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    AsignacionPersonaParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdParroquia);
                    AsignacionPersonaParroquiaEntidad.IdPersona = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdPersona);
                    AsignacionPersonaParroquiaEntidad.IdAsignacionPC = Seguridad.DesEncriptar(AsignacionPersonaParroquiaEntidad.IdAsignacionPC);
                    respuesta = GestionAsignacionPersonaComunidad.ModificarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad);

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
