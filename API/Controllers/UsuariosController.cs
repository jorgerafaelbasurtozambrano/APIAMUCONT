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
using Negocio.Logica.Credito;

namespace API.Controllers
{
    public class UsuariosController : ApiController
    {
        static ConsultarUsuariosYPersonas GestionUsuarios = new ConsultarUsuariosYPersonas();
        CatalogoAsignarComunidadFactura _GestionAsignarComunidadConfigurarVenta = new CatalogoAsignarComunidadFactura();
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
                if (Tokens.encriptada == null || string.IsNullOrEmpty(Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = GestionUsuarios.ObtenerListaUsuarios();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
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
        [Route("api/TalentoHumano/ListaUsuariosClientes")]
        public object ObtenerUsuariosClientes([FromBody] Tokens Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (Tokens.encriptada == null || string.IsNullOrEmpty(Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = GestionUsuarios.ObtenerUsuariosClientes();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje};
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
                if (Tokens.encriptada == null || string.IsNullOrEmpty(Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = GestionUsuarios.ObtenerUsuariosClientesInformacion();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
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
        [Route("api/TalentoHumano/Login")]
        public object BuscarUsuarioSistema(Login Login)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
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
                        string Token = "";
                        if (DatoUsuariosSistema.Token == null)
                        {
                            Token = Seguridad.setTokenUsuario(DatoUsuariosSistema);
                        }
                        else
                        {
                            Token = DatoUsuariosSistema.Token;
                        }
                        respuesta = DatoUsuariosSistema;
                        mensaje = "EXITO";
                        codigo = "200";
                        objeto = new { mensaje, codigo, respuesta, Token };
                        return objeto;
                    }
                }
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
                if (Login.encriptada == null || string.IsNullOrEmpty(Login.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(Login.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        respuesta = GestionUsuarios.UsuarioExistente(Login.usuario);
                        mensaje = "EXITO";
                        codigo = "200";
                        objeto = new { mensaje, codigo, respuesta };
                        return objeto;
                    }
                }
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
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                                mensaje = "El usuario " + DatoUsuariosSistema.UsuarioLogin + " no esta disponible porque ya esta siendo usado";
                                codigo = "418";
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
        [Route("api/TalentoHumano/EliminarCredencial")]
        public object EliminarCredencial(UsuarioEntidad UsuarioEntidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                                    mensaje = "El usuario " + Usuario.UsuarioLogin + " ya esta inhabilitado";
                                }
                                else
                                {
                                    List<AsignacionTipoUsuario> ListaTipoUsuario = new List<AsignacionTipoUsuario>();
                                    ListaTipoUsuario = GestionUsuario.ConsultarTiposUsuarioQueTieneUnUsuario(int.Parse(UsuarioEntidad.IdUsuario));
                                    List<PersonaEntidad> PersonasAsignadas = new List<PersonaEntidad>();
                                    foreach (var item in ListaTipoUsuario)
                                    {
                                        PersonasAsignadas = new List<PersonaEntidad>();
                                        PersonasAsignadas = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasAsignadasPorTecnico(int.Parse(Seguridad.DesEncriptar(item.IdAsignacionTUEncriptada)));
                                        if (PersonasAsignadas.Count > 0)
                                        {
                                            codigo = "409";
                                            mensaje = "No se puede eliminar el rol " + item.TipoUsuario.Descripcion + " porque tiene cliente para seguimiento asignados, por favor vaya a la seccion de trasnferencia de técnico";
                                            respuesta = PersonasAsignadas;
                                            objeto = new { codigo, mensaje, respuesta };
                                            return objeto;
                                        }
                                    }
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
                    }
                }
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
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (UsuarioEntidad.IdUsuario == null || string.IsNullOrEmpty(UsuarioEntidad.IdUsuario.Trim()))
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
                                    if (UsuarioEntidad.Cerrar=="1")
                                    {
                                        Seguridad.EliminarTokenUsuario(int.Parse(UsuarioEntidad.IdUsuario));
                                    }
                                    codigo = "200";
                                    mensaje = "EXITO";
                                    respuesta = DatoUsuario;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                        }
                    }
                }
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
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                                    mensaje = "El usuario" + Usuario.UsuarioLogin + " ya se encuentra habilitado";
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
                    }
                }
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
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
                        respuesta = GestionTipoUsuario.ObtenerListaTipoUsuarioDeUnUsuario(int.Parse(UsuarioEntidad.IdUsuario));
                        objeto = new { codigo, mensaje,respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje };
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
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                    }
                }
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
                if (UsuarioEntidad.encriptada == null || string.IsNullOrEmpty(UsuarioEntidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(UsuarioEntidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                    }
                }
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
