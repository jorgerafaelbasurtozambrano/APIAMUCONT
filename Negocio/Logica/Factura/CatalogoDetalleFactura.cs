using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Logica.Inventario;
using Negocio;
using Negocio.Logica.Seguridad;
using Negocio.Entidades;
namespace Negocio.Logica.Factura
{
    public class CatalogoDetalleFactura
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoCabeceraFactura GestionCabeceraFactura = new CatalogoCabeceraFactura();
        CatalogoConfigurarProducto GestionConfigurarProductos = new CatalogoConfigurarProducto();
        CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        CatalogoLote GestionLote = new CatalogoLote();

        public bool InsertarDetalleFactura(DetalleFactura DetalleFactura)
        {
            try
            {
                ConexionBD.sp_CrearDetalleFactura(int.Parse(DetalleFactura.IdCabeceraFactura), int.Parse(DetalleFactura.IdAsignarProductoLote), DetalleFactura.Cantidad, DetalleFactura.Faltante);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string EliminarDetalleFactura(int IdDetalleFactura, string IdCabeceraFactura)
        {
            try
            {
                CabeceraFactura CabeceraFactura = GestionCabeceraFactura.ConsultarFactura(IdCabeceraFactura).FirstOrDefault();
                if (CabeceraFactura != null)
                {
                    string Respuesta = "true";
                    if (CabeceraFactura.Finalizado == false)
                    {
                        DetalleFactura DetalleFactura = ListarDetalleFactura().Where(p => Seguridad.DesEncriptar(p.IdDetalleFactura) == IdDetalleFactura.ToString()).FirstOrDefault();
                        AsignarProductoLote AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(DetalleFactura.IdAsignarProductoLote)).First();
                        ConexionBD.sp_EliminarDetalleFactura(IdDetalleFactura);
                        ConexionBD.sp_EliminarAsignarProductoLote(int.Parse(Seguridad.DesEncriptar(DetalleFactura.IdAsignarProductoLote)));
                        Respuesta = "true";
                        if (AsignarProductoLote.IdLote != "")
                        {
                            Lote Lote = new Lote();
                            List<Lote> ListaDeLotes = new List<Lote>();
                            ListaDeLotes = GestionLote.CargarTodosLosLotes();
                            //Lote = GestionLote.CargarTodosLosLotes().Where(p => Seguridad.DesEncriptar(p.IdLote) == Seguridad.DesEncriptar(AsignarProductoLote.IdLote)).First();
                            Lote = ListaDeLotes.Where(p => Seguridad.DesEncriptar(p.IdLote) == Seguridad.DesEncriptar(AsignarProductoLote.IdLote)).First();
                            if (Lote.LoteUtilizado == "0")
                            {
                                ConexionBD.sp_EliminarLote(int.Parse(Seguridad.DesEncriptar(Lote.IdLote)));
                            }
                            else
                            {
                                ConexionBD.sp_DisminuirLote(int.Parse(Seguridad.DesEncriptar(Lote.IdLote)), DetalleFactura.Cantidad);
                                //ConexionBD.sp_AumentarLote(int.Parse(Seguridad.DesEncriptar(Lote.IdLote)), DetalleFactura.Cantidad);
                            }
                        }
                        int Cantidad = ConexionBD.sp_ConsultarCantidadDeDetalleDeUnaFactura(int.Parse(IdCabeceraFactura)).Select(p => p.Value).First();
                        if (Cantidad == 0)
                        {
                            ConexionBD.sp_EliminarCabeceraFactura(int.Parse(IdCabeceraFactura));
                            Respuesta = "0";
                        }
                        return Respuesta;
                    }
                    else
                    {
                        return "400";
                    }
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
        public List<DetalleFactura> ListarDetalleFactura()
        {
            List<DetalleFactura> ListaDetalle = new List<DetalleFactura>();
            foreach (var item in ConexionBD.sp_ConsultarTodosDetallesFactura())
            {
                ListaDetalle.Add(new DetalleFactura()
                {
                    IdDetalleFactura = Seguridad.Encriptar(item.IdDetalleFactura.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    Faltante = item.Faltante,
                    Cantidad = item.Cantidad,
                });
            }
            return ListaDetalle;
        }
        static List<Stock> ListaStock;
        public void CargarDatos()
        {
            ListaStock = new List<Stock>();
            foreach (var item in ConexionBD.sp_ConsultarStock())
            {
                    ListaStock.Add(new Stock()
                    {

                        IdStock = Seguridad.Encriptar(item.IdStock.ToString()),
                        IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                        Cantidad = item.Cantidad,
                        FechaActualizacion = item.FechaActualizacion,
                    });
            }
        }
        public List<Stock> ListarStock()
        {
            CargarDatos();
            return ListaStock;
        }
        public bool AumentarDetalle(int IdDetalleFactura,int Cantidad)
        {
            try
            {
                ConexionBD.sp_AumentarDetalleFactura(IdDetalleFactura, Cantidad);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
