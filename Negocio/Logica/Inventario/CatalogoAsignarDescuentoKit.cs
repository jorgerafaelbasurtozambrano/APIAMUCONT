
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
        public bool InsertarAsignarDescuentoKit(AsignarDescuentoKit AsignarDescuentoKit)
        {
            try
            {
                EliminarAsignacionDescuentoKit(AsignarDescuentoKit.IdKit);
                ConexionBD.sp_CrearAsignarDescuentoKit(int.Parse(AsignarDescuentoKit.IdDescuento), int.Parse(AsignarDescuentoKit.IdKit));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
