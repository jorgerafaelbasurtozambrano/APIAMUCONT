
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoAsignarDescuentoKit
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        List<AsignarDescuentoKit> ListaAsignarDescuentoKit;
        public AsignarDescuentoKit InsertarAsignarDescuentoKit(AsignarDescuentoKit AsignarDescuentoKit)
        {
            try
            {
                AsignarDescuentoKit DatoAsignarDescuentoKit = new AsignarDescuentoKit();
                DatoAsignarDescuentoKit = CargarAsignarDescuentoKitPorIdKit(int.Parse(AsignarDescuentoKit.IdKit)).FirstOrDefault();
                if (DatoAsignarDescuentoKit == null)
                {
                    foreach (var item in ConexionBD.sp_CrearAsignarDescuentoKit(int.Parse(AsignarDescuentoKit.IdDescuento), int.Parse(AsignarDescuentoKit.IdKit)))
                    {
                        AsignarDescuentoKit.IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString());
                        AsignarDescuentoKit.IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString());
                        AsignarDescuentoKit.IdKit = Seguridad.Encriptar(item.IdKit.ToString());
                        AsignarDescuentoKit.FechaCreacion = item.FechaCreacion;
                        AsignarDescuentoKit.FechaActualizacion = item.FechaModificacion;
                        AsignarDescuentoKit.Estado = item.Estado;
                    }
                }
                else
                {
                    EliminarAsignacionDescuentoKit(AsignarDescuentoKit.IdKit);
                    foreach (var item in ConexionBD.sp_CrearAsignarDescuentoKit(int.Parse(AsignarDescuentoKit.IdDescuento), int.Parse(AsignarDescuentoKit.IdKit)))
                    {
                        AsignarDescuentoKit.IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString());
                        AsignarDescuentoKit.IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString());
                        AsignarDescuentoKit.IdKit = Seguridad.Encriptar(item.IdKit.ToString());
                        AsignarDescuentoKit.FechaCreacion = item.FechaCreacion;
                        AsignarDescuentoKit.FechaActualizacion = item.FechaModificacion;
                        AsignarDescuentoKit.Estado = item.Estado;
                    }
                }
                return AsignarDescuentoKit;
            }
            catch (Exception)
            {
                AsignarDescuentoKit.IdAsignarDescuentoKit = null;
                return AsignarDescuentoKit;
            }
        }
        public List<AsignarDescuentoKit> CargarAsignarDescuentoKitPorIdKit(int idKit)
        {
            ListaAsignarDescuentoKit = new List<AsignarDescuentoKit>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarDescuentoKitPorKit(idKit))
            {
                ListaAsignarDescuentoKit.Add(new AsignarDescuentoKit()
                {
                    IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString()),
                    IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString()),
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaModificacion,
                    Estado = item.Estado,
                });
            }
            return ListaAsignarDescuentoKit;
        }
        public void CargarDatos()
        {
            ListaAsignarDescuentoKit = new List<AsignarDescuentoKit>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarDescuentoKit())
            {
                ListaAsignarDescuentoKit.Add(new AsignarDescuentoKit()
                {
                    IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString()),
                    IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString()),
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaModificacion,
                    Estado = item.Estado,
                });
            }
        }
        public List<AsignarDescuentoKit> ListarKit()
        {
            CargarDatos();
            return ListaAsignarDescuentoKit;
        }
        public void EliminarAsignacionDescuentoKit(string IdKit)
        {
            List<AsignarDescuentoKit> MostrarListaAsignarDescuentoKit = new List<AsignarDescuentoKit>();
            MostrarListaAsignarDescuentoKit = ListarKit().Where(p => Seguridad.DesEncriptar(p.IdKit) == IdKit && p.Estado == true).ToList();
            foreach (var item in MostrarListaAsignarDescuentoKit)
            {
                ConexionBD.sp_EliminarAsignarDescuentoKit(int.Parse(Seguridad.DesEncriptar(item.IdAsignarDescuentoKit)));
            }
        }
    }
}
