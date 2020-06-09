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
        public PrecioConfigurarProducto InsertarPrecioConfigurarProducto(PrecioConfigurarProducto PrecioConfigurarProducto)
        {
            PrecioConfigurarProducto DatoPrecio = new PrecioConfigurarProducto();
            DatoPrecio = ListarPrecioPorConfigurarProducto(int.Parse(PrecioConfigurarProducto.IdConfigurarProducto)).FirstOrDefault();
            if (DatoPrecio == null)
            {
                
                foreach (var item in ConexionBD.sp_CrearPrecioConfiguracionProducto(int.Parse(PrecioConfigurarProducto.IdConfigurarProducto), PrecioConfigurarProducto.Precio))
                {
                    PrecioConfigurarProducto.IdPrecioConfigurarProducto = Seguridad.Encriptar(item.IdPrecioConfiguracionProducto.ToString());
                    PrecioConfigurarProducto.Precio = item.Precio;
                    PrecioConfigurarProducto.IdConfigurarProducto = Seguridad.Encriptar(PrecioConfigurarProducto.IdConfigurarProducto.ToString());
                    PrecioConfigurarProducto.FechaRegistro = item.FechaRegistro;
                    PrecioConfigurarProducto.Estado = item.Estado.ToString();
                }
            }
            else
            {
                if (DatoPrecio.Precio != PrecioConfigurarProducto.Precio)
                {
                    ConexionBD.sp_EliminarPrecioConfigurarProducto(int.Parse(Seguridad.DesEncriptar(DatoPrecio.IdPrecioConfigurarProducto)));
                    foreach (var item in ConexionBD.sp_CrearPrecioConfiguracionProducto(int.Parse(PrecioConfigurarProducto.IdConfigurarProducto), PrecioConfigurarProducto.Precio))
                    {
                        PrecioConfigurarProducto.IdPrecioConfigurarProducto = Seguridad.Encriptar(item.IdPrecioConfiguracionProducto.ToString());
                        PrecioConfigurarProducto.Precio = item.Precio;
                        PrecioConfigurarProducto.IdConfigurarProducto = Seguridad.Encriptar(PrecioConfigurarProducto.IdConfigurarProducto.ToString());
                        PrecioConfigurarProducto.FechaRegistro = item.FechaRegistro;
                        PrecioConfigurarProducto.Estado = item.Estado.ToString();
                    }
                }
                else
                {
                    PrecioConfigurarProducto = DatoPrecio;
                }
            }
            return PrecioConfigurarProducto;
        }
        public List<PrecioConfigurarProducto> ListarPrecioPorConfigurarProducto(int idConfigurarProducto)
        {
            List<PrecioConfigurarProducto> Precio = new List<PrecioConfigurarProducto>();
            foreach (var item in ConexionBD.sp_ConsultarPrecioProductoPorConfigurarProducto(idConfigurarProducto))
            {
                Precio.Add(new PrecioConfigurarProducto()
                {
                    IdPrecioConfigurarProducto = Seguridad.Encriptar(item.IdPrecioConfiguracionProducto.ToString()),
                    IdConfigurarProducto = Seguridad.Encriptar(item.IdConfigurarProducto.ToString()),
                    FechaRegistro = item.FechaRegistro,
                    Precio = item.Precio,
                    Estado = item.Estado.ToString()
                });
            }
            return Precio;
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
