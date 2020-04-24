using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.Factura;
namespace Negocio.Logica.Credito
{
    public class CatalogoDetalleVenta
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoStock GestionStock = new CatalogoStock();
        List<DetalleVenta> ListaDetalleVenta;
        public string InsertarDetalleVenta(DetalleVenta DetalleVenta)
        {
            try
            {
                var DataDetalleVenta = ListarDetalleVenta().Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == DetalleVenta.IdCabeceraFactura && Seguridad.DesEncriptar(p.IdAsignarProductoLote) == DetalleVenta.IdAsignarProductoLote && p.AplicaDescuento == (DetalleVenta.AplicaDescuento == "1" ? "True": "False")).FirstOrDefault();
                var Stock = GestionStock.ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == DetalleVenta.IdAsignarProductoLote).FirstOrDefault();
                if (Stock == null)
                {
                    return "No hay stock de este producto";
                }
                else
                {
                    if (DetalleVenta.Cantidad <= Stock.Cantidad)
                    {
                        if (DataDetalleVenta == null)
                        {
                            ConexionBD.sp_CrearDetalleVenta(int.Parse(DetalleVenta.IdCabeceraFactura), int.Parse(DetalleVenta.IdAsignarProductoLote), DetalleVenta.AplicaDescuento, DetalleVenta.Faltante, DetalleVenta.Cantidad);
                        }
                        else
                        {
                            ConexionBD.sp_AumentarDetalleVenta(int.Parse(Seguridad.DesEncriptar(DataDetalleVenta.IdDetalleVenta)), DetalleVenta.Cantidad);
                        }
                        return "true";
                    }
                    else
                    {
                        return "No Hay esta cantidad disponible, existe en stock solo " + Stock.Cantidad.ToString() + " Unidades";
                    }
                }
            }
            catch (Exception)
            {
                return "false";
            }
        }
        public void CargarDatos()
        {
            ListaDetalleVenta = new List<DetalleVenta>();
            foreach (var item in ConexionBD.sp_ConsultarDetalleVenta())
            {
                ListaDetalleVenta.Add(new DetalleVenta()
                {
                    IdDetalleVenta = Seguridad.Encriptar(item.IdDetalleVenta.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    Cantidad = item.Cantidad,
                    AplicaDescuento = item.AplicaDescuento.ToString(),
                    Faltante= item.Faltante.ToString(),
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                });
            }
        }
        public List<DetalleVenta> ListarDetalleVenta()
        {
            CargarDatos();
            return ListaDetalleVenta;
        }

        public object AumentarDetalleVenta(DetalleVenta DetalleVenta)
        {
            try
            {
                var DataDetalleVenta = ListarDetalleVenta().Where(p => Seguridad.DesEncriptar(p.IdDetalleVenta) == DetalleVenta.IdDetalleVenta).FirstOrDefault();
                var Stock = GestionStock.ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(DataDetalleVenta.IdAsignarProductoLote)).FirstOrDefault();
                if (Stock == null)
                {
                    return "No hay stock de este producto";
                }
                else
                {
                    if (DetalleVenta.Cantidad <= Stock.Cantidad)
                    {
                        ConexionBD.sp_SetCantidadDetalleVenta(int.Parse(DetalleVenta.IdDetalleVenta), DetalleVenta.Cantidad);
                        return "true";
                    }
                    else
                    {
                        return "No Hay esta cantidad disponible, existe en stock solo " + Stock.Cantidad.ToString() + " Unidades";
                    }
                }
            }
            catch (Exception)
            {
                return "false";
            }
            
        }

        public bool EliminarDetalleVenta(int IdDetalleVenta)
        {
            try
            {
                ConexionBD.sp_EliminarDetalleVenta(IdDetalleVenta);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
