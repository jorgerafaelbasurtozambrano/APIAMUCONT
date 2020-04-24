using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class CorreoController : ApiController
    {
        CatalogoCorreo GestionCorreo = new CatalogoCorreo();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoCorreo")]
        public object IngresoCorreo(Correo Correo)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                //var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                //var _clavePost = ListaClaves.Where(c => c.Identificador == 1).FirstOrDefault();
                //Object resultado = new object();
                //string ClavePutEncripBD = p.desencriptar(Correo.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                //string variable = Correo.CorreoValor.ToString();
                if (Correo.CorreoValor != null)
                {
                    Correo.IdPersona = Seguridad.DesEncriptar(Correo.IdPersona);
                    respuesta = GestionCorreo.IngresoCorreo(Correo);
                }
                else
                {
                    respuesta = true;
                }
                    
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
        [Route("api/TalentoHumano/EliminarCorreo")]
        public object EliminarCorreo(Correo Correo)
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
                string ClavePutEncripBD = p.desencriptar(Correo.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Correo.IdCorreo = Seguridad.DesEncriptar(Correo.IdCorreo);
                    
                    respuesta = GestionCorreo.EliminarCorreo(int.Parse(Correo.IdCorreo));
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
        [Route("api/TalentoHumano/ActualizarCorreo")]
        public object ActualizarCorreo(Correo Correo)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                //var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                //var _clavePut = ListaClaves.Where(c => c.Identificador == 2).FirstOrDefault();
                Object resultado = new object();
                //string ClavePutEncripBD = p.desencriptar(Correo.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                if (Correo.IdCorreo == null)
                {
                    Correo.IdPersona = Seguridad.DesEncriptar(Correo.IdPersona);
                    respuesta = GestionCorreo.IngresoCorreo(Correo);
                }
                else
                {
                    Correo.IdCorreo = Seguridad.DesEncriptar(Correo.IdCorreo);
                    Correo.IdPersona = Seguridad.DesEncriptar(Correo.IdPersona);
                    respuesta = GestionCorreo.ModificarCorreo(Correo);
                    respuesta = true;
                }

                
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
