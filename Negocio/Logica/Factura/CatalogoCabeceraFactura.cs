 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.Credito;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Factura;
namespace Negocio.Logica.Inventario
{
    public class CatalogoCabeceraFactura
    {
        public AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        ConsultarUsuariosYPersonas Busqueda = new ConsultarUsuariosYPersonas();
        CatalogoStock GestionStock = new CatalogoStock();
        CatalogoDetalleVenta GestionDetalleVenta = new CatalogoDetalleVenta();
        CatalogoPrecioConfigurarProducto GestionPrecioConfigurarProducto = new CatalogoPrecioConfigurarProducto();
        static List<Stock> ListaStock;

        static List<CabeceraFactura> ListaCabeceraFactura;
        public object InsertarCabeceraFactura(CabeceraFactura CabeceraFactura)
        {
            try
            {
                CabeceraFactura DatoCabeceraFactura = new CabeceraFactura();
                foreach (var item in ConexionBD.sp_CrearCabeceraFactura(int.Parse(CabeceraFactura.IdAsignacionTU), int.Parse(CabeceraFactura.IdTipoTransaccion)))
                {
                    DatoCabeceraFactura.IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString());
                    DatoCabeceraFactura.Codigo = item.Codigo;
                    DatoCabeceraFactura.IdAsignacionTU = Seguridad.Encriptar(item.IdAsignacionTU.ToString());
                    DatoCabeceraFactura.IdTipoTransaccion = Seguridad.Encriptar(item.IdTipoTransaccion.ToString());
                    DatoCabeceraFactura.FechaGeneracion = item.FechaGeneracion;
                    DatoCabeceraFactura.EstadoCabeceraFactura = item.Estado_Cabecera_Factura;
                    DatoCabeceraFactura.Finalizado = item.Finalizado;
                }
                return DatoCabeceraFactura;
            }
            catch (Exception)
            {
                return "false";
            }
        }
        public bool AnularFactura(CabeceraFactura CabeceraFactura)
        {
            try
            {
                ConexionBD.sp_AnularFactura(int.Parse(CabeceraFactura.IdCabeceraFactura), int.Parse(CabeceraFactura.IdAsignacionTU));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool FinalizarCabeceraFactura(int IdCabeceraFactura)
        {
            try
            {
                foreach (var item in ListarCabeceraFacturaNoFinalizadas().Where(p=>Seguridad.DesEncriptar(p.IdCabeceraFactura)== IdCabeceraFactura.ToString()))
                {
                    foreach (var item1 in item.DetalleFactura)
                    {
                        foreach (var item2 in item1.AsignarProductoLote)
                        {
                            Stock DatoStock = GestionStock.ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item2.IdAsignarProductoLote)).FirstOrDefault();
                            if (Seguridad.DesEncriptar(item.IdTipoTransaccion) == "1")
                            {
                                if (DatoStock == null)
                                {
                                    GestionStock.IngresarStock(new Stock()
                                    {
                                        Cantidad = item1.Cantidad,
                                        IdAsignarProductoLote = Seguridad.DesEncriptar(item2.IdAsignarProductoLote),
                                    });
                                }
                                else
                                {
                                    ConexionBD.sp_AumentarStock(int.Parse(Seguridad.DesEncriptar(DatoStock.IdStock)), item1.Cantidad);
                                }
                            }
                            else if (Seguridad.DesEncriptar(item.IdTipoTransaccion) == "2")
                            {
                                if (item1.Cantidad  <= DatoStock.Cantidad)
                                {
                                    ConexionBD.sp_DisminuirStock(int.Parse(Seguridad.DesEncriptar(DatoStock.IdStock)), item1.Cantidad);
                                }
                            }
                        }
                    }
                }
                ConexionBD.sp_FinalizarCabeceraFactura(IdCabeceraFactura);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CargarDatos()
        {
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura())
            {
                List<DetalleFactura> DetalleFactura = new List<Entidades.DetalleFactura>();
                foreach (var item1 in ConexionBD.sp_ConsultarDetalleFactura(item.CabeceraFacturaIdCabeceraFactura))
                {
                    DetalleFactura.Add(new Entidades.DetalleFactura()
                    {
                        IdDetalleFactura = Seguridad.Encriptar(item1.IdDetalleFactura.ToString()),
                        IdCabeceraFactura = Seguridad.Encriptar(item1.IdCabeceraFactura.ToString()),
                        IdAsignarProductoLote = Seguridad.Encriptar(item1.IdAsignarProductoLote.ToString()),
                        Cantidad = item1.Cantidad,
                        Faltante = item1.Faltante,
                        AsignarProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == item1.IdAsignarProductoLote.ToString()).ToList(),
                    });
                }
                List<PersonaEntidad> Persona = new List<PersonaEntidad>();
                List<TipoUsuario> TipoUsuario = new List<Entidades.TipoUsuario>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    Persona.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                    TipoUsuario.Add(new Entidades.TipoUsuario()
                    {
                        IdTipoUsuario = Seguridad.Encriptar(item2.TipoUsuarioIdTipoUsuario.ToString()),
                        IdAsignacionTu = Seguridad.Encriptar(item2.AsignacionTUIdAsignacionTU.ToString()),
                        Identificacion = item2.TipoUsuarioIdentificacion,
                        Descripcion = item2.TipoUsuarioDescripcion,
                    });
                }
                ListaCabeceraFactura.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    EstadoCabeceraFactura = item.CabeceraFacturaEstadoCabeceraFactura,
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    DetalleFactura = DetalleFactura,
                    PersonaEntidad = Persona.FirstOrDefault(),
                    TipoUsuario = TipoUsuario.FirstOrDefault(),
                    
                });
            }
        }
        public void CargarDatosVenta()
        {
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            var ListadetalleVenta = GestionDetalleVenta.ListarDetalleVenta();
            var ListaPrecio = GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p=> p.Estado == "True").ToList();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p=>p.CabeceraFacturaIdTipoTransaccion == 2).ToList())
            {
                List<DetalleVenta> DetalleVenta = new List<DetalleVenta>();
                foreach (var item1 in ListadetalleVenta.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == item.CabeceraFacturaIdCabeceraFactura.ToString()))
                {
                    var ProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item1.IdAsignarProductoLote.ToString())).FirstOrDefault();
                    decimal ValorUnitario = 0;
                    decimal? Total = 0;
                    decimal? Subtotal = 0;
                    if (ProductoLote.PerteneceKit == "True")
                    {
                        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].IdConfigurarProducto)).FirstOrDefault().Precio;
                        if (item1.AplicaDescuento == "True")
                        {
                            Subtotal = ValorUnitario * item1.Cantidad;
                            //decimal? proce = Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100);
                            Total = Subtotal - (Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].Kit.AsignarDescuentoKit.Descuento.Porcentaje)/100));
                        }
                        else
                        {
                            Total = ValorUnitario * item1.Cantidad;
                            Subtotal = Total;
                        }
                    }
                    else
                    {
                        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.ConfigurarProductos.IdConfigurarProducto)).FirstOrDefault().Precio;
                        Total = ValorUnitario * item1.Cantidad;
                        Subtotal = Total;
                    }

                    DetalleVenta.Add(new Entidades.DetalleVenta()
                    {
                        IdDetalleVenta = item1.IdDetalleVenta,
                        IdCabeceraFactura = item1.IdCabeceraFactura,
                        IdAsignarProductoLote = item1.IdAsignarProductoLote,
                        AplicaDescuento = item1.AplicaDescuento,
                        Faltante = item1.Faltante,
                        Cantidad = item1.Cantidad,
                        AsignarProductoLote = ProductoLote,
                        ValorUnitario = ValorUnitario,
                        Total = Total,
                        Subtotal = Subtotal,
                    });
                }
                DetalleVenta = DetalleVenta.GroupBy(a => a.IdDetalleVenta).Select(grp => grp.First()).ToList();
                List<PersonaEntidad> Persona = new List<PersonaEntidad>();
                List<TipoUsuario> TipoUsuario = new List<Entidades.TipoUsuario>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    Persona.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                    TipoUsuario.Add(new Entidades.TipoUsuario()
                    {
                        IdTipoUsuario = Seguridad.Encriptar(item2.TipoUsuarioIdTipoUsuario.ToString()),
                        IdAsignacionTu = Seguridad.Encriptar(item2.AsignacionTUIdAsignacionTU.ToString()),
                        Identificacion = item2.TipoUsuarioIdentificacion,
                        Descripcion = item2.TipoUsuarioDescripcion,
                    });
                }
                ListaCabeceraFactura.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    EstadoCabeceraFactura = item.CabeceraFacturaEstadoCabeceraFactura,
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    DetalleVenta = DetalleVenta,
                    PersonaEntidad = Persona.FirstOrDefault(),
                    TipoUsuario = TipoUsuario.FirstOrDefault(),

                });
            }
        }
        public List<CabeceraFactura> ListarCabeceraFacturaFinalizadas()
        {
            CargarDatos();
            return ListaCabeceraFactura.Where(p=>p.Finalizado == true && int.Parse(Seguridad.DesEncriptar(p.IdTipoTransaccion)) == 1).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ListarCabeceraFacturaNoFinalizadas()
        {
            CargarDatos();
            return ListaCabeceraFactura.Where(p => p.Finalizado == false && int.Parse(Seguridad.DesEncriptar(p.IdTipoTransaccion)) == 1).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }

        public List<CabeceraFactura> ListarCabeceraFacturaVentaFinalizadas()
        {
            CargarDatosVenta();
            return ListaCabeceraFactura.Where(p => p.Finalizado == true).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ListarCabeceraFacturaVentaNoFinalizadas(int idCabecera)
        {
            //CargarDatosVenta();
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            var ListadetalleVenta = GestionDetalleVenta.ListarDetalleVenta();
            var ListaPrecio = GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => p.Estado == "True").ToList();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p => p.CabeceraFacturaIdTipoTransaccion == 2 && p.CabeceraFacturaIdCabeceraFactura == idCabecera).ToList())
            {
                List<DetalleVenta> DetalleVenta = new List<DetalleVenta>();
                foreach (var item1 in ListadetalleVenta.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == item.CabeceraFacturaIdCabeceraFactura.ToString()))
                {
                    var ProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item1.IdAsignarProductoLote.ToString())).FirstOrDefault();
                    decimal ValorUnitario = 0;
                    decimal? Total = 0;
                    decimal? Subtotal = 0;
                    if (ProductoLote.PerteneceKit == "True")
                    {
                        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].IdConfigurarProducto)).FirstOrDefault().Precio;
                        if (item1.AplicaDescuento == "True")
                        {
                            Subtotal = ValorUnitario * item1.Cantidad;
                            //decimal? proce = Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100);
                            Total = Subtotal - (Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100));
                        }
                        else
                        {
                            Total = ValorUnitario * item1.Cantidad;
                            Subtotal = Total;
                        }
                    }
                    else
                    {
                        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.ConfigurarProductos.IdConfigurarProducto)).FirstOrDefault().Precio;
                        Total = ValorUnitario * item1.Cantidad;
                        Subtotal = Total;
                    }

                    DetalleVenta.Add(new Entidades.DetalleVenta()
                    {
                        IdDetalleVenta = item1.IdDetalleVenta,
                        IdCabeceraFactura = item1.IdCabeceraFactura,
                        IdAsignarProductoLote = item1.IdAsignarProductoLote,
                        AplicaDescuento = item1.AplicaDescuento,
                        Faltante = item1.Faltante,
                        Cantidad = item1.Cantidad,
                        AsignarProductoLote = ProductoLote,
                        ValorUnitario = ValorUnitario,
                        Total = Total,
                        Subtotal = Subtotal,
                    });
                }
                DetalleVenta = DetalleVenta.GroupBy(a => a.IdDetalleVenta).Select(grp => grp.First()).ToList();
                List<PersonaEntidad> Persona = new List<PersonaEntidad>();
                List<TipoUsuario> TipoUsuario = new List<Entidades.TipoUsuario>();
                foreach (var item2 in ConexionBD.sp_ConsultaActividadPersona(item.CabeceraFacturaIdAsignacionTU))
                {
                    Persona.Add(new PersonaEntidad()
                    {
                        IdPersona = Seguridad.Encriptar(item2.PersonadPersona.ToString()),
                        NumeroDocumento = item2.PersonaNumeroDocumento,
                        ApellidoPaterno = item2.PersonaApellidoPaterno,
                        ApellidoMaterno = item2.PersonaApellidoMaterno,
                        PrimerNombre = item2.PersonaPrimerNombre,
                        SegundoNombre = item2.PersonaSegundoNombre,
                        IdTipoDocumento = Seguridad.Encriptar(item2.PersonaIdTipoDocumento.ToString()),
                    });
                    TipoUsuario.Add(new Entidades.TipoUsuario()
                    {
                        IdTipoUsuario = Seguridad.Encriptar(item2.TipoUsuarioIdTipoUsuario.ToString()),
                        IdAsignacionTu = Seguridad.Encriptar(item2.AsignacionTUIdAsignacionTU.ToString()),
                        Identificacion = item2.TipoUsuarioIdentificacion,
                        Descripcion = item2.TipoUsuarioDescripcion,
                    });
                }
                ListaCabeceraFactura.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    EstadoCabeceraFactura = item.CabeceraFacturaEstadoCabeceraFactura,
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    DetalleVenta = DetalleVenta,
                    PersonaEntidad = Persona.FirstOrDefault(),
                    TipoUsuario = TipoUsuario.FirstOrDefault(),

                });
            }

            return ListaCabeceraFactura.Where(p => p.Finalizado == false).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ConsultarFactura(string IdFactura)
        {
            CargarDatos();
            return ListaCabeceraFactura.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == IdFactura).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public bool FinalizarFacturaVenta(int IdCabeceraFactura)
        {
            try
            {
                var Factura = ListarCabeceraFacturaVentaNoFinalizadas(IdCabeceraFactura).FirstOrDefault();
                var ListaStock = GestionStock.ListarStock();
                foreach (var item in Factura.DetalleVenta)
                {
                    ConexionBD.sp_DisminuirStock(int.Parse(Seguridad.DesEncriptar(ListaStock.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == item.IdAsignarProductoLote).FirstOrDefault().IdAsignarProductoLote)), item.Cantidad);
                }
                ConexionBD.sp_FinalizarCabeceraFactura(IdCabeceraFactura);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public PrecioConfigurarProducto BuscarPrecioDeUnProducto(AsignarProductoLote AsignarProductoLote)
        {
            var ListaAsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == AsignarProductoLote.IdAsignarProductoLote).FirstOrDefault();
            if (ListaAsignarProductoLote.PerteneceKit == "True")
            {
                return GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ListaAsignarProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].IdConfigurarProducto) && p.Estado == "True").FirstOrDefault();
            }
            else
            {
                return GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ListaAsignarProductoLote.ConfigurarProductos.IdConfigurarProducto) && p.Estado == "True").FirstOrDefault();
            }
        }


    }
}
