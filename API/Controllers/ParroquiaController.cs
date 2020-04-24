using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class ParroquiaController : ApiController
    {
        CatalogoParroquia GestionParroquia = new CatalogoParroquia();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoParroquia")]
        public object IngresoParroquia(ParroquiaEntidad ParroquiaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(ParroquiaEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    ParroquiaEntidad.IdCanton = Seguridad.DesEncriptar(ParroquiaEntidad.IdCanton);
                    
                    respuesta = GestionParroquia.IngresarParroquia(ParroquiaEntidad);
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
        [Route("api/TalentoHumano/EliminarParroquia")]
        public object EliminarParroquia(ParroquiaEntidad ParroquiaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(ParroquiaEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    ParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(ParroquiaEntidad.IdParroquia);
                   
                    respuesta = GestionParroquia.EliminarParroquia(int.Parse(ParroquiaEntidad.IdParroquia));
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
        [Route("api/TalentoHumano/ActualizarParroquia")]
        public object ActualizarParroquia(ParroquiaEntidad ParroquiaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(ParroquiaEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    ParroquiaEntidad.IdParroquia = Seguridad.DesEncriptar(ParroquiaEntidad.IdParroquia);
                    ParroquiaEntidad.IdCanton = Seguridad.DesEncriptar(ParroquiaEntidad.IdCanton);
                    
                    respuesta = GestionParroquia.ModificarParroquia(ParroquiaEntidad);
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
        [Route("api/TalentoHumano/ListaParroquia")]
        public object ListaParroquia([FromBody] Tokens Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveGet = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClaveGetEncripBD = p.desencriptar(Tokens.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    respuesta = GestionParroquia.ObtenerListaParroquia();
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
        [Route("api/TalentoHumano/ListaParroquiaCanton")]
        public object ListaParroquiaCanton(Canton Canton)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveGet = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClaveGetEncripBD = p.desencriptar(Canton.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Canton.IdCanton = Seguridad.DesEncriptar(Canton.IdCanton);
                    respuesta = GestionParroquia.ListarParroquiaCanton(int.Parse(Canton.IdCanton));
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
