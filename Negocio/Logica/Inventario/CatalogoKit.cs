using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoKit
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Kit> ListaKit;
        CatalogoAsignarDescuentoKit GestionAsignarDescuentoKit = new CatalogoAsignarDescuentoKit();
        public string InsertarKit(Kit Kit)
        {
            try
            {
                Kit Kit1 = ListarKit().Where(p => p.Codigo == Kit.Codigo).FirstOrDefault();
                if (Kit1 == null)
                {
                    return Seguridad.Encriptar(ConexionBD.sp_CrearKit(Kit.Codigo, Kit.Descripcion.ToUpper()).Select(e => e.Value.ToString()).First());                    
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
        public bool EliminarKit(int IdKit)
        {
            try
            {
                ConexionBD.sp_EliminarKit(IdKit);
                GestionAsignarDescuentoKit.EliminarAsignacionDescuentoKit(IdKit.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string ModificarKit(Kit Kit)
        {
            try
            {
                //Kit Kit1 = ListarKit().Where(p => p.Descripcion == Kit.Descripcion.ToUpper()).FirstOrDefault();
                //if (Kit1 == null)
                //{
                    ConexionBD.sp_ModificarKit(int.Parse(Kit.IdKit), Kit.Codigo, Kit.Descripcion.ToUpper());
                    return "true";
                //}
                //else
                //{
                    //return "400";
                //}
            }
            catch (Exception)
            {
                return "false";
            }
        }
        public void CargarKits()
        {
            ListaKit = new List<Kit>();
            foreach (var item in ConexionBD.sp_ConsultarKit())
            {
                ListaKit.Add(new Kit()
                {
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado,
                    KitUtilizado = item.KitUtilizado,
                    AsignarDescuentoKit = new AsignarDescuentoKit()
                    {
                        IdAsignarDescuentoKit = Seguridad.Encriptar(item.AsignarDescuentoKitIdAsignarDescuentoKit.ToString()),
                        IdDescuento = Seguridad.Encriptar(item.AsignarDescuentoKitIdDescuento.ToString()),
                        IdKit = Seguridad.Encriptar(item.AsignarDescuentoKitIdKit.ToString()),
                        FechaCreacion = item.AsignarDescuentoKitFechaCreacion,
                        FechaActualizacion = item.AsignarDescuentoKitFechaModificacion,
                        Estado = item.AsignarDescuentoKitEstado,
                        Descuento = new Descuento()
                        {
                            IdDescuento = Seguridad.Encriptar(item.AsignarDescuentoKitIdDescuento.ToString()),
                            Porcentaje = item.DescuentoPorcentaje,
                            Estado = item.DescuentoEstado,
                        }
                    },
                });
            }
        }
        public List<Kit> ListarKit()
        {
            CargarKits();
            //return ListaKit.Where(p=>p.Estado !=false).GroupBy(a => a.IdKit).Select(grp => grp.First()).Where(p => p.Estado != false).ToList();
            return ListaKit.Where(p => p.Estado == true && p.AsignarDescuentoKit.Estado == true ).GroupBy(a => a.IdKit).Select(grp => grp.First()).ToList();
        }
    }
}
