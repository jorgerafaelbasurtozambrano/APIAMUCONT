using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoProvincia
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        static List<Provincia> ListaProvincia;
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoCanton GestionCanton = new CatalogoCanton();
        public Provincia IngresoProvincia(Provincia Provincia)
        {
            foreach (var item in ConexionBD.sp_CrearProvincia(Provincia.Descripcion.ToUpper()))
            {
                Provincia.IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString());
                Provincia.Descripcion = item.Descripcion;
                Provincia.Estado = item.Estado;
                Provincia.FechaCreacion = item.FechaCreacion;
                Provincia.PermitirEliminacion = true;
            }
            return Provincia;
        }
        public bool EliminarProvincia(int IdProvincia)
        {
            try
            {
                ConexionBD.sp_EliminarProvincia(IdProvincia);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Provincia ModificarProvincia(Provincia Provincia)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarProvincia(int.Parse(Provincia.IdProvincia), Provincia.Descripcion.ToUpper()))
                {
                    bool PermitirEliminar = false;
                    if (item.ProvinciaUtilizado == "0")
                    {
                        PermitirEliminar = true;
                    }
                    else
                    {
                        PermitirEliminar = false;
                    }
                    Provincia.IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString());
                    Provincia.Descripcion = item.Descripcion;
                    Provincia.Estado = item.Estado;
                    Provincia.FechaCreacion = item.FechaCreacion;
                    Provincia.PermitirEliminacion = PermitirEliminar;
                }
                return Provincia;
            }
            catch (Exception)
            {
                Provincia.IdProvincia = null;
                return Provincia;
            }
        }
        public CatalogoProvincia()
        {
            ListaProvincia = new List<Provincia>();
            foreach (var item in ConexionBD.sp_ConsultarProvincia())
            {
                bool estadoProvinciaEliminacion = false;
                if (item.ProvinciaUtilizado == "0")
                {
                    estadoProvinciaEliminacion = true;
                }
                else
                {
                    estadoProvinciaEliminacion = false;
                }
                ListaProvincia.Add(new Provincia()
                {
                    IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString()),
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    PermitirEliminacion = estadoProvinciaEliminacion,
                });
            }
        }
        public List<Provincia> ConsultarProvincias()
        {
            return ListaProvincia.Where(p=>p.Estado != false).GroupBy(a => a.IdProvincia).Select(grp => grp.First()).ToList();
        }
        public List<Provincia> ConsultarProvinciaPorDescripcion(string Descripcion)
        {
            List<Provincia> ListaProvincia = new List<Provincia>();
            foreach (var item in ConexionBD.sp_ConsultarProvinciaSiexiste(Descripcion))
            {
                ListaProvincia.Add(new Provincia()
                {
                    IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString()),
                    Descripcion = item.Descripcion,
                });
            }
            return ListaProvincia;
        }
        public List<Provincia> ConsultarProvinciaPorId(int IdProvincia)
        {
            List<Provincia> ListaProvincia = new List<Provincia>();
            foreach (var item in ConexionBD.sp_ConsultarProvinciaPorId(IdProvincia))
            {
                bool permitirEliminar = false;
                if (item.ProvinciaUtilizado=="0")
                {
                    permitirEliminar = true;
                }
                else
                {
                    permitirEliminar = false;
                }
                ListaProvincia.Add(new Provincia()
                {
                    IdProvincia = Seguridad.Encriptar(item.IdProvincia.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    PermitirEliminacion = permitirEliminar
                });
            }
            return ListaProvincia;
        }
    }
}
