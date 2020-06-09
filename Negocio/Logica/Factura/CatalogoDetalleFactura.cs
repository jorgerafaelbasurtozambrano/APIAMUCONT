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

        public DetalleFactura InsertarDetalleFactura(DetalleFactura DetalleFactura)
        {
            foreach (var item in ConexionBD.sp_CrearDetalleFactura(int.Parse(DetalleFactura.IdCabeceraFactura), int.Parse(DetalleFactura.IdAsignarProductoLote), DetalleFactura.Cantidad, DetalleFactura.Faltante))
            {
                DetalleFactura.IdDetalleFactura = Seguridad.Encriptar(item.IdDetalleFactura.ToString());
                DetalleFactura.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                DetalleFactura.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                DetalleFactura.Cantidad = item.Cantidad;
            }
            return DetalleFactura;
        }
        public List<AsignarProductoLote> ConsultarAsignarProductoLotePorId(int _IdAsignarProductoLote)
        {
            List<AsignarProductoLote> ListaAPL = new List<AsignarProductoLote>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarProductoLotePorId(_IdAsignarProductoLote))
            {
                ListaAPL.Add(new AsignarProductoLote()
                {
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    IdLote = item.IdLote.ToString(),
                    IdRelacionLogica = item.IdRelacionLogica.ToString(),
                    Utilizado = item.AsignarProductoUtilizado
                });
            }
            return ListaAPL;
        }
        public bool EliminarDetalleFactura(DetalleFacturaVenta _DetalleFacturaVenta)
        {
            try
            {
                ConexionBD.sp_EliminarDetalleFactura(int.Parse(Seguridad.DesEncriptar(_DetalleFacturaVenta.IdDetalleFactura)));
                AsignarProductoLote DatoAPL = new AsignarProductoLote();
                DatoAPL = ConsultarAsignarProductoLotePorId(int.Parse(Seguridad.DesEncriptar(_DetalleFacturaVenta.IdAsignarProductoLote))).FirstOrDefault();
                //if (DatoAPL.Utilizado == "0")
                //{
                //    ConexionBD.sp_EliminarAsignarProductoLote(int.Parse(Seguridad.DesEncriptar(_DetalleFacturaVenta.IdAsignarProductoLote)));
                //}
                if (_DetalleFacturaVenta.AsignarProductoLote.IdLote != "")
                {
                    Lote Lote = new Lote();
                    Lote = ConsultarLotePorId(int.Parse(Seguridad.DesEncriptar(_DetalleFacturaVenta.AsignarProductoLote.IdLote))).FirstOrDefault();
                    if (Lote != null)
                    {
                        if (Lote.LoteUtilizado == "0")
                        {
                            ConexionBD.sp_EliminarLote(int.Parse(Seguridad.DesEncriptar(Lote.IdLote)));
                        }
                        //else
                        //{
                        //    ConexionBD.sp_DisminuirLote(int.Parse(Seguridad.DesEncriptar(Lote.IdLote)), _DetalleFacturaVenta.Cantidad);
                        //}
                    }
                }
                //int Cantidad = ConexionBD.sp_ConsultarCantidadDeDetalleDeUnaFactura(int.Parse(Seguridad.DesEncriptar(_DetalleFacturaVenta.CabeceraFactura.IdCabeceraFactura))).Select(p => p.Value).First();
                //if (Cantidad == 0)
                //{
                  //  ConexionBD.sp_EliminarCabeceraFactura(int.Parse(Seguridad.DesEncriptar(_DetalleFacturaVenta.CabeceraFactura.IdCabeceraFactura)));
                //}
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarCabeceraFactura(int _idCabeceraFactura)
        {
            try
            {
                ConexionBD.sp_EliminarCabeceraFactura(_idCabeceraFactura);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int CantidadDetalleFactura(int _idCabeceraFactura)
        {
            return ConexionBD.sp_ConsultarCantidadDeDetalleDeUnaFactura(_idCabeceraFactura).Select(p => p.Value).First();
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
        public List<DetalleFacturaVenta> ConsultarDetalleFacturaPorId(int idDetalleFactura)
        {
            List<DetalleFacturaVenta> ListaDetalleFactura = new List<DetalleFacturaVenta>();
            foreach (var item in ConexionBD.sp_ConsultarDetalleFacturaPorId(idDetalleFactura))
            {
                string idLote = "";
                if (item.AsignarProductoLoteIdLote!=null)
                {
                    idLote = Seguridad.Encriptar(item.AsignarProductoLoteIdLote.ToString());
                }
                ListaDetalleFactura.Add(new DetalleFacturaVenta()
                {
                    IdDetalleFactura = Seguridad.Encriptar(item.DetalleFacturaIdDetalleFactura.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.DetalleFacturaIdAsignarProductoLote.ToString()),
                    Faltante = item.DetalleFacturaFaltante,
                    Cantidad = item.DetalleFacturaCantidad,
                    AsignarProductoLote = new AsignarProductoLote()
                    {
                        IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                        FechaExpiracion = item.AsignarProductoLoteFechaExpiracion,
                        IdRelacionLogica = item.AsignarProductoLoteIdRelacionLogica.ToString(),
                        PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                        ValorUnitario = item.AsignarProductoLoteValorUnitario,
                        IdLote = idLote
                    },
                    CabeceraFactura = new CabeceraFactura()
                    {
                        IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                        Codigo = item.CabeceraFacturaCodigo,
                        EstadoCabeceraFactura = item.CabeceraFacturaEstado_Cabecera_Factura,
                        FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                        Finalizado = item.CabeceraFacturaFinalizado,
                        IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                        IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    }
                });
            }
            return ListaDetalleFactura;
        }
        public List<Lote> ConsultarLotePorId(int _idlote)
        {
            List<Lote> ListaLote = new List<Lote>();
            foreach (var item in ConexionBD.sp_ConsultarLotePorId(_idlote))
            {
                ListaLote.Add(new Lote()
                {
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    Codigo = item.Codigo,
                    FechaExpiracion = item.FechaExpiracion,
                    Capacidad = item.Capacidad,
                    LoteUtilizado = item.LoteUtilizado
                });
            }
            return ListaLote;
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
        public bool AumentarDetalle(int IdDetalleFactura,int? Cantidad)
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

        public List<DetalleFactura> ConsultarDetalleFacturaCompraPorId(int _idDetalleFactura)
        {
            List<DetalleFactura> ListaDetalleFactura = new List<DetalleFactura>();
            foreach (var item in ConexionBD.sp_ConsultarDetalleFacturaCompraPorId(_idDetalleFactura))
            {
                ListaDetalleFactura.Add(new DetalleFactura()
                {
                    IdDetalleFactura = Seguridad.Encriptar(item.IdDetalleFactura.ToString()),
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    Cantidad = item.Cantidad
                });
            }
            return ListaDetalleFactura;
        }
    }
}
