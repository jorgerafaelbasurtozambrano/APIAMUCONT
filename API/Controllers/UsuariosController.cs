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
                if (Login.usuario == null || string.IsNullOrEmpty(Login.usuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el usuario";
                }
                else if(Login.contrasena == null || string.IsNullOrEmpty(Login.contrasena.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese la contrasena";
                }
                else
                {
                    UsuariosSistema DatoUsuariosSistema = new UsuariosSistema();
                    DatoUsuariosSistema = GestionUsuarios.LoginSistema(Login);
                    if (DatoUsuariosSistema.IdUsuario == null)
                    {
                        codigo = "500";
                        mensaje = "Las credenciales no son correctas";
                    }
                    else
                    {
                        respuesta = DatoUsuariosSistema;
                        mensaje = "EXITO";
                        codigo = "200";
                        objeto = new { mensaje, codigo, respuesta };
                        return objeto;
                    }

                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { mensaje, codigo};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
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
                if (UsuarioEntidad.UsuarioLogin == null || string.IsNullOrEmpty(UsuarioEntidad.UsuarioLogin.Trim()))
                {
                    mensaje = "Ingrese el usuario";
                    codigo = "418";
                }
                else if (UsuarioEntidad.Contrasena == null || string.IsNullOrEmpty(UsuarioEntidad.Contrasena.Trim()))
                {
                    mensaje = "Ingrese la contrasena";
                    codigo = "418";
                }
                else if (UsuarioEntidad.IdPersona == null || string.IsNullOrEmpty(UsuarioEntidad.IdPersona.Trim()))
                {
                    mensaje = "Seleccione la persona a asignar el usuario";
                    codigo = "418";
                }
                else
                {
                    UsuariosSistema DatoUsuariosSistema = new UsuariosSistema();
                    DatoUsuariosSistema = GestionUsuario.ConsultarUsuario(UsuarioEntidad.UsuarioLogin).FirstOrDefault();
                    if (DatoUsuariosSistema == null)
                    {
                        UsuarioEntidad.Contrasena = p.encriptar(UsuarioEntidad.Contrasena, "Contrasena");
                        UsuarioEntidad.IdPersona = Seguridad.DesEncriptar(UsuarioEntidad.IdPersona);
                        DatoUsuariosSistema = new UsuariosSistema();
                        DatoUsuariosSistema = GestionUsuario.IngresarUsuario(UsuarioEntidad);
                        if (DatoUsuariosSistema.IdUsuario == null || string.IsNullOrEmpty(DatoUsuariosSistema.IdUsuario.Trim()))
                        {
                            codigo = "500";
                            mensaje = "Ocurrio un error al ingresar el usuario";
                        }
                        else
                        {
                            codigo = "200";
                            mensaje = "EXITO";
                            respuesta = DatoUsuariosSistema;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        mensaje = "El usuario"+ DatoUsuariosSistema.UsuarioLogin + " no esta disponible porque ya esta siendo usado";
                        codigo = "418";
                    }
                }

                //}
                //else
                //{
                    //mensaje = "ERROR";
                    //codigo = "401";
                //}
                objeto = new { codigo, mensaje};
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
                if (UsuarioEntidad.IdUsuario == null || string.IsNullOrEmpty(UsuarioEntidad.IdUsuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id usuario";
                }
                else
                {
                    UsuariosSistema Usuario = new UsuariosSistema();
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    Usuario = GestionUsuario.ConsultarUsuarioPorId(int.Parse(UsuarioEntidad.IdUsuario)).FirstOrDefault();
                    if (Usuario == null)
                    {
                        codigo = "418";
                        mensaje = "El usuario que intenta eliminar no existe";
                    }
                    else
                    {
                        if (Usuario.EstadoUsuario == false)
                        {
                            codigo = "500";
                            mensaje = "El usuario "+ Usuario.UsuarioLogin+ " ya esta inhabilitado";
                        }
                        else
                        {
                            if (GestionUsuario.EliminarUsuario(int.Parse(UsuarioEntidad.IdUsuario)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                mensaje = "Ocurrio un error al tratar de eliminar el usuario";
                                codigo = "500";
                            }
                        }
                    }
                }
                //}
                //else
                //{
                    //mensaje = "ERROR";
                    //codigo = "401";
                //}
                objeto = new { codigo, mensaje};
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
                if (UsuarioEntidad.IdUsuario == null  || string.IsNullOrEmpty(UsuarioEntidad.IdUsuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id usuario";
                }
                else if (UsuarioEntidad.IdPersona == null || string.IsNullOrEmpty(UsuarioEntidad.IdPersona.Trim()))
                {
                    codigo = "418";
                    mensaje = "ingrese el id persona";
                }
                else if (UsuarioEntidad.UsuarioLogin == null || string.IsNullOrEmpty(UsuarioEntidad.UsuarioLogin.Trim()))
                {
                    codigo = "418";
                    mensaje = "ingrese el usuario";
                }
                else if (UsuarioEntidad.Contrasena == null || string.IsNullOrEmpty(UsuarioEntidad.Contrasena.Trim()))
                {
                    codigo = "418";
                    mensaje = "ingrese la contraseña";
                }
                else
                {
                    UsuarioEntidad.Contrasena = p.encriptar(UsuarioEntidad.Contrasena, "Contrasena");
                    UsuarioEntidad.IdPersona = Seguridad.DesEncriptar(UsuarioEntidad.IdPersona);
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    UsuariosSistema DatoUsuario = new UsuariosSistema();
                    DatoUsuario = GestionUsuario.ConsultarUsuarioPorId(int.Parse(UsuarioEntidad.IdUsuario)).FirstOrDefault();
                    if (DatoUsuario == null)
                    {
                        codigo = "418";
                        mensaje = "El usuario que intenta modificar no existe";
                    }
                    else
                    {
                        UsuarioEntidad.UsuarioLogin = DatoUsuario.UsuarioLogin;
                        DatoUsuario = new UsuariosSistema();
                        DatoUsuario = GestionUsuario.ModificarUsuario(UsuarioEntidad);
                        if (DatoUsuario.IdUsuario == null)
                        {
                            mensaje = "Ocurrio un error al modificar el usuario";
                            codigo = "500";
                        }
                        else
                        {
                            codigo = "200";
                            mensaje = "EXITO";
                            respuesta = DatoUsuario;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }

                }  
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje};
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
                
                if (UsuarioEntidad.IdUsuario == null || string.IsNullOrEmpty(UsuarioEntidad.IdUsuario.Trim()))
                {
                    mensaje = "Ingrese el id usuario";
                    codigo = "418";
                }
                else
                {
                    UsuariosSistema Usuario = new UsuariosSistema();
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    Usuario = GestionUsuario.ConsultarUsuarioPorId(int.Parse(UsuarioEntidad.IdUsuario)).FirstOrDefault();
                    if (Usuario == null)
                    {
                        codigo = "418";
                        mensaje = "El usuario que intenta habilitar no existe";
                    }
                    else
                    {
                        if (Usuario.EstadoUsuario == true)
                        {
                            codigo = "418";
                            mensaje = "El usuario"+ Usuario.UsuarioLogin+ " ya se encuentra habilitado";
                        }
                        else
                        {
                            if (GestionUsuarios.HabilitarUsuario(int.Parse(UsuarioEntidad.IdUsuario)) == true)
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al tratar de habilitar el usuario";
                            }
                        }
                    }
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje};
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


        [HttpPost]
        [Route("api/Usuario/ConsultarTiposUsuarioQueNoTieneUnUsuario")]
        public object ConsultarTiposUsuarioQueNoTieneUnUsuario(UsuarioEntidad UsuarioEntidad)
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
                string ClavePostEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{
                if (UsuarioEntidad.IdUsuario == null || string.IsNullOrEmpty(UsuarioEntidad.IdUsuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "ingrese el id usuario";
                }
                else
                {
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    mensaje = "EXITO";
                    codigo = "200";
                    respuesta = GestionUsuario.ConsultarTiposUsuarioQueNoTieneUnUsuario(int.Parse(UsuarioEntidad.IdUsuario));
                    objeto = new { mensaje, codigo, respuesta };
                    return objeto;
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { mensaje, codigo};
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { mensaje, codigo };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/Usuario/ConsultarTiposUsuarioQueTieneUnUsuario")]
        public object ConsultarTiposUsuarioQueTieneUnUsuario(UsuarioEntidad UsuarioEntidad)
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
                string ClavePostEncripBD = p.desencriptar(UsuarioEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{
                if (UsuarioEntidad.IdUsuario == null || string.IsNullOrEmpty(UsuarioEntidad.IdUsuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "ingrese el id usuario";
                }
                else
                {
                    UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                    mensaje = "EXITO";
                    codigo = "200";
                    respuesta = GestionUsuario.ConsultarTiposUsuarioQueTieneUnUsuario(int.Parse(UsuarioEntidad.IdUsuario));
                    objeto = new { mensaje, codigo, respuesta };
                    return objeto;
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { mensaje, codigo };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { mensaje, codigo };
                return objeto;
            }
        }
    }
}
