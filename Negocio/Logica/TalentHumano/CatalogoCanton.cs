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
                bool estadoCantonEliminacion = false;
                if (item.CantonUtilizado == "0")
                {
                    estadoCantonEliminacion = true;
                }
                else
                {
                    estadoCantonEliminacion = false;
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
        public Canton IngresarCanton(CantonEntidad CantonEntidad)
        {
            Canton Canton_Dato = new Canton();
            foreach (var item in ConexionBD.sp_CrearCanton(CantonEntidad.Descripcion.ToUpper(), int.Parse(CantonEntidad.IdProvincia)))
            {
                Canton_Dato.IdCanton = Seguridad.Encriptar(item.CantonIdCanton.ToString());
                Canton_Dato.Descripcion = item.CantonDescripcion;
                Canton_Dato.FechaCreacion = item.CantonFechaCreacion;
                Canton_Dato.Estado = item.CantonEstado;
                Canton_Dato.PermitirEliminacion = true;
                Canton_Dato.Provincia = new Provincia()
                {
                    IdProvincia = Seguridad.Encriptar(item.ProvinciaIdProvincia.ToString()),
                    Descripcion = item.ProvinciaDescripcion,
                    Estado = item.ProvinciaEstado,
                };
            }
            return Canton_Dato;
        }
        public bool EliminarCanton(int IdCanton)
        {
            try
            {
                ConexionBD.sp_EliminarCanton(IdCanton);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Canton ModificarCanton(CantonEntidad CantonEntidad)
        {
            Canton Canton_Dato = new Canton();
            try
            {
                foreach (var item in ConexionBD.sp_ModificarCanton(int.Parse(CantonEntidad.IdCanton), CantonEntidad.Descripcion.ToUpper(), int.Parse(CantonEntidad.IdProvincia)))
                {
                    bool PermitirEliminar = false;
                    if (item.CantonUtilizado == "0")
                    {
                        PermitirEliminar = true;
                    }
                    else
                    {
                        PermitirEliminar = false;
                    }
                    Canton_Dato.IdCanton = Seguridad.Encriptar(item.CantonIdCanton.ToString());
                    Canton_Dato.Descripcion = item.CantonDescripcion;
                    Canton_Dato.FechaCreacion = item.CantonFechaCreacion;
                    Canton_Dato.Estado = item.CantonEstado;
                    Canton_Dato.PermitirEliminacion = PermitirEliminar;
                    Canton_Dato.Provincia = new Provincia()
                    {
                        IdProvincia = Seguridad.Encriptar(item.ProvinciaIdProvincia.ToString()),
                        Descripcion = item.ProvinciaDescripcion,
                        Estado = item.ProvinciaEstado,
                    };
                }
                return Canton_Dato;
            }
            catch (Exception)
            {
                Canton_Dato.IdCanton = null;
                return Canton_Dato;
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
        public List<Canton> ConsultarCantonPorDescripcion(string Descripcion)
        {
            List<Canton> ListaCanton = new List<Canton>();
            foreach (var item in ConexionBD.sp_ConsultarCantonSiexiste(Descripcion))
            {
                ListaCanton.Add(new Canton()
                {
                    IdCanton = Seguridad.Encriptar(item.IdCanton.ToString()),
                    Descripcion = item.Descripcion,
                });
            }
            return ListaCanton;
        }
        public List<Canton> ConsultarCantonPorId(int IdCanton)
        {
            List<Canton> ListaCanton = new List<Canton>();
            foreach (var item in ConexionBD.sp_ConsultarCantonPorId(IdCanton))
            {
                bool permitirEliminar = false;
                if (item.CantonUtilizado == "0")
                {
                    permitirEliminar = true;
                }
                else
                {
                    permitirEliminar = false;
                }
                ListaCanton.Add(new Canton()
                {
                    IdCanton = Seguridad.Encriptar(item.IdCanton.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    PermitirEliminacion = permitirEliminar
                });
            }
            return ListaCanton;
        }
        public List<Canton> ConsultarCantonesParaSeguimiento(int idProvincia)
        {
            ListaCanton = new List<Canton>();
            foreach (var item in ConexionBD.sp_ConsultarCantonesParaSeguimiento(idProvincia))
            {
                ListaCanton.Add(new Canton()
                {
                    IdCanton = Seguridad.Encriptar(item.IdCanton.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaCanton;
        }
    }
}
