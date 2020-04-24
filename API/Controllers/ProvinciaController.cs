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
    public class ProvinciaController : ApiController
    {
        CatalogoProvincia GestionProvincia = new CatalogoProvincia();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoProvincia")]
        public object IngresoProvincia(Provincia Provincia)
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
                string ClavePutEncripBD = p.desencriptar(Provincia.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";                
                    GestionProvincia.IngresoProvincia(Provincia);
                    respuesta = GestionProvincia.IngresoProvincia(Provincia);
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
        [Route("api/TalentoHumano/EliminarProvincia")]
        public object EliminarProvincia(Provincia Provincia)
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
                string ClavePutEncripBD = p.desencriptar(Provincia.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                    
                    respuesta = GestionProvincia.EliminarProvincia(int.Parse(Provincia.IdProvincia));
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
        [Route("api/TalentoHumano/ActualizarProvincia")]
        public object ActualizarProvincia(Provincia Provincia)
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
                string ClavePutEncripBD = p.desencriptar(Provincia.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                    
                    respuesta = GestionProvincia.ModificarProvincia(Provincia);
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
        [Route("api/TalentoHumano/ListaProvincia")]
        public object ObtenerProvincias([FromBody] Tokens Tokens)
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
                    respuesta = GestionProvincia.ConsultarProvincias();
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
