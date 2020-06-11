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
                bool estadoParroquiaEliminacion = false;
                if (item.ParroquiaUtilizado == "0")
                {
                    estadoParroquiaEliminacion = true;
                }
                else
                {
                    estadoParroquiaEliminacion = false;
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
        public Parroquia IngresarParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            Parroquia _DatoParroquia = new Parroquia();
            foreach (var item in ConexionBD.sp_CrearParroquia(ParroquiaEntidad.Descripcion.ToUpper(), int.Parse(ParroquiaEntidad.IdCanton)))
            {
                _DatoParroquia.IdParroquia = Seguridad.Encriptar(item.ParroquiaIdParroquia.ToString());
                _DatoParroquia.Descripcion = item.ParroquiaDescripcion;
                _DatoParroquia.FechaCreacion = item.ParroquiaFechaCreacion;
                _DatoParroquia.Estado = item.ParroquiaEstado;
                _DatoParroquia.PermitirEliminacion = true;
                _DatoParroquia.Canton = new Canton()
                {
                    IdCanton = Seguridad.Encriptar(item.CantonIdCanton.ToString()),
                    Descripcion = item.CantonDescripcion,
                    Estado = item.CantonEstado,
                };
            }
            return _DatoParroquia;
        }
        public bool EliminarParroquia(int IdParroquia)
        {
            try
            {
                ConexionBD.sp_EliminarParroquia(IdParroquia);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Parroquia ModificarParroquia(ParroquiaEntidad ParroquiaEntidad)
        {
            Parroquia _DatoParroquia = new Parroquia();
            try
            {                
                foreach (var item in ConexionBD.sp_ModificarParroquia(int.Parse(ParroquiaEntidad.IdParroquia), ParroquiaEntidad.Descripcion.ToUpper(), int.Parse(ParroquiaEntidad.IdCanton)))
                {
                    bool PermitirEliminar = false;
                    if (item.ParroquiaUtilizado == "0")
                    {
                        PermitirEliminar = true;
                    }
                    else
                    {
                        PermitirEliminar = false;
                    }
                    _DatoParroquia.IdParroquia = Seguridad.Encriptar(item.ParroquiaIdParroquia.ToString());
                    _DatoParroquia.Descripcion = item.ParroquiaDescripcion;
                    _DatoParroquia.FechaCreacion = item.ParroquiaFechaCreacion;
                    _DatoParroquia.Estado = item.ParroquiaEstado;
                    _DatoParroquia.PermitirEliminacion = PermitirEliminar;
                    _DatoParroquia.Canton = new Canton()
                    {
                        IdCanton = Seguridad.Encriptar(item.CantonIdCanton.ToString()),
                        Descripcion = item.CantonDescripcion,
                        Estado = item.CantonEstado,
                    };
                }
                return _DatoParroquia;
            }
            catch (Exception)
            {
                _DatoParroquia.IdParroquia = null;
                return _DatoParroquia;
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
        public List<Parroquia> ConsultarParroquiaPorDescripcion(string Descripcion)
        {
            List<Parroquia> ListaParroquia = new List<Parroquia>();
            foreach (var item in ConexionBD.sp_ConsultarParroquiaSiexiste(Descripcion))
            {
                ListaParroquia.Add(new Parroquia()
                {
                    IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                    Descripcion = item.Descripcion,
                });
            }
            return ListaParroquia;
        }
        public List<Parroquia> ConsultarParroquiaPorId(int IdParroquia)
        {
            List<Parroquia> ListaParroquia = new List<Parroquia>();
            foreach (var item in ConexionBD.sp_ConsultarParroquiaPorId(IdParroquia))
            {
                bool permitirEliminar = false;
                if (item.ParroquiaUtilizado == "0")
                {
                    permitirEliminar = true;
                }
                else
                {
                    permitirEliminar = false;
                }
                ListaParroquia.Add(new Parroquia()
                {
                    IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    PermitirEliminacion = permitirEliminar
                });
            }
            return ListaParroquia;
        }

        public List<Parroquia> ConsultarParroquiaParaSeguimiento(int idCanton)
        {
            ListaParroquia = new List<Parroquia>();
            foreach (var item in ConexionBD.sp_ConsultarParroquiaParaSeguimiento(idCanton))
            {
                ListaParroquia.Add(new Parroquia()
                {
                    IdParroquia = Seguridad.Encriptar(item.IdParroquia.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaParroquia;
        }
    }
}
