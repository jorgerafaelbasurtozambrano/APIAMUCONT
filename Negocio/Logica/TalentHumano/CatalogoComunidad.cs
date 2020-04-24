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
                ListaComunidad.Add(new Comunidad()
                {
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    Descripcion = item.Descripcion,
                    ComunidadUtilizado = item.ComunidadUtilizado,
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
        public string IngresarComunidad(ComunidadEntidad ComunidadEntidad)
        {
            try
            {
                Comunidad Comunidad1 = ObtenerListaComunidad().Where(p => p.Descripcion == ComunidadEntidad.Descripcion.ToUpper()).FirstOrDefault();
                if (Comunidad1 == null)
                {
                    ConexionBD.sp_CrearComunidad(int.Parse(ComunidadEntidad.IdParroquia), ComunidadEntidad.Descripcion.ToUpper());
                    return "true";
                }
                else
                {
                    return "400";
                }
            }
            catch (Exception)
            {
                return "false";
            }
        }
        public void EliminarComunidad(int IdComunidad)
        {
            ConexionBD.sp_EliminarComunidad(IdComunidad);
        }
        public string ModificarComunidad(ComunidadEntidad ComunidadEntidad)
        {
            Comunidad Comunidad1 = ObtenerListaComunidad().Where(p => Seguridad.DesEncriptar(p.IdComunidad) == ComunidadEntidad.IdComunidad).FirstOrDefault();
            try
            {
                if (Comunidad1 == null)
                {
                    return "false";
                }
                if (Comunidad1.Descripcion.Trim().Contains(ComunidadEntidad.Descripcion.Trim()))
                {
                    ConexionBD.sp_ModificarComunidad(int.Parse(ComunidadEntidad.IdComunidad), ComunidadEntidad.Descripcion.ToUpper(), int.Parse(ComunidadEntidad.IdParroquia));
                    return "true";
                }
                else
                {
                    if (ObtenerListaComunidad().Where(p => p.Descripcion.Trim() == ComunidadEntidad.Descripcion.ToUpper().Trim()).FirstOrDefault() == null)
                    {
                        ConexionBD.sp_ModificarComunidad(int.Parse(ComunidadEntidad.IdComunidad), ComunidadEntidad.Descripcion.ToUpper(), int.Parse(ComunidadEntidad.IdParroquia));
                        return "true";
                    }
                    else
                    {
                        return "400";
                    }
                }
            }
            catch (Exception)
            {
                return "false";
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
    }
}
