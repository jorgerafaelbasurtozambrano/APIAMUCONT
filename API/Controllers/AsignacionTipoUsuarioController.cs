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
using Negocio.Logica.Usuarios;
using Negocio;
namespace API.Controllers
{
    public class AsignacionTipoUsuarioController : ApiController
    {
        static ConsultarUsuariosYPersonas GestionUsuarios = new ConsultarUsuariosYPersonas();
        CatalogoAsignacionTipoUsuario GestionTipoUsuario = new CatalogoAsignacionTipoUsuario();
        Prueba p = new Prueba();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        [HttpPost]
        [Route("api/TalentoHumano/IngresoTipoUsuario")]
        public object IngresoTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionTipoUsuarioEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    AsignacionTipoUsuarioEntidad.IdTipoUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdTipoUsuario);
                    AsignacionTipoUsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdUsuario);
                    respuesta = GestionTipoUsuario.crearAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad);

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
        [Route("api/TalentoHumano/ActualizarAsignacionTipoUsuario")]
        public object ActualizarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionTipoUsuarioEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                AsignacionTipoUsuarioEntidad.IdTipoUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdTipoUsuario);
                AsignacionTipoUsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdUsuario);
                GestionTipoUsuario.modificarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad);
                respuesta = AsignacionTipoUsuarioEntidad;
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
        [Route("api/TalentoHumano/EliminarAsignacionTipoUsuario")]
        public object EliminarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionTipoUsuarioEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                AsignacionTipoUsuarioEntidad.IdAsignacionTU = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdAsignacionTU);
                GestionTipoUsuario.eliminarAsignacionTipoUsuario(int.Parse(AsignacionTipoUsuarioEntidad.IdAsignacionTU));
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
    }
}
