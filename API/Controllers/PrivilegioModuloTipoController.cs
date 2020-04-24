using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Logica.Usuarios;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class PrivilegioModuloTipoController : ApiController
    {
        CatalogoPrivilegioModuloTipo GestionPrivilegioModuloTipo = new CatalogoPrivilegioModuloTipo();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();


        [HttpPost]
        [Route("api/Usuarios/IngresoPrivilegioModuloTipo")]
        public object IngresoPrivilegioModuloTipo(PrivilegioModuloTipoEntidad PrivilegioModuloTipoEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PrivilegioModuloTipoEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    GestionPrivilegioModuloTipo.IngresarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad);
                    respuesta = "successful";
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
        [Route("api/Usuarios/EliminarPrivilegioModuloTipo")]
        public object EliminarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad PrivilegioModuloTipoEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PrivilegioModuloTipoEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    GestionPrivilegioModuloTipo.EliminarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad.IdPrivilegioModuloTipo);
                    respuesta = "successful";
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
        [Route("api/Usuarios/ActualizarPrivilegioModuloTipo")]
        public object ActualizarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad PrivilegioModuloTipoEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PrivilegioModuloTipoEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    GestionPrivilegioModuloTipo.ModificarPrivilegioModuloTipo(PrivilegioModuloTipoEntidad);
                    respuesta = PrivilegioModuloTipoEntidad;
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
