using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Entidades.DatoUsuarios;
using Datos;
using Negocio.Entidades;

namespace Negocio.Logica.Usuarios
{
    public class CatalogoAsignacionTipoUsuario
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        CatalogoTipoUsuario GestionTipoUsuario = new CatalogoTipoUsuario();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public AsignacionTipoUsuario crearAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
        {
            AsignacionTipoUsuario DatoAsignacionTipoUsuario = new AsignacionTipoUsuario();
            foreach (var item in ConexionBD.sp_CrearAsignacionTU(int.Parse(AsignacionTipoUsuarioEntidad.IdUsuario), int.Parse(AsignacionTipoUsuarioEntidad.IdTipoUsuario)))
            {
                DatoAsignacionTipoUsuario.IdAsignacionTUEncriptada = Seguridad.Encriptar(item.AsignacionTipoUsuarioIdAsignacionTU.ToString());
                DatoAsignacionTipoUsuario.Estado = item.AsignacionTipoUsuarioEstado;
                DatoAsignacionTipoUsuario.FechaCreacion = item.AsignacionTipoUsuarioFechaCreacion;
                DatoAsignacionTipoUsuario.TipoUsuario = new TipoUsuario()
                {
                    IdTipoUsuario = Seguridad.Encriptar(item.TipoUsuarioIdTipoUsuario.ToString()),
                    Descripcion = item.TipoUsuarioDescripcion,
                    Estado = item.TipoUsuarioEstado,
                    FechaCreacion = item.TipoUsuarioFechaCreacion
                };
            }
            return DatoAsignacionTipoUsuario;
        }
        public bool modificarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarAsignacionTU(int.Parse(AsignacionTipoUsuarioEntidad.IdAsignacionTU),int.Parse(AsignacionTipoUsuarioEntidad.IdUsuario), int.Parse(AsignacionTipoUsuarioEntidad.IdTipoUsuario));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool eliminarAsignacionTipoUsuario(int IdAsignacionTU)
        {
            try
            {
                ConexionBD.sp_EliminarAsignacionTU(IdAsignacionTU);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<AsignacionTipoUsuario> ConsultarTiposUsuarioQueTieneUnUsuario(int idUsuario)
        {
            List<AsignacionTipoUsuario> ListaAsignarTipoUsuario = new List<AsignacionTipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarTiposDeUsuariosQueTieneUnUsuario(idUsuario))
            {
                ListaAsignarTipoUsuario.Add(new AsignacionTipoUsuario()
                {
                    IdAsignacionTUEncriptada = Seguridad.Encriptar(item.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                    Estado = item.AsignacionTipoUsuarioEstado,
                    FechaCreacion = item.AsignacionTipoUsuarioFechaCreacion,
                    TipoUsuario = new TipoUsuario()
                    {
                        IdTipoUsuario = Seguridad.Encriptar(item.TipoUsuarioIdTipoUsuario.ToString()),
                        Descripcion = item.TipoUsuarioDescripcion,
                        Estado = item.TipoUsuarioEstado,
                        FechaCreacion = item.TipoUsuarioFechaCreacion
                    }
                });
            }
            return ListaAsignarTipoUsuario;
        }
        public List<AsignacionTipoUsuario> ConsultarAsignarTUPorId(int idAsignacionTU)
        {
            List<AsignacionTipoUsuario> ListaAsignarTipoUsuario = new List<AsignacionTipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarAsignacionTUPorId(idAsignacionTU))
            {
                ListaAsignarTipoUsuario.Add(new AsignacionTipoUsuario()
                {
                    IdAsignacionTUEncriptada = Seguridad.Encriptar(item.IdAsignacionTU.ToString()),
                    Estado = item.Estado,
                    FechaCreacion = item.FechaCreacion,
                    IdUsuario = item.IdUsuario
                });
            }
            return ListaAsignarTipoUsuario;
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
    }
}
