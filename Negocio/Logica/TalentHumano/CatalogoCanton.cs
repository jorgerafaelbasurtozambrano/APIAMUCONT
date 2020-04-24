using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoCanton
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        static List<Canton> ListaCanton;
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoParroquia GestionParroquia = new CatalogoParroquia();
        public CatalogoCanton()
        {
            ListaCanton = new List<Canton>();
            foreach (var item in ConexionBD.sp_ConsultarCanton().ToList().Where(p=>p.Estado != false && p.ProvinciaEstado != false).ToList())
            {
                bool estadoCantonEliminacion;
                if (GestionParroquia.ListarParroquiaCanton(item.IdCanton).ToList().Count > 0)
                {
                    estadoCantonEliminacion = false;
                }
                else
                {
                    estadoCantonEliminacion = true;
                }
                ListaCanton.Add(new Canton()
                {
                    IdCanton =Seguridad.Encriptar(item.IdCanton.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    PermitirEliminacion = estadoCantonEliminacion,
                    Provincia = new Provincia()
                    {
                        IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString()),
                        Descripcion = item.ProvinciaDescripcion,
                        Estado = item.ProvinciaEstado,
                    },
                });
            }
        }
        public List<Canton> ObtenerListaCanton()
        {
            return ListaCanton.GroupBy(a => a.IdCanton).Select(grp => grp.First()).ToList();
        }
        public string IngresarCanton(CantonEntidad CantonEntidad)
        {
            try
            {
                Canton Canton1 = ObtenerListaCanton().Where(p => p.Descripcion == CantonEntidad.Descripcion.ToUpper()).FirstOrDefault();
                if (Canton1 == null)
                {
                    ConexionBD.sp_CrearCanton(CantonEntidad.Descripcion.ToUpper(), int.Parse(CantonEntidad.IdProvincia));
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
        public bool EliminarCanton(int IdCanton)
        {
            if (GestionParroquia.ListarParroquiaCanton(IdCanton).ToList().Count>0)
            {
                return false;
            }
            else
            {
                ConexionBD.sp_EliminarCanton(IdCanton);
                return true;
            }
        }
        public string ModificarCanton(CantonEntidad CantonEntidad)
        {
            Canton Canton1 = ObtenerListaCanton().Where(p => Seguridad.DesEncriptar(p.IdCanton) == CantonEntidad.IdCanton).FirstOrDefault();           
            try
            {
                if (Canton1 == null)
                {
                    return "false";
                }
                if (Canton1.Descripcion.TrimEnd().TrimStart().Trim().Contains(CantonEntidad.Descripcion.TrimEnd().TrimStart().Trim()))
                {
                    ConexionBD.sp_ModificarCanton(int.Parse(CantonEntidad.IdCanton), CantonEntidad.Descripcion.ToUpper(), int.Parse(CantonEntidad.IdProvincia));
                    return "true";
                }
                else
                {
                    if (ObtenerListaCanton().Where(p => p.Descripcion.TrimEnd().TrimStart().Trim() == CantonEntidad.Descripcion.ToUpper().TrimEnd().TrimStart().Trim()).FirstOrDefault() == null)
                    {
                        ConexionBD.sp_ModificarCanton(int.Parse(CantonEntidad.IdCanton), CantonEntidad.Descripcion.ToUpper(), int.Parse(CantonEntidad.IdProvincia));
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
        public List<Canton> ListaCantonesProvincia(int IdProvincia)
        {
            List<Canton> ListaCantones = new List<Canton>();
            foreach (var item in ConexionBD.ConsultarCantonesDeUnaProvincia(IdProvincia))
            {
                ListaCantones.Add(new Canton()
                {
                    IdCanton = Seguridad.Encriptar(item.IdCanton.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaCantones;


        }
    }
}
