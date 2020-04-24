using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Logica.Usuarios
{
    public class CatalogoUsuario
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<UsuarioEntidad> ListaDatos;
        Negocio.Metodos.Seguridad seguridad = new Metodos.Seguridad();
        public bool IngresarUsuario(UsuarioEntidad UsuarioEntidad)
        {
            if (ConexionBD.sp_ConsultarTodosLosUsuarios().ToList().Where(p => p.Usuario.Trim() == UsuarioEntidad.UsuarioLogin.Trim()).Count() > 0)
            {
                return false;
            }
            else
            {
                //return int.Parse(ConexionBD.sp_CrearUsuario(int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim()).Select(e => e.Value.ToString()).First());
                int.Parse(ConexionBD.sp_CrearUsuario(int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim()).Select(e => e.Value.ToString()).First());
                return true;
            }
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
        public bool ModificarUsuario(UsuarioEntidad UsuarioEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarUsuario(int.Parse(UsuarioEntidad.IdUsuario), int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            /*UsuarioEntidad Usuario = new UsuarioEntidad();
            //UsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(UsuarioEntidad.IdUsuario);
            Usuario = ListarUsuarios().Where(p => p.IdUsuario == UsuarioEntidad.IdUsuario).FirstOrDefault();
            if (Usuario!=null)
            {
                if (Usuario.UsuarioLogin == UsuarioEntidad.UsuarioLogin)
                {
                        ConexionBD.sp_ModificarUsuario(int.Parse(UsuarioEntidad.IdUsuario), int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim());
                        return true;                    
                }
                else
                {
                    if (ConexionBD.sp_ConsultarTodosLosUsuarios().ToList().Where(p => p.Usuario.Trim() == UsuarioEntidad.UsuarioLogin.Trim()).Count() > 0)
                    {
                        return false;
                    }
                    else
                    {
                        ConexionBD.sp_ModificarUsuario(int.Parse(UsuarioEntidad.IdUsuario), int.Parse(UsuarioEntidad.IdPersona), UsuarioEntidad.UsuarioLogin.Trim(), UsuarioEntidad.Contrasena.Trim());
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }*/
            
        }
        public void EliminarUsuario(int IdUsuarioEntidad)
        {
            ConexionBD.sp_EliminarUsuario(IdUsuarioEntidad);
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
    }
}
