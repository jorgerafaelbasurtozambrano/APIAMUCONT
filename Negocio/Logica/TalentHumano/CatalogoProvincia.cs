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
        public string IngresoProvincia(Provincia Provincia)
        {
            Provincia Provincia1 = ConsultarProvincias().Where(p => p.Descripcion == Provincia.Descripcion.ToUpper()).FirstOrDefault();
            try
            {
                if (Provincia1 == null)
                {
                    ConexionBD.sp_CrearProvincia(Provincia.Descripcion.ToUpper());
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
        public bool EliminarProvincia(int IdProvincia)
        {
            if (GestionCanton.ListaCantonesProvincia(IdProvincia).ToList().Count >0 )
            {
                return false;
            }
            else
            {
                ConexionBD.sp_EliminarProvincia(IdProvincia);
                return true;
            }
        }
        public string ModificarProvincia(Provincia Provincia)
        {
            Provincia Provincia1 = ConsultarProvincias().Where(p => Seguridad.DesEncriptar(p.IdProvincia) == Provincia.IdProvincia).FirstOrDefault();
            try
            {
                if (Provincia1 == null)
                {
                    return "false";
                }
                if (Provincia1.Descripcion.Trim().Contains(Provincia.Descripcion.Trim()))
                {
                    ConexionBD.sp_ModificarProvincia(int.Parse(Provincia.IdProvincia), Provincia.Descripcion.ToUpper());
                    return "true";
                }
                else
                {
                    if (ConsultarProvincias().Where(p => p.Descripcion.Trim() == Provincia.Descripcion.ToUpper().Trim()).FirstOrDefault() == null)
                    {
                        ConexionBD.sp_ModificarProvincia(int.Parse(Provincia.IdProvincia), Provincia.Descripcion.ToUpper());
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
        public CatalogoProvincia()
        {
            ListaProvincia = new List<Provincia>();
            foreach (var item in ConexionBD.sp_ConsultarProvincia())
            {
                bool estadoProvinciaEliminacion;
                if (GestionCanton.ListaCantonesProvincia(item.IdProvincia).ToList().Count > 0)
                {
                    estadoProvinciaEliminacion = false;
                }
                else
                {
                    estadoProvinciaEliminacion = true;
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

    }
}
