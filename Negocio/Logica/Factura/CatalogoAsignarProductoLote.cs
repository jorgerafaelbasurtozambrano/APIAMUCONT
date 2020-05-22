using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.Inventario;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Logica.Factura
{
    public class CatalogoAsignarProductoLote
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        List<AsignarProductoLote> ListaAsignarProductoLote;
        List<AsignarProductosKits> ListaAsignarProductoKit;
        CatalogoAsignarProductoKit GestionAsignarProductoKit = new CatalogoAsignarProductoKit();
        CatalogoConfigurarProducto GestionConfigurarProducto = new CatalogoConfigurarProducto();
        CatalogoLote GestionLote = new CatalogoLote();

        public object InsertarAsignarProductoLote(AsignarProductoLote AsignarProductoLote)
        {
            if (AsignarProductoLote.Cantidad > 0 )
            {
                AsignarProductoLote.IdCabeceraFactura = Seguridad.DesEncriptar(AsignarProductoLote.IdCabeceraFactura);
                AsignarProductoLote.PerteneceKit = Convert.ToBoolean(AsignarProductoLote.PerteneceKit).ToString();
                if (AsignarProductoLote.IdLote == null)
                {
                    List<DetalleFactura> DetalleFactura = new List<DetalleFactura>();
                    DetalleFactura = BuscarInformacionDetalle(AsignarProductoLote);
                    if (DetalleFactura.Count == 0)
                    {
                        foreach (var item in ConexionBD.sp_CrearAsignarProductoLote(null, int.Parse(AsignarProductoLote.IdRelacionLogica), Convert.ToBoolean(AsignarProductoLote.PerteneceKit), AsignarProductoLote.FechaExpiracion, AsignarProductoLote.ValorUnitario))
                        {
                            AsignarProductoLote.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                            AsignarProductoLote.IdLote = Seguridad.Encriptar(item.IdLote.ToString());
                            AsignarProductoLote.IdRelacionLogica = Seguridad.Encriptar(item.IdRelacionLogica.ToString());
                            AsignarProductoLote.PerteneceKit = item.PerteneceKit.ToString();
                            AsignarProductoLote.FechaExpiracion = item.FechaExpiracion;
                        }
                        return AsignarProductoLote;
                    }
                    else
                    {
                        var id = Seguridad.DesEncriptar(DetalleFactura.FirstOrDefault().IdCabeceraFactura);
                        var Detalle = DetalleFactura.Where(p => Seguridad.DesEncriptar(p.IdCabeceraFactura) == AsignarProductoLote.IdCabeceraFactura).FirstOrDefault();
                        if (Detalle != null)
                        {
                            ConexionBD.sp_AumentarDetalleFactura(int.Parse(Seguridad.DesEncriptar(Detalle.IdDetalleFactura)), Detalle.Cantidad + AsignarProductoLote.Cantidad);
                            return true;
                        }
                        else
                        {
                            AsignarProductoLote.IdAsignarProductoLote = DetalleFactura.FirstOrDefault().IdAsignarProductoLote.ToString();
                            AsignarProductoLote.IdLote = DetalleFactura.FirstOrDefault().AsignarProductoLote[0].IdLote;
                            AsignarProductoLote.IdRelacionLogica = DetalleFactura.FirstOrDefault().AsignarProductoLote[0].IdRelacionLogica.ToString();
                            AsignarProductoLote.PerteneceKit = DetalleFactura.FirstOrDefault().AsignarProductoLote[0].PerteneceKit.ToString();
                            AsignarProductoLote.FechaExpiracion = DetalleFactura.FirstOrDefault().AsignarProductoLote[0].FechaExpiracion;
                            return AsignarProductoLote;
                        }
                    }
                }
                else
                {
                    Lote Lote = new Lote();
                    Lote = GestionLote.CargarTodosLosLotes().Where(p => Seguridad.DesEncriptar(p.IdLote) == AsignarProductoLote.IdLote).FirstOrDefault();
                    if (Lote.LoteUtilizado == "0")
                    {
                        foreach (var item in ConexionBD.sp_CrearAsignarProductoLote(int.Parse(AsignarProductoLote.IdLote), int.Parse(AsignarProductoLote.IdRelacionLogica), Convert.ToBoolean(AsignarProductoLote.PerteneceKit), null, AsignarProductoLote.ValorUnitario))
                        {
                            AsignarProductoLote.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                            AsignarProductoLote.IdLote = Seguridad.Encriptar(item.IdLote.ToString());
                            AsignarProductoLote.IdRelacionLogica = Seguridad.Encriptar(item.IdRelacionLogica.ToString());
                            AsignarProductoLote.PerteneceKit = item.PerteneceKit.ToString();
                            AsignarProductoLote.FechaExpiracion = item.FechaExpiracion;
                        }
                        return AsignarProductoLote;
                    }
                    else
                    {
                        AsignarProductoLote AsignarProductoLotes = new AsignarProductoLote();
                        AsignarProductoLotes = ListarAsignarProductoLote().Where(p => Seguridad.DesEncriptar(p.IdLote) == AsignarProductoLote.IdLote && Seguridad.DesEncriptar(p.IdRelacionLogica) == AsignarProductoLote.IdRelacionLogica && p.PerteneceKit == AsignarProductoLote.PerteneceKit && p.Lote.FechaExpiracion.ToString() == AsignarProductoLote.FechaExpiracion.ToString()).FirstOrDefault();
                        Object Data = validarLoteExiste(GestionLote.CargarTodosLosLotes().Where(p => Seguridad.DesEncriptar(p.IdLote) == AsignarProductoLote.IdLote).FirstOrDefault().Codigo, AsignarProductoLote.IdRelacionLogica, AsignarProductoLote.PerteneceKit);
                        try
                        {
                            if (Data.ToString() == "404" || Convert.ToBoolean(Data) == false)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        catch (Exception)
                        {
                            if (AsignarProductoLotes == null)
                            {
                                foreach (var item in ConexionBD.sp_CrearAsignarProductoLote(int.Parse(AsignarProductoLote.IdLote), int.Parse(AsignarProductoLote.IdRelacionLogica), Convert.ToBoolean(AsignarProductoLote.PerteneceKit), null, AsignarProductoLote.ValorUnitario))
                                {
                                    AsignarProductoLote.IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString());
                                    AsignarProductoLote.IdLote = Seguridad.Encriptar(item.IdLote.ToString());
                                    AsignarProductoLote.IdRelacionLogica = Seguridad.Encriptar(item.IdRelacionLogica.ToString());
                                    AsignarProductoLote.PerteneceKit = item.PerteneceKit.ToString();
                                    AsignarProductoLote.FechaExpiracion = item.FechaExpiracion;
                                }
                                return AsignarProductoLote;
                            }
                            else
                            {
                                var ConsultaDetalle1 = ConexionBD.sp_ConsultarTodosDetallesFactura().Where(p => p.IdAsignarProductoLote.ToString() == Seguridad.DesEncriptar(AsignarProductoLotes.IdAsignarProductoLote));
                                var ConsultaDetalle = ConsultaDetalle1.Where(p => p.IdCabeceraFactura.ToString() == AsignarProductoLote.IdCabeceraFactura).FirstOrDefault();

                                if (ConsultaDetalle != null)
                                {
                                    ConexionBD.sp_AumentarDetalleFactura(ConsultaDetalle.IdDetalleFactura, ConsultaDetalle.Cantidad + AsignarProductoLote.Cantidad);
                                    //ConexionBD.sp_AumentarLote(int.Parse(AsignarProductoLote.IdLote), AsignarProductoLote.Cantidad);
                                    return true;
                                }
                                else
                                {
                                    AsignarProductoLote.IdAsignarProductoLote = AsignarProductoLotes.IdAsignarProductoLote.ToString();
                                    AsignarProductoLote.IdLote = AsignarProductoLotes.IdLote.ToString();
                                    AsignarProductoLote.IdRelacionLogica = AsignarProductoLotes.IdRelacionLogica.ToString();
                                    AsignarProductoLote.PerteneceKit = AsignarProductoLotes.PerteneceKit.ToString();
                                    AsignarProductoLote.FechaExpiracion = AsignarProductoLotes.FechaExpiracion;
                                    return AsignarProductoLote;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
        public void CargarDatos()
        {
            ListaAsignarProductoLote = new List<AsignarProductoLote>();
            var listaConfigurarProducto = GestionConfigurarProducto.ListarConfigurarProductos();
            var listaLotes = GestionLote.CargarTodosLosLotes();
            foreach (var item in ConexionBD.sp_ConsultarAsignarProductoLote())
            {
                Lote Lote = new Lote();
                AsignarProductoKit AsignarProductosKits = new AsignarProductoKit();
                ConfigurarProductos ConfigurarProductos = new ConfigurarProductos();
                if (item.PerteneceKit == true)
                {
                    //AsignarProductosKits = BuscarDato(item.IdRelacionLogica.ToString());
                    AsignarProductosKits = BuscarDato(item.IdRelacionLogica.ToString());
                }
                else
                {
                    ConfigurarProductos = listaConfigurarProducto.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == item.IdRelacionLogica.ToString()).FirstOrDefault();
                }
                if (item.IdLote == null)
                {
                    Lote = null;
                }
                else
                {
                    Lote = listaLotes.Where(p => Seguridad.DesEncriptar(p.IdLote) == item.IdLote.ToString()).FirstOrDefault();
                }
                ListaAsignarProductoLote.Add(new AsignarProductoLote()
                {
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    IdRelacionLogica = Seguridad.Encriptar(item.IdRelacionLogica.ToString()),
                    PerteneceKit = item.PerteneceKit.ToString(),
                    FechaExpiracion = item.FechaExpiracion,
                    ValorUnitario = item.ValorUnitario,
                    Lote = Lote,
                    ConfigurarProductos = ConfigurarProductos,
                    AsignarProductoKit = AsignarProductosKits,
                });
            }
            
        }
        public AsignarProductoKit BuscarDato(string IdRelacionLogica)
        {
            ListaAsignarProductoKit = new List<AsignarProductosKits>();
            ListaAsignarProductoKit = GestionAsignarProductoKit.ListarAsignarProductosKit();
            AsignarProductoKit AsignarProductoKit = new AsignarProductoKit();
            //AsignarProductosKits AsignarProductosKits = new AsignarProductosKits();
            for (int i = 0; i < ListaAsignarProductoKit.Count; i++)
            {
                for (int i1 = 0; i1 < ListaAsignarProductoKit[i].ListaAsignarProductoKit.Count; i1++)
                {
                    if (Seguridad.DesEncriptar(ListaAsignarProductoKit[i].ListaAsignarProductoKit[i1].IdAsignarProductoKit) == IdRelacionLogica)
                    {
                        //AsignarProductosKits = ListaAsignarProductoKit[i];
                        AsignarProductoKit = ListaAsignarProductoKit[i].ListaAsignarProductoKit[i1];
                        break;
                    }
                }
            }
            return AsignarProductoKit;
        }
        public List<AsignarProductoLote> ListarAsignarProductoLote()
        {
            CargarDatos();
            return ListaAsignarProductoLote;
        }
        public List<DetalleFactura> BuscarInformacionDetalle(AsignarProductoLote AsignarProductoLote)
        {
            List<DetalleFactura> DetalleFactura = new List<DetalleFactura>();
            List<AsignarProductoLote> AsignarProductoLotes = new List<AsignarProductoLote>();
            try
            {
                if (AsignarProductoLote.FechaExpiracion == null)
                {
                    foreach (var item in ConexionBD.sp_ConsultarDetalleFacturaExiste(int.Parse(AsignarProductoLote.IdCabeceraFactura), int.Parse(AsignarProductoLote.IdRelacionLogica), Convert.ToBoolean(AsignarProductoLote.PerteneceKit), AsignarProductoLote.FechaExpiracion.ToString(), 0))
                    {
                        if (item.AsignarProductoLoteIdLote != null)
                        {
                            AsignarProductoLotes.Add(new AsignarProductoLote()
                            {
                                IdLote = Seguridad.Encriptar(item.AsignarProductoLoteIdLote.ToString()),
                                IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                                IdRelacionLogica = Seguridad.Encriptar(item.AsignarProductoLoteIdRelacionLogica.ToString()),
                                PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                                ValorUnitario = item.AsignarProductoLoteValorUnitario,
                            });
                        }
                        else
                        {
                            AsignarProductoLotes.Add(new AsignarProductoLote()
                            {
                                IdLote = "",
                                IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                                IdRelacionLogica = Seguridad.Encriptar(item.AsignarProductoLoteIdRelacionLogica.ToString()),
                                PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                                ValorUnitario = item.AsignarProductoLoteValorUnitario,
                            });
                        }
                        DetalleFactura.Add(new Entidades.DetalleFactura()
                        {
                            IdDetalleFactura = Seguridad.Encriptar(item.DetalleFacturaIdDetalleFactura.ToString()),
                            IdCabeceraFactura = Seguridad.Encriptar(item.DetalleFacturaIdCabeceraFactura.ToString()),
                            IdAsignarProductoLote = Seguridad.Encriptar(item.DetalleFacturaIdAsignarProductoLote.ToString()),
                            Cantidad = item.DetalleFacturaCantidad,
                            Faltante = item.DetalleFacturaFaltante,
                            AsignarProductoLote = AsignarProductoLotes,
                        });
                    }
                }
                else
                {
                    string Mes,Dia;
                    if (AsignarProductoLote.FechaExpiracion.Value.Month >=1 && AsignarProductoLote.FechaExpiracion.Value.Month<=9)
                    {
                        Mes = "0" + AsignarProductoLote.FechaExpiracion.Value.Month.ToString();
                    }
                    else
                    {
                        Mes = AsignarProductoLote.FechaExpiracion.Value.Month.ToString();
                    }

                    if (AsignarProductoLote.FechaExpiracion.Value.Day >= 1 && AsignarProductoLote.FechaExpiracion.Value.Day <= 9)
                    {
                        Dia = "0" + AsignarProductoLote.FechaExpiracion.Value.Day.ToString();
                    }
                    else
                    {
                        Dia = AsignarProductoLote.FechaExpiracion.Value.Day.ToString();
                    }
                    string Fecha = (AsignarProductoLote.FechaExpiracion.Value.Year + "-" + Mes + "-" + Dia + " " + AsignarProductoLote.FechaExpiracion.Value.TimeOfDay);
                    foreach (var item in ConexionBD.sp_ConsultarDetalleFacturaExiste(int.Parse(AsignarProductoLote.IdCabeceraFactura), int.Parse(AsignarProductoLote.IdRelacionLogica), Convert.ToBoolean(AsignarProductoLote.PerteneceKit),Fecha , 1))
                    {
                        if (item.AsignarProductoLoteIdLote != null)
                        {
                            AsignarProductoLotes.Add(new AsignarProductoLote()
                            {
                                IdLote = Seguridad.Encriptar(item.AsignarProductoLoteIdLote.ToString()),
                                IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                                IdRelacionLogica = Seguridad.Encriptar(item.AsignarProductoLoteIdRelacionLogica.ToString()),
                                PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                                ValorUnitario = item.AsignarProductoLoteValorUnitario,
                            });
                        }
                        else
                        {
                            AsignarProductoLotes.Add(new AsignarProductoLote()
                            {
                                IdLote = "",
                                IdAsignarProductoLote = Seguridad.Encriptar(item.AsignarProductoLoteIdAsignarProductoLote.ToString()),
                                IdRelacionLogica = Seguridad.Encriptar(item.AsignarProductoLoteIdRelacionLogica.ToString()),
                                PerteneceKit = item.AsignarProductoLotePerteneceKit.ToString(),
                                ValorUnitario = item.AsignarProductoLoteValorUnitario,
                            });
                        }
                        DetalleFactura.Add(new Entidades.DetalleFactura()
                        {
                            IdDetalleFactura = Seguridad.Encriptar(item.DetalleFacturaIdDetalleFactura.ToString()),
                            IdCabeceraFactura = Seguridad.Encriptar(item.DetalleFacturaIdCabeceraFactura.ToString()),
                            IdAsignarProductoLote = Seguridad.Encriptar(item.DetalleFacturaIdAsignarProductoLote.ToString()),
                            Cantidad = item.DetalleFacturaCantidad,
                            Faltante = item.DetalleFacturaFaltante,
                            AsignarProductoLote = AsignarProductoLotes,
                        });
                    }
                }
                return DetalleFactura;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public AsignarProductoLote ConsultarDatosAsignarProductoLote(string IdAsignacionProductoLote)
        {
            CargarDatos();
            return ListaAsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == IdAsignacionProductoLote).FirstOrDefault();
        }
        public object validarLoteExiste(string codigo,string idRelacionLogica,string perteneceKit)
        {
            try
            {
                Lote Lote = new Lote();
                Lote = GestionLote.CargarTodosLosLotes().Where(p => p.Codigo == codigo).FirstOrDefault();
                if (Lote!=null)
                {
                    AsignarProductoLote AsignarProductoLote = new AsignarProductoLote();
                    AsignarProductoLote = ListarAsignarProductoLote().Where(p => Seguridad.DesEncriptar(p.IdLote) == Seguridad.DesEncriptar(Lote.IdLote) && Seguridad.DesEncriptar(p.IdRelacionLogica) == idRelacionLogica && p.PerteneceKit == perteneceKit).FirstOrDefault();
                    if (AsignarProductoLote != null)
                    {
                        Lote.AsignarProductoLote = AsignarProductoLote;
                        return AsignarProductoLote;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return "404";
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<AsignarProductoLote> filtrarAsignarProductoLote(int IdAsignacionProductoLote)
        {
            var listaConfigurarProducto = GestionConfigurarProducto.ListarConfigurarProductos();
            var listaLotes = GestionLote.CargarTodosLosLotes();
            List<AsignarProductoLote> _DataAsignarProductoLote = new List<AsignarProductoLote>();
            foreach (var item in ConexionBD.sp_BuscarAsignarProductoLote(IdAsignacionProductoLote))
            {
                Lote Lote = new Lote();
                AsignarProductoKit AsignarProductosKits = new AsignarProductoKit();
                ConfigurarProductos ConfigurarProductos = new ConfigurarProductos();
                if (item.PerteneceKit == true)
                {
                    //AsignarProductosKits = BuscarDato(item.IdRelacionLogica.ToString());
                    AsignarProductosKits = BuscarDato(item.IdRelacionLogica.ToString());
                }
                else
                {
                    ConfigurarProductos = listaConfigurarProducto.Where(p => Seguridad.DesEncriptar(p.IdConfigurarProducto) == item.IdRelacionLogica.ToString()).FirstOrDefault();
                }
                if (item.IdLote == null)
                {
                    Lote = null;
                }
                else
                {
                    Lote = listaLotes.Where(p => Seguridad.DesEncriptar(p.IdLote) == item.IdLote.ToString()).FirstOrDefault();
                }
                _DataAsignarProductoLote.Add(new AsignarProductoLote()
                {
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    IdRelacionLogica = Seguridad.Encriptar(item.IdRelacionLogica.ToString()),
                    PerteneceKit = item.PerteneceKit.ToString(),
                    FechaExpiracion = item.FechaExpiracion,
                    ValorUnitario = item.ValorUnitario,
                    Lote = Lote,
                    ConfigurarProductos = ConfigurarProductos,
                    AsignarProductoKit = AsignarProductosKits,
                });
            }
            return _DataAsignarProductoLote;
        }
    }
}
