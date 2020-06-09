using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoComunidad
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Comunidad> ListaComunidad;
        public void CatalogoComunidadObtener()
        {
            ListaComunidad = new List<Comunidad>();
            foreach (var item in ConexionBD.sp_ConsultarComunidad().Where(p=>p.Estado != false && p.ParroquiaEstado != false).ToList())
            {
                bool estadoComunidadEliminacion = false;
                if (item.ComunidadUtilizado == "0")
                {
                    estadoComunidadEliminacion = true;
                }
                else
                {
                    estadoComunidadEliminacion = false;
                }
                ListaComunidad.Add(new Comunidad()
                {
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    Descripcion = item.Descripcion,
                    PermitirEliminacion = estadoComunidadEliminacion,
                    Parroquia = new Parroquia()
                    {
                        IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                        Descripcion = item.ParroquiaDescripcion,
                        Estado = item.ParroquiaEstado,
                    },
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
        }
        public Comunidad IngresarComunidad(ComunidadEntidad ComunidadEntidad)
        {
            Comunidad _DatoComunidad = new Comunidad();
            foreach (var item in ConexionBD.sp_CrearComunidad(int.Parse(ComunidadEntidad.IdParroquia), ComunidadEntidad.Descripcion.ToUpper()))
            {
                _DatoComunidad.IdComunidad = Seguridad.Encriptar(item.ComunidadIdComunidad.ToString());
                _DatoComunidad.Descripcion = item.ComunidadDescripcion;
                _DatoComunidad.PermitirEliminacion = true;
                _DatoComunidad.FechaCreacion = item.ComunidadFechaCreacion;
                _DatoComunidad.Estado = item.ComunidadEstado;
                _DatoComunidad.Parroquia = new Parroquia()
                {
                     IdParroquia = Seguridad.Encriptar(item.ParroquiaIdParroquia.ToString()),
                     Descripcion = item.ParroquiaDescripcion,
                     Estado = item.ParroquiaEstado,
                };
            }
            return _DatoComunidad;
        }
        public bool EliminarComunidad(int IdComunidad)
        {
            try
            {
                ConexionBD.sp_EliminarComunidad(IdComunidad);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Comunidad ModificarComunidad(ComunidadEntidad ComunidadEntidad)
        {
            Comunidad _DatoComunidad = new Comunidad();
            try
            {
                foreach (var item in ConexionBD.sp_ModificarComunidad(int.Parse(ComunidadEntidad.IdComunidad), ComunidadEntidad.Descripcion.ToUpper(), int.Parse(ComunidadEntidad.IdParroquia)))
                {
                    bool PermitirEliminar = false;
                    if (item.ComunidadUtilizado == "0")
                    {
                        PermitirEliminar = true;
                    }
                    else
                    {
                        PermitirEliminar = false;
                    }
                    _DatoComunidad.IdComunidad = Seguridad.Encriptar(item.ComunidadIdComunidad.ToString());
                    _DatoComunidad.Descripcion = item.ComunidadDescripcion;
                    _DatoComunidad.PermitirEliminacion = PermitirEliminar;
                    _DatoComunidad.FechaCreacion = item.ComunidadFechaCreacion;
                    _DatoComunidad.Estado = item.ComunidadEstado;
                    _DatoComunidad.Parroquia = new Parroquia()
                    {
                        IdParroquia = Seguridad.Encriptar(item.ParroquiaIdParroquia.ToString()),
                        Descripcion = item.ParroquiaDescripcion,
                        Estado = item.ParroquiaEstado,
                    };
                }
                return _DatoComunidad;
            }
            catch (Exception)
            {
                _DatoComunidad.IdComunidad = null;
                return _DatoComunidad;
            }
        }
        public List<Comunidad> ObtenerListaComunidad()
        {
            CatalogoComunidadObtener();
            return ListaComunidad.GroupBy(a => a.IdComunidad).Select(grp => grp.First()).ToList();
        }
        public List<Comunidad> ListarComunidadParroquia(int IdParroquia)
        {
            List<Comunidad> ListaComunidades = new List<Comunidad>();
            foreach (var item in ConexionBD.ConsultarComunidadesDeUnaParroquia(IdParroquia))
            {
                ListaComunidades.Add(new Comunidad()
                {
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    Descripcion = item.Descripcion,
                    Parroquia = null,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaComunidades;
        }
        public List<Comunidad> ConsultarComunidadPorDescripcion(ComunidadEntidad _ComunidadEntidad)
        {
            List<Comunidad> ListaComunidad = new List<Comunidad>();
            foreach (var item in ConexionBD.sp_ConsultarComunidadSiexiste(_ComunidadEntidad.Descripcion,int.Parse(_ComunidadEntidad.IdParroquia)))
            {
                ListaComunidad.Add(new Comunidad()
                {
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    Descripcion = item.Descripcion,
                });
            }
            return ListaComunidad;
        }
        public List<Comunidad> ConsultarComunidadPorId(int IdComunidad)
        {
            List<Comunidad> ListaComunidad = new List<Comunidad>();
            foreach (var item in ConexionBD.sp_ConsultarComunidadPorId(IdComunidad))
            {
                bool permitirEliminar = false;
                if (item.ComunidadUtilizado == "0")
                {
                    permitirEliminar = true;
                }
                else
                {
                    permitirEliminar = false;
                }
                ListaComunidad.Add(new Comunidad()
                {
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    PermitirEliminacion = permitirEliminar
                });
            }
            return ListaComunidad;
        }

    }
}
