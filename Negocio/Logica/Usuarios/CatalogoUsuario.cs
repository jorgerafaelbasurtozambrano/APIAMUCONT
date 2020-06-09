using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoUsuario
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<UsuarioEntidad> ListaDatos;
        Negocio.Metodos.Seguridad seguridad = new Metodos.Seguridad();
        public UsuariosSistema IngresarUsuario(UsuarioEntidad UsuarioEntidad)
        {
            UsuariosSistema DatoUsuariosSistema = new UsuariosSistema();
            foreach (var item in ConexionBD.sp_CrearUsuario(int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim()))
            {
                DatoUsuariosSistema.IdPersona = seguridad.Encriptar(item.PersonaIdPersona.ToString());
                DatoUsuariosSistema.NumeroDocumento = item.PersonaNumeroDocumento;
                DatoUsuariosSistema.ApellidoPaterno = item.PersonaApellidoPaterno;
                DatoUsuariosSistema.ApellidoMaterno = item.PersonaApellidoMaterno;
                DatoUsuariosSistema.PrimerNombre = item.PersonaPrimerNombre;
                DatoUsuariosSistema.SegundoNombre = item.PersonaSegundoNombre;

                DatoUsuariosSistema.IdUsuario = seguridad.Encriptar(item.UsuarioIdUsuario.ToString());
                DatoUsuariosSistema.UsuarioLogin = item.Usuario;
                DatoUsuariosSistema.Contrasena = item.Contrasena;
                DatoUsuariosSistema.EstadoUsuario = item.UsuarioEstado;
            }
            return DatoUsuariosSistema;
        }

        public List<UsuariosSistema> ConsultarUsuario(string UsuarioLogin)
        {
            List<UsuariosSistema> ListaDatos = new List<UsuariosSistema>();
            foreach (var item in ConexionBD.sp_ConsultarUsuarioPorUsuario(UsuarioLogin))
            {
                ListaDatos.Add(new UsuariosSistema()
                {
                    IdPersona = seguridad.Encriptar(item.PersonaIdPersona.ToString()),
                    NumeroDocumento = item.PersonaNumeroDocumento,
                    ApellidoPaterno = item.PersonaApellidoPaterno,
                    ApellidoMaterno = item.PersonaApellidoMaterno,
                    PrimerNombre = item.PersonaPrimerNombre,
                    SegundoNombre = item.PersonaSegundoNombre,
                    IdUsuario = seguridad.Encriptar(item.UsuarioIdUsuario.ToString()),
                    UsuarioLogin = item.UsuarioUsuario,
                    Contrasena = item.UsuarioContrasena,
                    EstadoUsuario = item.UsuarioEstado,
                });
            }
            return ListaDatos;
        }
        private List<UsuarioEntidad> ListarUsuarios()
        {
            List<UsuarioEntidad> Lista = new List<UsuarioEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarTodosLosUsuarios())
            {
                Lista.Add(new UsuarioEntidad()
                {
                    IdUsuario = item.IdUsuario.ToString(),
                    UsuarioLogin = item.Usuario,
                });
            }
            return Lista;
        }
        public UsuariosSistema ModificarUsuario(UsuarioEntidad UsuarioEntidad)
        {
            UsuariosSistema DatoUsuariosSistema = new UsuariosSistema();
            try
            {
                foreach (var item in ConexionBD.sp_ModificarUsuario(int.Parse(UsuarioEntidad.IdUsuario), int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim()))
                {
                    DatoUsuariosSistema.IdPersona = seguridad.Encriptar(item.PersonaIdPersona.ToString());
                    DatoUsuariosSistema.NumeroDocumento = item.PersonaNumeroDocumento;
                    DatoUsuariosSistema.ApellidoPaterno = item.PersonaApellidoPaterno;
                    DatoUsuariosSistema.ApellidoMaterno = item.PersonaApellidoMaterno;
                    DatoUsuariosSistema.PrimerNombre = item.PersonaPrimerNombre;
                    DatoUsuariosSistema.SegundoNombre = item.PersonaSegundoNombre;

                    DatoUsuariosSistema.IdUsuario = seguridad.Encriptar(item.UsuarioIdUsuario.ToString());
                    DatoUsuariosSistema.UsuarioLogin = item.Usuario;
                    DatoUsuariosSistema.Contrasena = item.Contrasena;
                    DatoUsuariosSistema.EstadoUsuario = item.UsuarioEstado;
                }
                return DatoUsuariosSistema;
            }
            catch (Exception)
            {
                DatoUsuariosSistema.IdUsuario = null;
                return DatoUsuariosSistema;
            }
        }
        public bool EliminarUsuario(int IdUsuarioEntidad)
        {
            try
            {
                ConexionBD.sp_EliminarUsuario(IdUsuarioEntidad);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public UsuarioEntidad ObtenerUsuario(int IdUsuarioEntidad)
        {
            CargarDatos();
            for (int i = 0; i < ListaDatos.Count; i++)
            {
                ListaDatos[i].IdUsuario = seguridad.DesEncriptar(ListaDatos[i].IdUsuario);
            }
            UsuarioEntidad Usuario = new UsuarioEntidad();
            Usuario = ListaDatos.Where(p => p.IdUsuario == IdUsuarioEntidad.ToString()).FirstOrDefault();
            if (Usuario != null)
            {
                Usuario.IdUsuario = seguridad.DesEncriptar(Usuario.IdUsuario);
            }
            else
            {
                Usuario = null;
            }
            return Usuario;
        }
        public void CargarDatos()
        {
            ListaDatos = new List<UsuarioEntidad>();
            foreach (var item in ConexionBD.sp_ConsultarUsuario())
            {
                ListaDatos.Add(new UsuarioEntidad()
                {
                    IdUsuario = Seguridad.Encriptar(item.IdUsuario.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    UsuarioLogin = item.Usuario,
                    Contrasena = item.Contrasena,
                });
            }
        }


        public List<TipoUsuario> ConsultarTiposUsuarioQueNoTieneUnUsuario(int idUsuario)
        {
            List<TipoUsuario> Tipousuarios = new List<TipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarTiposDeUsuariosQueNoTieneUnUsuario(idUsuario))
            {
                Tipousuarios.Add(new TipoUsuario()
                {
                    IdTipoUsuario = seguridad.Encriptar(item.IdTipoUsuario.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    FechaCreacion = item.FechaCreacion
                });
            }
            return Tipousuarios;
        }
        public List<AsignacionTipoUsuario> ConsultarTiposUsuarioQueTieneUnUsuario(int idUsuario)
        {
            List<AsignacionTipoUsuario> ListaAsignarTipoUsuario = new List<AsignacionTipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarTiposDeUsuariosQueTieneUnUsuario(idUsuario))
            {
                ListaAsignarTipoUsuario.Add(new AsignacionTipoUsuario()
                {
                    IdAsignacionTUEncriptada = seguridad.Encriptar(item.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                    Estado = item.AsignacionTipoUsuarioEstado,
                    FechaCreacion = item.AsignacionTipoUsuarioFechaCreacion,
                    TipoUsuario = new TipoUsuario()
                    {
                        IdTipoUsuario = seguridad.Encriptar(item.TipoUsuarioIdTipoUsuario.ToString()),
                        Descripcion = item.TipoUsuarioDescripcion,
                        Estado = item.TipoUsuarioEstado,
                        FechaCreacion = item.TipoUsuarioFechaCreacion
                    }
                });
            }
            return ListaAsignarTipoUsuario;
        }
        public List<UsuariosSistema> ConsultarUsuarioPorId(int idUsuario)
        {
            List<UsuariosSistema> Usuarios = new List<UsuariosSistema>();
            foreach (var item in ConexionBD.sp_ConsultarUsuarioPorId(idUsuario))
            {
                Usuarios.Add(new UsuariosSistema()
                {
                    IdUsuario = seguridad.Encriptar(item.IdUsuario.ToString()),
                    UsuarioLogin = item.Usuario,
                    Contrasena = item.Contrasena,
                    EstadoUsuario = item.Estado
                });
            }
            return Usuarios;
        }

    }
}
