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
using Negocio.Logica.Usuarios ;
using Negocio;

namespace API.Controllers
{
    public class UsuariosController : ApiController
    {
        static ConsultarUsuariosYPersonas GestionUsuarios = new ConsultarUsuariosYPersonas();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoUsuario GestionUsuario = new CatalogoUsuario();
        CatalogoTipoUsuario GestionTipoUsuario = new CatalogoTipoUsuario();
        Prueba p = new Prueba();

        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        [HttpPost]
        [Route("api/TalentoHumano/ListaUsuariosSistema")]
        public object ObtenerUsuariosSistema([FromBody] Tokens Tokens)
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
                    respuesta = GestionUsuarios.ObtenerListaUsuarios();
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
        [Route("api/TalentoHumano/ListaUsuariosClientes")]
        public object ObtenerUsuariosClientes([FromBody] Tokens Tokens)
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
                    respuesta = GestionUsuarios.ObtenerUsuariosClientes();
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
        [Route("api/TalentoHumano/ConsultarPersonasSinUsuario")]
        public object ListaUsuariosClientesInformacion([FromBody] Tokens Tokens)
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
                respuesta = GestionUsuarios.ObtenerUsuariosClientesInformacion();
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
        [Route("api/TalentoHumano/Login")]
        public object BuscarUsuarioSistema(Login Login)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePost = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClavePostEncripBD = p.desencriptar(Login.token, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{
                var UsuarioLogin = GestionUsuarios.LoginSistema(Login);
                    if (UsuarioLogin != null)
                    {
                        respuesta = UsuarioLogin;
                        mensaje = "EXITO";
                        codigo = "200";
                    }
                    else
                    {
                        mensaje = "ERROR";
                        codigo = "401";
                    }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { mensaje, codigo, respuesta };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";

                objeto = new { mensaje, codigo };
                return objeto;
            }


        }
        [HttpPost]
        [Route("api/Usuario/BuscarUsuarioExistente")]
        public object BuscarUsuarioExistente(Login Login)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePost = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClavePostEncripBD = p.desencriptar(Login.token, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{
                respuesta = GestionUsuarios.UsuarioExistente(Login.usuario);
                mensaje = "EXITO";
                codigo = "200";
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { mensaje, codigo, respuesta };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";

                objeto = new { mensaje, codigo };
                return objeto;
            }


        }

        [HttpPost]
        [Route("api/TalentoHumano/IngresoCredencial")]
        public object IngresoCredenciales(UsuarioEntidad UsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    UsuarioEntidad.Contrasena = p.encriptar(UsuarioEntidad.Contrasena, "Contrasena");
                    UsuarioEntidad.IdPersona = Seguridad.DesEncriptar(UsuarioEntidad.IdPersona);
                    respuesta = GestionUsuario.IngresarUsuario(UsuarioEntidad);

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
        [Route("api/TalentoHumano/EliminarCredencial")]
        public object EliminarCredencial(UsuarioEntidad UsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    GestionUsuario.EliminarUsuario(int.Parse(UsuarioEntidad.IdUsuario));
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
        [Route("api/TalentoHumano/ActualizarCredencial")]
        public object ActualizarCredencial(UsuarioEntidad UsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    UsuarioEntidad.Contrasena = p.encriptar(UsuarioEntidad.Contrasena, "Contrasena");
                    UsuarioEntidad.IdPersona = Seguridad.DesEncriptar(UsuarioEntidad.IdPersona);
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    
                    respuesta = GestionUsuario.ModificarUsuario(UsuarioEntidad);
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
        [Route("api/Usuario/HabilitarUsuario")]
        public object Habilitarusuario(UsuarioEntidad UsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                respuesta = GestionUsuarios.HabilitarUsuario(int.Parse(UsuarioEntidad.IdUsuario));
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
        [Route("api/Usuarios/ObtenerTipoUsuarioDeUnUsuario")]
        public object ObtenerTipoUsuarioDeunUsuario(UsuarioEntidad UsuarioEntidad)
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
                string ClaveGetEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                respuesta = GestionTipoUsuario.ObtenerListaTipoUsuarioDeUnUsuario(int.Parse(UsuarioEntidad.IdUsuario));
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
