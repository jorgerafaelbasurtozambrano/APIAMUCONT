using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.Factura;

namespace Negocio.Logica.Inventario
{
    public class CatalogoPrecioConfigurarProducto
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        //CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        List<PrecioConfigurarProducto> ListaPrecioConfigurarProducto;
        public bool InsertarPrecioConfigurarProducto(PrecioConfigurarProducto PrecioConfigurarProducto)
        {
            try
            {
                //var id = Seguridad.DesEncriptar(ListarPrecioConfigurarProducto().FirstOrDefault().IdConfigurarProducto);
                var ListaPrecios = ListarPrecioConfigurarProducto().Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == PrecioConfigurarProducto.IdConfigurarProducto && p.Estado == "True").FirstOrDefault();
                if (ListaPrecios == null)
                {
                    ConexionBD.sp_CrearPrecioConfiguracionProducto(int.Parse(PrecioConfigurarProducto.IdConfigurarProducto), PrecioConfigurarProducto.Precio);
                }
                else
                {
                    if (ListaPrecios.Precio != PrecioConfigurarProducto.Precio)
                    {
                        ConexionBD.sp_EliminarPrecioConfigurarProducto(int.Parse(Seguridad.DesEncriptar(ListaPrecios.IdPrecioConfigurarProducto)));
                        ConexionBD.sp_CrearPrecioConfiguracionProducto(int.Parse(PrecioConfigurarProducto.IdConfigurarProducto), PrecioConfigurarProducto.Precio);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CargarDatos()
        {
            ListaPrecioConfigurarProducto = new List<PrecioConfigurarProducto>();
            foreach (var item in ConexionBD.sp_ConsultarPrecioConfiguracionProducto())
            {
                ListaPrecioConfigurarProducto.Add(new PrecioConfigurarProducto()
                {
                    IdPrecioConfigurarProducto = Seguridad.Encriptar(item.IdPrecioConfiguracionProducto.ToString()),
                    IdConfigurarProducto = Seguridad.Encriptar(item.IdConfigurarProducto.ToString()),
                    FechaRegistro = item.FechaRegistro,
                    Precio = item.Precio,
                    Estado = item.Estado.ToString()
                });
            }
        }
        public List<PrecioConfigurarProducto> ListarPrecioConfigurarProducto()
        {
            CargarDatos();
            return ListaPrecioConfigurarProducto;
        }

        
    }
}
