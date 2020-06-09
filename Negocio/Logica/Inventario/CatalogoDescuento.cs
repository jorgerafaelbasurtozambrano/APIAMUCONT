using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoDescuento
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Descuento> ListaDescuento;
        public Descuento InsertarDescuento(Descuento Descuento)
        {
            foreach (var item in ConexionBD.sp_CrearDescuento(Descuento.Porcentaje))
            {
                Descuento.IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString());
                Descuento.Porcentaje = item.Porcentaje;
            }
            return Descuento;
        }
        public void CargarDescuentos()
        {
            ListaDescuento = new List<Descuento>();
            foreach (var item in ConexionBD.sp_ConsultarDescuento())
            {
                ListaDescuento.Add(new Descuento()
                {
                    IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString()),
                    Porcentaje = item.Porcentaje,
                    Estado = item.Estado,
                    DescuentoUtilizado = item.DescuentoUtilizado
                });
            }
        }
        public List<Descuento> ListarDescuento()
        {
            CargarDescuentos();
            return ListaDescuento.GroupBy(a => a.IdDescuento).Select(grp => grp.First()).ToList();
        }
    }
}
