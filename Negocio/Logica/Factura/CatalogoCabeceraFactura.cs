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
        CatalogoConfigurarVenta GestionConfigurarVenta = new CatalogoConfigurarVenta();
        CatalogoAsignarTecnicoPersonaComunidad _TecnicoPersonaComunidad = new CatalogoAsignarTecnicoPersonaComunidad();
        static List<Stock> ListaStock;

        static List<CabeceraFactura> ListaCabeceraFactura;
        public CabeceraFactura InsertarCabeceraFactura(CabeceraFactura CabeceraFactura)
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
                //foreach (var item in ListarCabeceraFacturaNoFinalizadas().Where(p=>Seguridad.DesEncriptar(p.IdCabeceraFactura)== IdCabeceraFactura.ToString()))
                foreach (var item in ConsultarFactura(IdCabeceraFactura.ToString()))
                {
                    foreach (var item1 in item.DetalleFactura)
                    {
                        foreach (var item2 in item1.AsignarProductoLote)
                        {
                            if (Seguridad.DesEncriptar(item.IdTipoTransaccion) == "1")
                            {
                                Stock DatoStock = GestionStock.ListarStock().Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item2.IdAsignarProductoLote)).FirstOrDefault();
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
                                if (item2.IdLote !="")
                                {
                                    ConexionBD.sp_AumentarLote(int.Parse(Seguridad.DesEncriptar(item2.IdLote)), item1.Cantidad);
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
                        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.AsignarProductoKit.IdConfigurarProducto)).FirstOrDefault().Precio;
                        if (item1.AplicaDescuento == "True")
                        {
                            Subtotal = ValorUnitario * item1.Cantidad;
                            //decimal? proce = Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100);
                            Total = Subtotal - (Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKit.Kit.AsignarDescuentoKit.Descuento.Porcentaje)/100));
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
            //CargarDatos();
            decimal? TotalFactura = 0;
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p=>p.CabeceraFacturaFinalizado == true && p.CabeceraFacturaIdTipoTransaccion == 1).ToList())
            {
                TotalFactura = 0;
                List<DetalleFactura> DetalleFactura = new List<Entidades.DetalleFactura>();
                foreach (var item1 in ConexionBD.sp_ConsultarDetalleFactura(item.CabeceraFacturaIdCabeceraFactura))
                {
                    var _AsignarProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == item1.IdAsignarProductoLote.ToString()).ToList();
                    TotalFactura = TotalFactura + (item1.Cantidad * _AsignarProductoLote[0].ValorUnitario);
                    DetalleFactura.Add(new Entidades.DetalleFactura()
                    {
                        IdDetalleFactura = Seguridad.Encriptar(item1.IdDetalleFactura.ToString()),
                        IdCabeceraFactura = Seguridad.Encriptar(item1.IdCabeceraFactura.ToString()),
                        IdAsignarProductoLote = Seguridad.Encriptar(item1.IdAsignarProductoLote.ToString()),
                        Cantidad = item1.Cantidad,
                        Faltante = item1.Faltante,
                        AsignarProductoLote = _AsignarProductoLote,
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
                    Total = TotalFactura,
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
            //return ListaCabeceraFactura.Where(p=>p.Finalizado == true && int.Parse(Seguridad.DesEncriptar(p.IdTipoTransaccion)) == 1).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
            return ListaCabeceraFactura.GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ListarCabeceraFacturaNoFinalizadas()
        {
            ListaCabeceraFactura = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p => p.CabeceraFacturaFinalizado == false && p.CabeceraFacturaIdTipoTransaccion == 1).ToList())
            {
                List<DetalleFactura> DetalleFactura = new List<Entidades.DetalleFactura>();
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
            return ListaCabeceraFactura.GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ListarCabeceraFacturaVentaFinalizadas()
        {
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            var ListadetalleVenta = GestionDetalleVenta.ListarDetalleVenta();
            var ListaPrecio = GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => p.Estado == "True").ToList();
            decimal? TotalFactura = 0;
            decimal? TotalDescuento = 0;
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p => p.CabeceraFacturaFinalizado == true && p.CabeceraFacturaIdTipoTransaccion == 2).ToList())
            {
                List<DetalleVenta> DetalleVenta = new List<DetalleVenta>();
                TotalFactura = 0;
                TotalDescuento = 0;
                foreach (var item1 in ListadetalleVenta.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == item.CabeceraFacturaIdCabeceraFactura.ToString()))
                {
                    var ProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item1.IdAsignarProductoLote.ToString())).FirstOrDefault();
                    decimal ValorUnitario = 0;
                    decimal? Total = 0;
                    decimal? Subtotal = 0;
                    if (ProductoLote.PerteneceKit == "True")
                    {
                        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.AsignarProductoKit.IdConfigurarProducto)).FirstOrDefault().Precio;
                        if (item1.AplicaDescuento == "True")
                        {
                            Subtotal = ValorUnitario * item1.Cantidad;
                            Total = Subtotal - (Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKit.Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100));
                            TotalDescuento = TotalDescuento+(Subtotal - Total);
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
                    TotalFactura = TotalFactura + Total;
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
                ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
                _ConfigurarVenta = GestionConfigurarVenta.ConsultarConfigurarVentaPorFactura(item.CabeceraFacturaIdCabeceraFactura);
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
                    ConfigurarVenta = _ConfigurarVenta,
                    Total = TotalFactura,
                    TotalDescuento = TotalDescuento,
                });
            }
            return ListaCabeceraFactura.GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> FacturaVentaNoFinalizadas()
        {
            //List<Stock> ListaStock = new List<Stock>();
            //ListaStock = GestionStock.ListarStock();
            ListaCabeceraFactura = new List<CabeceraFactura>();
            //var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            //var ListadetalleVenta = GestionDetalleVenta.ListarDetalleVenta();
            //var ListaPrecio = GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => p.Estado == "True").ToList();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p => p.CabeceraFacturaIdTipoTransaccion == 2 && p.CabeceraFacturaFinalizado == false).ToList())
            {
                //List<DetalleVenta> DetalleVenta = new List<DetalleVenta>();
                //foreach (var item1 in ListadetalleVenta.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == item.CabeceraFacturaIdCabeceraFactura.ToString()))
                //{
                //    var ProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item1.IdAsignarProductoLote.ToString())).FirstOrDefault();
                //    decimal ValorUnitario = 0;
                //    decimal? Total = 0;
                //    decimal? Subtotal = 0;
                //    if (ProductoLote.PerteneceKit == "True")
                //    {
                //        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.AsignarProductoKit.IdConfigurarProducto)).FirstOrDefault().Precio;
                //        if (item1.AplicaDescuento == "True")
                //        {
                //            Subtotal = ValorUnitario * item1.Cantidad;
                //            //decimal? proce = Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKits.ListaAsignarProductoKit[0].Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100);
                //            Total = Subtotal - (Subtotal * (Convert.ToDecimal(ProductoLote.AsignarProductoKit.Kit.AsignarDescuentoKit.Descuento.Porcentaje) / 100));
                //        }
                //        else
                //        {
                //            Total = ValorUnitario * item1.Cantidad;
                //            Subtotal = Total;
                //        }
                //    }
                //    else
                //    {
                //        ValorUnitario = ListaPrecio.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ProductoLote.ConfigurarProductos.IdConfigurarProducto)).FirstOrDefault().Precio;
                //        Total = ValorUnitario * item1.Cantidad;
                //        Subtotal = Total;
                //    }

                //    DetalleVenta.Add(new Entidades.DetalleVenta()
                //    {
                //        IdDetalleVenta = item1.IdDetalleVenta,
                //        IdCabeceraFactura = item1.IdCabeceraFactura,
                //        IdAsignarProductoLote = item1.IdAsignarProductoLote,
                //        AplicaDescuento = item1.AplicaDescuento,
                //        Faltante = item1.Faltante,
                //        Cantidad = item1.Cantidad,
                //        AsignarProductoLote = ProductoLote,
                //        ValorUnitario = ValorUnitario,
                //        Total = Total,
                //        Subtotal = Subtotal,
                //        //CantidadDisponible = cantidadDisponible,
                //        //PermitirVender = PermitirVender
                //    });
                //}
                //DetalleVenta = DetalleVenta.GroupBy(a => a.IdDetalleVenta).Select(grp => grp.First()).ToList();
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
                ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
                foreach (var item3 in ConexionBD.sp_ConsultarConfigurarVentaPorIdCabeceraFactura(item.CabeceraFacturaIdCabeceraFactura))
                {
                    _ConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item3.IdConfigurarVenta.ToString());
                    _ConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item3.IdCabeceraFactura.ToString());
                    _ConfigurarVenta.IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString());
                    _ConfigurarVenta.EstadoConfVenta = item3.IdConfigurarVenta.ToString();
                    _ConfigurarVenta.Efectivo = item3.Efectivo.ToString();
                    if (item3.IdConfiguracionInteres != null)
                    {
                        _ConfigurarVenta.IdConfiguracionInteres = Seguridad.Encriptar(item3.IdConfiguracionInteres.ToString());
                    }
                    _ConfigurarVenta.Descuento = item3.Descuento;
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
                    ConfigurarVenta = _ConfigurarVenta,
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion,
                    },
                    //DetalleVenta = DetalleVenta,
                    PersonaEntidad = Persona.FirstOrDefault(),
                    TipoUsuario = TipoUsuario.FirstOrDefault(),

                });
            }
            return ListaCabeceraFactura.GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ListarCabeceraFacturaVentaNoFinalizadas(int idCabecera)
        {
            var ListaStock = GestionStock.ListarStock();
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            decimal? TotalDescuento = 0;
            decimal? TotalIva = 0;
            decimal? TotalSubtotal = 0;
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p => p.CabeceraFacturaIdTipoTransaccion == 2 && p.CabeceraFacturaIdCabeceraFactura == idCabecera).ToList())
            {
                List<DetalleVenta> DetalleVenta = new List<DetalleVenta>();
                TotalDescuento = 0;
                TotalIva = 0;
                TotalSubtotal = 0;
                foreach (var item1 in GestionDetalleVenta.ListarDetalleVentaPorFactura(item.CabeceraFacturaIdCabeceraFactura))
                {
                    var ProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item1.IdAsignarProductoLote.ToString())).FirstOrDefault();
                    var ProductoEnStock = ListaStock.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(ProductoLote.IdAsignarProductoLote)).FirstOrDefault();
                    int? cantidadDisponible=0;
                    bool PermitirVender = false;
                    if (item1.Cantidad <= ProductoEnStock.Cantidad)
                    {
                        PermitirVender = true;
                        cantidadDisponible = ProductoEnStock.Cantidad;
                    }
                    else
                    {
                        PermitirVender = false;
                        cantidadDisponible = ProductoEnStock.Cantidad;
                    }
                    decimal ValorUnitario = 0;
                    decimal? Total = 0;
                    decimal? Subtotal = 0;
                    decimal? IvaAnadido = 0;
                    decimal? Descuento = 0;
                    ValorUnitario = item1.ValorUnitario;
                    Subtotal = ValorUnitario * item1.Cantidad;
                    if (item1.AplicaDescuento == "True")
                    {
                        if (item1.Iva>0)
                        {
                            IvaAnadido = (Subtotal * (Convert.ToDecimal(item1.Iva) / 100));
                        }
                        Descuento = (Subtotal + IvaAnadido) * (Convert.ToDecimal(item1.PorcentajeDescuento) / 100);
                        Total = (Subtotal + IvaAnadido) - Descuento;
                        //CantidadDescontada = Subtotal - Total;
                    }
                    else
                    {
                        if (item1.Iva > 0)
                        {
                            IvaAnadido = Subtotal * (Convert.ToDecimal(item1.Iva) / 100);
                        }
                        Total = (Subtotal + IvaAnadido) - Descuento;
                    }
                    TotalDescuento = TotalDescuento + Descuento;
                    TotalSubtotal = TotalSubtotal + Subtotal;
                    TotalIva = TotalIva + IvaAnadido;
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
                        PorcentajeDescuento = item1.PorcentajeDescuento,
                        PerteneceKitCompleto = item1.PerteneceKitCompleto,
                        CantidadDisponible = cantidadDisponible,
                        PermitirVender = PermitirVender,
                        Iva=item1.Iva,
                        CantidadDescontada = Descuento,
                        IvaAnadido = IvaAnadido
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
                ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
                _ConfigurarVenta = GestionConfigurarVenta.ConsultarConfigurarVentaPorFactura(item.CabeceraFacturaIdCabeceraFactura);
                ListaCabeceraFactura.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    EstadoCabeceraFactura = item.CabeceraFacturaEstadoCabeceraFactura,
                    Finalizado = item.CabeceraFacturaFinalizado,
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    Codigo = item.CabeceraFacturaCodigo,
                    TotalDescuento = TotalDescuento,
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
                    ConfigurarVenta = _ConfigurarVenta,
                    TotalIva = TotalIva,
                    Subtotal = TotalSubtotal,
                    Total = TotalSubtotal+TotalIva-TotalDescuento,
                });
            }
            return ListaCabeceraFactura.Where(p => p.Finalizado == false).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public List<CabeceraFactura> ConsultarFactura(string IdFactura)
        {
            //CargarDatos();
            decimal? TotalFactura = 0;
            ListaCabeceraFactura = new List<CabeceraFactura>();
            var AsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            foreach (var item in ConexionBD.sp_ConsultarCabeceraFactura().Where(p=>p.CabeceraFacturaIdCabeceraFactura == int.Parse(IdFactura)).ToList())
            {
                TotalFactura = 0;
                List<DetalleFactura> DetalleFactura = new List<Entidades.DetalleFactura>();
                foreach (var item1 in ConexionBD.sp_ConsultarDetalleFactura(item.CabeceraFacturaIdCabeceraFactura))
                {                    
                    var _AsignarProductoLote = AsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == item1.IdAsignarProductoLote.ToString()).ToList();
                    TotalFactura = TotalFactura + (_AsignarProductoLote[0].ValorUnitario * item1.Cantidad);
                    DetalleFactura.Add(new Entidades.DetalleFactura()
                    {
                        IdDetalleFactura = Seguridad.Encriptar(item1.IdDetalleFactura.ToString()),
                        IdCabeceraFactura = Seguridad.Encriptar(item1.IdCabeceraFactura.ToString()),
                        IdAsignarProductoLote = Seguridad.Encriptar(item1.IdAsignarProductoLote.ToString()),
                        Cantidad = item1.Cantidad,
                        Faltante = item1.Faltante,
                        AsignarProductoLote = _AsignarProductoLote,
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
                    Total = TotalFactura,
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
            return ListaCabeceraFactura.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == IdFactura).GroupBy(a => a.IdCabeceraFactura).Select(grp => grp.First()).ToList();
        }
        public bool FinalizarFacturaVenta(CabeceraFactura _CabeceraFactura)
        {
            try
            {
                var Factura = _CabeceraFactura;
                var ListaStock = GestionStock.ListarStock();
                if (Factura.DetalleVenta.Count > 0)
                {
                    int SePuedeVender = Factura.DetalleVenta.Where(p => p.PermitirVender == false).Count();
                    if (SePuedeVender == 0)
                    {
                        foreach (var item in Factura.DetalleVenta)
                        {
                            var stock = ListaStock.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == Seguridad.DesEncriptar(item.IdAsignarProductoLote)).FirstOrDefault();
                            int idStock = int.Parse(Seguridad.DesEncriptar(stock.IdStock));
                            ConexionBD.sp_DisminuirStock(idStock, item.Cantidad);
                            if (stock != null)
                            {
                                if (stock.AsignarProductoLote.IdLote != "")
                                {
                                    ConexionBD.sp_DisminuirLote(int.Parse(Seguridad.DesEncriptar(stock.AsignarProductoLote.IdLote)), item.Cantidad);
                                }
                            }
                        }
                        ConexionBD.sp_FinalizarCabeceraFactura(int.Parse(Seguridad.DesEncriptar(_CabeceraFactura.IdCabeceraFactura)));
                        if (Factura.ConfigurarVenta.AplicaSeguro == "True")
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsignarTecnico = new AsignarTecnicoPersonaComunidad();
                            _DatoAsignarTecnico.IdPersona = Seguridad.DesEncriptar(Factura.ConfigurarVenta.IdPersona);
                            _DatoAsignarTecnico.IdAsignarTUTecnico = "0";
                            _TecnicoPersonaComunidad.ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura1(_DatoAsignarTecnico);
                        }
                        if (Factura.ConfigurarVenta.Efectivo == "False")
                        {
                            CrearSaldoPendiente(new SaldoPendiente() {IdConfigurarVenta = Seguridad.DesEncriptar(Factura.ConfigurarVenta.IdConfigurarVenta),TotalFactura = Factura.Total,TotalInteres = Factura.Total+((Factura.Total* Factura.ConfigurarVenta.ConfiguracionInteres.TasaInteres)/100),Pendiente = Factura.Total + ((Factura.Total * Factura.ConfigurarVenta.ConfiguracionInteres.TasaInteres) / 100) });
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    ConexionBD.sp_FinalizarCabeceraFactura(int.Parse(Seguridad.DesEncriptar(_CabeceraFactura.IdCabeceraFactura)));
                    if (Factura.ConfigurarVenta.AplicaSeguro == "True")
                    {
                        AsignarTecnicoPersonaComunidad _DatoAsignarTecnico = new AsignarTecnicoPersonaComunidad();
                        _DatoAsignarTecnico.IdPersona = Seguridad.DesEncriptar(Factura.ConfigurarVenta.IdPersona);
                        _DatoAsignarTecnico.IdAsignarTUTecnico = "0";
                        _TecnicoPersonaComunidad.ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura1(_DatoAsignarTecnico);
                    }
                    return true;
                }
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
                return GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ListaAsignarProductoLote.AsignarProductoKit.IdConfigurarProducto) && p.Estado == "True").FirstOrDefault();
            }
            else
            {
                return GestionPrecioConfigurarProducto.ListarPrecioConfigurarProducto().Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == Seguridad.DesEncriptar(ListaAsignarProductoLote.ConfigurarProductos.IdConfigurarProducto) && p.Estado == "True").FirstOrDefault();
            }
        }
        public bool ListarDetalleVentaPorKit(int _IdKit,int _IdCabeceraFactura)
        {
            try
            {
                List<DetalleVenta> ListaDetalleVenta = new List<DetalleVenta>();
                CabeceraFactura ListaFacturaVenta = new CabeceraFactura();
                ListaFacturaVenta = ListarCabeceraFacturaVentaNoFinalizadas(_IdCabeceraFactura).FirstOrDefault();
                if (ListaFacturaVenta != null)
                {
                    ListaDetalleVenta = ListaFacturaVenta.DetalleVenta;
                    if (ListaDetalleVenta != null)
                    {
                        ListaDetalleVenta = ListaDetalleVenta.Where(p => p.PerteneceKitCompleto == true && p.AsignarProductoLote.PerteneceKit == "True" && Seguridad.DesEncriptar(p.AsignarProductoLote.AsignarProductoKit.Kit.IdKit) == _IdKit.ToString()).ToList();
                        foreach (var item in ListaDetalleVenta)
                        {
                            ConexionBD.sp_EliminarDetalleVenta(int.Parse(Seguridad.DesEncriptar(item.IdDetalleVenta)));
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ConfigurarVenta ConsultarConfigurarVentaPorFactura(int id_Factura)
        {
            ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
            foreach (var item3 in ConexionBD.sp_ConsultarConfigurarVentaPorIdCabeceraFactura(id_Factura))
            {
                _ConfigurarVenta.IdConfigurarVenta = Seguridad.Encriptar(item3.IdConfigurarVenta.ToString());
                _ConfigurarVenta.IdCabeceraFactura = Seguridad.Encriptar(item3.IdCabeceraFactura.ToString());
                _ConfigurarVenta.IdPersona = Seguridad.Encriptar(item3.IdPersona.ToString());
                _ConfigurarVenta.EstadoConfVenta = item3.IdConfigurarVenta.ToString();
                _ConfigurarVenta.Efectivo = item3.Efectivo.ToString();
                if (item3.IdConfiguracionInteres != null)
                {
                    _ConfigurarVenta.IdConfiguracionInteres = Seguridad.Encriptar(item3.IdConfiguracionInteres.ToString());
                }
                _ConfigurarVenta.Descuento = item3.Descuento;
            }
            return _ConfigurarVenta;
        }
        public List<FacturasPendientePorPersona> FacturasPendientePorPagarPorPersona(string numeroDocumento)
        {
            List<FacturasPendientePorPersona> ListaFacturas = new List<FacturasPendientePorPersona>();
            foreach (var item in ConexionBD.sp_ConsultarFacturasPendientePorPersona(numeroDocumento))
            {
                ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
                _ConfigurarVenta = GestionConfigurarVenta.ConsultarConfigurarVentaPorFactura(item.CabeceraFacturaIdCabeceraFactura);
                if (item.ConfigurarVentaEstadoConfVenta == true)
                {
                    _ConfigurarVenta.EstadoConfVenta = "1";
                }
                else
                {
                    _ConfigurarVenta.EstadoConfVenta = "0";
                }
                ListaFacturas.Add(new FacturasPendientePorPersona()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.CabeceraFacturaIdCabeceraFactura.ToString()),
                    Codigo = item.CabeceraFacturaCodigo,
                    IdAsignacionTU = Seguridad.Encriptar(item.CabeceraFacturaIdAsignacionTU.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.CabeceraFacturaIdTipoTransaccion.ToString()),
                    FechaGeneracion = item.CabeceraFacturaFechaGeneracion,
                    EstadoCabeceraFactura = item.CabeceraFacturaEstado_Cabecera_Factura,
                    Finalizado = item.CabeceraFacturaFinalizado,
                    TipoTransaccion = new TipoTransaccion()
                    {
                        IdTipoTransaccion = Seguridad.Encriptar(item.TipoTransaccionIdTipoTransaccion.ToString()),
                        Descripcion = item.TipoTransaccionDescripcion,
                        Identificador = item.TipoTransaccionIdentificador,
                        FechaActualizacion = item.TipoTransaccionFechaModificacion,
                        FechaCreacion = item.TipoTransaccionFechaCreacion
                    },
                    ConfigurarVenta = _ConfigurarVenta
                });
            }
            return ListaFacturas;
        }
        public SaldoPendiente CrearSaldoPendiente(SaldoPendiente _SaldoPendiente)
        {
            foreach (var item in ConexionBD.sp_CrearSaldoPendiente(int.Parse(_SaldoPendiente.IdConfigurarVenta), _SaldoPendiente.TotalFactura, _SaldoPendiente.TotalInteres, _SaldoPendiente.Pendiente))
            {
                _SaldoPendiente.IdConfigurarVenta = Seguridad.Encriptar(item.IdConfiguracionVenta.ToString());
                _SaldoPendiente.IdSaldoPendiente = Seguridad.Encriptar(item.IdSaldoPendiente.ToString());
                _SaldoPendiente.FechaRegistro = item.FechaRegistro;
                _SaldoPendiente.Pendiente = item.Pendiente;
                _SaldoPendiente.TotalFactura = item.TotalFactura;
                _SaldoPendiente.TotalInteres = item.TotalInteres;
            }
            return _SaldoPendiente;
        }

        public List<CabeceraFactura> ConsultarFacturaPorId(int idCabeceraFactura)
        {
            List<CabeceraFactura> _Factura = new List<CabeceraFactura>();
            foreach (var item in ConexionBD.sp_ConsultarFacturaPorId(idCabeceraFactura))
            {
                _Factura.Add(new CabeceraFactura()
                {
                    IdCabeceraFactura = Seguridad.Encriptar(item.IdCabeceraFactura.ToString()),
                    IdTipoTransaccion = Seguridad.Encriptar(item.IdTipoTransaccion.ToString()),
                    Finalizado = item.Finalizado
                });
            }
            return _Factura;
        }
        public bool EliminarFacturaPorId(CabeceraFactura _CabeceraFactura)
        {
            try
            {
                ConexionBD.sp_EliminarFacturasPorId(int.Parse(_CabeceraFactura.IdCabeceraFactura), int.Parse(_CabeceraFactura.IdTipoTransaccion));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
