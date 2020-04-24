using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoParroquia
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        static List<Parroquia> ListaParroquia;
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoComunidad GestionComunidad = new CatalogoComunidad();
        public void CatalogoParroquiaCarga()
        {
            ListaParroquia = new List<Parroquia>();
            foreach (var item in ConexionBD.sp_ConsultarParroquia().ToList().Where(p=>p.Estado !=false && p.CantonEstado != false).ToList())
            {
                bool estadoParroquiaEliminacion;

                if (GestionComunidad.ListarComunidadParroquia(item.IdParroquia).ToList().Count > 0)
                {
                    estadoParroquiaEliminacion =  false;
                }
                else
                {
                    if (ConexionBD.sp_ConsultarAsignacionPPersonas(item.IdParroquia).ToList().Count > 0)
                    {
                        estadoParroquiaEliminacion = false;
                    }
                    else
                    {
                        estadoParroquiaEliminacion = true;
                    }
                }
                ListaParroquia.Add(new Parroquia()
                {
                    IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                    Descripcion = item.Descripcion,
                    Canton = new Canton()
                    { 
                        IdCanton = Seguridad.Encriptar(item.IdCanton.ToString()),
                        Descripcion = item.CantonDescripcion,
                        Estado = item.CantonEstado,
                    },
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    PermitirEliminacion = estadoParroquiaEliminacion,
                });
            }
        }
        public string IngresarParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            try
            {
                Parroquia Parroquia1 = ObtenerListaParroquia().Where(p => p.Descripcion == ParroquiaEntidad.Descripcion.ToUpper()).FirstOrDefault();
                if (Parroquia1 == null)
                {
                    ConexionBD.sp_CrearParroquia(ParroquiaEntidad.Descripcion.ToUpper(), int.Parse(ParroquiaEntidad.IdCanton));
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
        public bool EliminarParroquia(int IdParroquia)
        {
            if (GestionComunidad.ListarComunidadParroquia(IdParroquia).ToList().Count>0)
            {
                return false;
            }
            else
            {
                if (ConexionBD.sp_ConsultarAsignacionPPersonas(IdParroquia).ToList().Count>0)
                {
                    return false;
                }
                else
                {
                    ConexionBD.sp_EliminarParroquia(IdParroquia);
                    return true;
                }
            }
            
        }
        public string ModificarParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            Parroquia Parroquia1 = ObtenerListaParroquia().Where(p => Seguridad.DesEncriptar(p.IdParroquia) == ParroquiaEntidad.IdParroquia).FirstOrDefault();
            try
            {
                if (Parroquia1 == null)
                {
                    return "false";
                }
                if (Parroquia1.Descripcion.TrimEnd().TrimStart().Trim().Contains(ParroquiaEntidad.Descripcion.TrimEnd().TrimStart().Trim()))
                {
                    ConexionBD.sp_ModificarParroquia(int.Parse(ParroquiaEntidad.IdParroquia), ParroquiaEntidad.Descripcion.ToUpper(), int.Parse(ParroquiaEntidad.IdCanton));
                    return "true";
                }
                else
                {
                    if (ObtenerListaParroquia().Where(p => p.Descripcion.TrimEnd().TrimStart().Trim() == ParroquiaEntidad.Descripcion.ToUpper().TrimEnd().TrimStart().Trim()).FirstOrDefault() == null)
                    {
                        ConexionBD.sp_ModificarParroquia(int.Parse(ParroquiaEntidad.IdParroquia), ParroquiaEntidad.Descripcion.ToUpper(), int.Parse(ParroquiaEntidad.IdCanton));
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
        public List<Parroquia> ObtenerListaParroquia()
        {
            CatalogoParroquiaCarga();
            return ListaParroquia.GroupBy(a => a.IdParroquia).Select(grp => grp.First()).ToList();
        }
        public List<Parroquia> ListarParroquiaCanton(int IdCanton)
        {
            List<Parroquia> ListaParroquias = new List<Parroquia>();
            foreach (var item in ConexionBD.ConsultarParroquiasDeUnCanton(IdCanton))
            {
                ListaParroquias.Add(new Parroquia()
                {
                    IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                    Descripcion = item.Descripcion,
                    Canton = null,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaParroquias;
        }

    }
}
