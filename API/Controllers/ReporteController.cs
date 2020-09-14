using Rotativa.Core.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Negocio.Logica.Rubros;
using Negocio.Entidades;
using System.Net.Http;
using System.Net;
using Negocio.Logica.Factura;
using Negocio.Logica.Reporte;
using Negocio.Entidades.Reporte;
using System.IO;
using Negocio.Logica.Inventario;

namespace API.Controllers
{
    public class ReporteController : Controller
    {
        CatalogoTicket _CatalogoTicket = new CatalogoTicket();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoVentaRubro GestionVentaRubro = new CatalogoVentaRubro();
        CatalogoReporte GestionReporte = new CatalogoReporte();
        // GET: Reporte
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Stock()
        {
            CatalogoStock GestionStock = new CatalogoStock();
            List<Stock> _ListaStock = new List<Stock>();
            _ListaStock = GestionStock.ListarStock();
            ViewBag.Datos = _ListaStock;
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("Stock")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };

        }
        public ActionResult Ticket(string Ticket)
        {
            Ticket = Seguridad.DesEncriptar(Ticket);
            Ticket _Ticket = new Ticket();
            _Ticket = _CatalogoTicket.ConsultarTiketsPorId(int.Parse(Ticket)).FirstOrDefault();
            //ViewBag.Datos = _Ticket._TipoRubro.Descripcion;
            ViewBag.Datos = _Ticket;
            return new Rotativa.MVC.PartialViewAsPdf("Ticket")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    PageHeight = 105,
                    PageMargins = new Margins(5,25,25,25)
                    //PageWidth = 210,
                }
            };
        }
        public ActionResult Venta(string Ticket)
        {
            Ticket = Seguridad.DesEncriptar(Ticket);
            TicketVenta _Ticket = new TicketVenta();
            _Ticket = GestionVentaRubro.ConsultarTicketVentaRubroPorId(int.Parse(Ticket)).FirstOrDefault();
            //ViewBag.Datos = _Ticket._TipoRubro.Descripcion;
            ViewBag.Datos = _Ticket;
            return new Rotativa.MVC.PartialViewAsPdf("Venta")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    PageHeight = 105,
                    PageMargins = new Margins(5, 25, 25, 25)
                    //PageWidth = 210,
                }
            };
        }
        public ActionResult Estadistica(string Inicio,string Fin)
        {
            PromedioPreciosCompraVenta Dato = new PromedioPreciosCompraVenta();
            Dato = GestionReporte.PrecioCompra(Inicio,Fin);
            ViewBag.Fechas = Dato.Fecha.ToArray();
            ViewBag.PrecioCompra = Dato.PrecioCompra.ToArray();
            ViewBag.PrecioVenta = Dato.PrecioVenta.ToArray();
            ViewBag.Title = "PROMEDIO DE PRECIOS COMPRAS/VENTAS";
            ViewBag.yAxis = "PRECIO";
            ViewBag.tooltip = " dólares";
            ViewBag.series_a = "COMPRA";
            ViewBag.series_b = "VENTA";
            return View("Grafico");
        }
        public ActionResult Compra(string Identificacion,string Inicio, string Fin,string Identificador)
        {
            ViewBag.TipoRubro = "";
            List<Ticket> _Ticket = new List<Ticket>();
            if (Identificacion != null && Identificacion !="")
            {
                _Ticket = GestionReporte.ConsultarCompraRubroPorPersona(Identificacion, Inicio, Fin);
                ViewBag.Datos = _Ticket;
                ViewBag.Carro = _Ticket.Where(p => p._Vehiculo != null).ToList();
                ViewBag.Saco = _Ticket.Where(p => p._Vehiculo == null).ToList();
            }
            else
            {
                if (Identificador!=null&&Identificador!="")
                {
                    Identificador = Identificador.Trim();
                    _Ticket = GestionReporte.ConsultarCompraRubroPorPresentacionRubro(Identificador.Trim(), Inicio, Fin);
                    if (_Ticket.Count>0)
                    {
                        ViewBag.TipoRubro = "POR "+_Ticket[0]._TipoPresentacionRubro.Descripcion;
                    }
                    else
                    {
                        ViewBag.TipoRubro = "";
                    }
                    ViewBag.Datos = _Ticket;
                }
            }
            ViewBag.Inicio = Inicio;
            ViewBag.Fin = Fin;
            ViewBag.Identificacion = Identificacion;
            ViewBag.Identificador = Identificador;
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("Compra")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    PageMargins = new Rotativa.Core.Options.Margins(5, 0, 10, 0),
                    CustomSwitches = footer,
                }
            };
        }
        public ActionResult ReporteVenta(string Identificacion,string Inicio, string Fin, string Identificador)
        {
            List<TicketVenta> _Ticket = new List<TicketVenta>();
            ViewBag.TipoRubro = "";
            if (Identificacion != null && Identificacion != "")
            {
                _Ticket = GestionReporte.ConsultarVentaRubroPorPersona(Identificacion, Inicio, Fin);
                ViewBag.Datos = _Ticket;
                ViewBag.Carro = _Ticket.Where(p => p._TipoPresentacionRubro.Identificador == 1).ToList();
                ViewBag.Saco = _Ticket.Where(p => p._TipoPresentacionRubro.Identificador != 1).ToList();
            }
            else
            {
                _Ticket = GestionReporte.ConsultarVentaRubroPorPresentacionRubro("1", Inicio, Fin);
                if (_Ticket.Count > 0)
                {
                    //ViewBag.TipoRubro = "POR " + _Ticket[0]._TipoPresentacionRubro.Descripcion;
                }
                else
                {
                    ViewBag.TipoRubro = "";
                }
                ViewBag.Datos = _Ticket;
                ViewBag.Saco = GestionReporte.ConsultarVentaRubroPorPresentacionRubro("2", Inicio, Fin); ;
            }
            ViewBag.Inicio = Inicio;
            ViewBag.Fin = Fin;
            ViewBag.Identificacion = Identificacion;
            ViewBag.Identificador = Identificador;
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("ReporteVenta")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    PageMargins = new Rotativa.Core.Options.Margins(5, 0, 10, 0),
                    CustomSwitches = footer,
                }
            };
        }
        public ActionResult PDF(string Inicio, string Fin)
        {
            ComparacionQuintales _ComparacionQuintales = new ComparacionQuintales();
            _ComparacionQuintales = GestionReporte.ConsultarComparacionQuintales(Inicio, Fin);
            ViewBag.PorcentajeQuintalesComprados = Math.Round(_ComparacionQuintales.PorcentajeQuintalesComprados, 1);
            ViewBag.PorcentajeQuintalesVendidos = Math.Round(_ComparacionQuintales.PorcentajeQuintalesVendidos, 1);
            ViewBag.QuintalesVendidos = _ComparacionQuintales.QuintalesVendidos;
            ViewBag.QuintalesComprados = _ComparacionQuintales.QuintalesComprados;
            ViewBag.Diferencia = _ComparacionQuintales.QuintalesComprados - _ComparacionQuintales.QuintalesVendidos;
            ViewBag.Inicio = Inicio;
            ViewBag.Fin = Fin;
            ViewBag.Invertido = GestionReporte.ConsultarValorInvertidoCompra(Inicio, Fin);
            ViewBag.Recuperado = GestionReporte.ConsultarValorRecuperadoVenta(Inicio, Fin);
            ViewBag.Pdf = true;
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("ReporteQuintales")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    CustomSwitches = footer,
                    PageMargins = new Rotativa.Core.Options.Margins(15, 15, 15, 15),
                }
            };
        }
        public ActionResult ReporteQuintales(string Inicio, string Fin)
        {
            ComparacionQuintales _ComparacionQuintales = new ComparacionQuintales();
            _ComparacionQuintales = GestionReporte.ConsultarComparacionQuintales(Inicio, Fin);
            ViewBag.PorcentajeQuintalesComprados = Math.Round(_ComparacionQuintales.PorcentajeQuintalesComprados, 1);
            ViewBag.PorcentajeQuintalesVendidos = Math.Round(_ComparacionQuintales.PorcentajeQuintalesVendidos, 1);
            ViewBag.QuintalesVendidos = _ComparacionQuintales.QuintalesVendidos;
            ViewBag.QuintalesComprados = _ComparacionQuintales.QuintalesComprados;
            ViewBag.Diferencia = _ComparacionQuintales.QuintalesComprados - _ComparacionQuintales.QuintalesVendidos;
            ViewBag.Inicio = Inicio;
            ViewBag.Fin = Fin;
            ViewBag.Invertido = GestionReporte.ConsultarValorInvertidoCompra(Inicio, Fin);
            ViewBag.Recuperado = GestionReporte.ConsultarValorRecuperadoVenta(Inicio, Fin);
            ViewBag.Pdf = false;
            return View("ReporteQuintales");
        }
        public ActionResult Humedad(string Inicio, string Fin)
        {
            PromedioPreciosCompraVenta Dato = new PromedioPreciosCompraVenta();
            Dato = GestionReporte.HumedadPromedio(Inicio, Fin);
            ViewBag.Fechas = Dato.Fecha.ToArray();
            ViewBag.PrecioCompra = Dato.PrecioCompra.ToArray();
            ViewBag.PrecioVenta = Dato.PrecioVenta.ToArray();
            ViewBag.Title = "PROMEDIO PORCENTAJE DE HUMEDAD";
            ViewBag.series_a = "COMPRA";
            ViewBag.series_b = "VENTA";
            ViewBag.yAxis = "% HUMEDAD";
            ViewBag.tooltip = "%";
            return View("Grafico");
        }
        public ActionResult FacturasPendientes(string Inicio, string Fin,string Persona)
        {
            List<CabeceraFactura> _CabeceraFactura = new List<CabeceraFactura>();
            if (Persona != null)
            {
                Persona = Seguridad.DesEncriptar(Persona);
                _CabeceraFactura = GestionReporte.ConsultarFacturasPendientesPorIdPersona(Persona);
            }
            else
            {
                _CabeceraFactura = GestionReporte.ConsultarFacturasPendientes(Inicio, Fin);
            }
            ViewBag.Datos = _CabeceraFactura;
            ViewBag.Titulo = _CabeceraFactura.Count.ToString() + " FACTURAS PENDIENTES";
            ViewBag.Columna = "PENDIENTE";
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("FacturasPendientes")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult FacturasVencidas(string Inicio, string Fin)
        {
            List<CabeceraFactura> _CabeceraFactura = new List<CabeceraFactura>();
            _CabeceraFactura = GestionReporte.ConsultarFacturasVencidas(Inicio, Fin);
            ViewBag.Datos = _CabeceraFactura;
            ViewBag.Titulo = _CabeceraFactura.Count.ToString() + " FACTURAS VENCIDAS";
            ViewBag.Columna = "PENDIENTE";
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("FacturasPendientes")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult FacturasCanceladas(string Inicio, string Fin)
        {
            List<CabeceraFactura> _CabeceraFactura = new List<CabeceraFactura>();
            _CabeceraFactura = GestionReporte.ConsultarFacturasCanceladas(Inicio, Fin);
            ViewBag.Datos = _CabeceraFactura;
            ViewBag.Titulo = _CabeceraFactura.Count.ToString() + " FACTURAS CANCELADAS";
            ViewBag.Columna = "TOTAL";
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("FacturasPendientes")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult FacturasEmitidas(string Inicio, string Fin)
        {
            List<CabeceraFactura> _CabeceraFactura = new List<CabeceraFactura>();
            _CabeceraFactura = GestionReporte.ConsultarFacturasEmitidas(Inicio, Fin);
            ViewBag.Datos = _CabeceraFactura;
            ViewBag.Titulo = _CabeceraFactura.Count.ToString() + " FACTURAS EMITIDAS";
            ViewBag.Columna = "TOTAL";
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("FacturasPendientes")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Landscape,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult Factura(string Factura)
        {
            CatalogoCabeceraFactura GestionFactura = new CatalogoCabeceraFactura();
            CabeceraFactura _Factura = new CabeceraFactura();
            Factura = Seguridad.DesEncriptar(Factura);
            _Factura = GestionFactura.ConsultarCabeceraFacturaVentaFinalizada(int.Parse(Factura)).FirstOrDefault();
            if (_Factura == null)
            {
                _Factura = GestionFactura.ConsultarFactura(Factura).FirstOrDefault();
                ViewBag.Datos = _Factura;
                ViewBag.Tipo = "COMPRA";
            }
            else
            {
                ViewBag.Datos = _Factura;
                ViewBag.Titulo = "FACTURA " + _Factura.Codigo;
                ViewBag.Columna = "FACTURA";
            }
            string footer ="--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("FacturasPendientes")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult FacturaCompras(string Inicio,string Fin)
        {
            CatalogoCabeceraFactura GestionFactura = new CatalogoCabeceraFactura();
            List<CabeceraFactura> _Factura = new List<CabeceraFactura>();
            _Factura = GestionFactura.ListarCabeceraFacturaFinalizadasPorRangoFecha(Inicio, Fin);
            ViewBag.Datos = _Factura;
            ViewBag.Titulo = _Factura.Count.ToString()+ " FACTURAS COMPRADAS ";            
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("FacturaCompras")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult Abono(string Abono)
        {
            CatalogoCabeceraFactura GestionFactura = new CatalogoCabeceraFactura();
            CabeceraFactura _Factura = new CabeceraFactura();
            Abono _Abono = new Abono();
            Abono = Seguridad.DesEncriptar(Abono);
            _Abono = GestionReporte.ConsultarAbonoPorId(Abono).FirstOrDefault();
            _Factura = GestionFactura.ConsultarCabeceraFacturaVentaFinalizada(int.Parse(_Abono._ConfigurarVenta.IdCabeceraFactura)).FirstOrDefault();
            ViewBag.Datos = _Factura;
            ViewBag.Abono = _Abono;
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("Abono")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult Tecnicos(string Tecnico)
        {
            ViewBag.Pendiente = false;
            if (Tecnico != null)
            {
                Tecnico = Seguridad.DesEncriptar(Tecnico);
                ViewBag.Datos = GestionReporte.ConsultarTecnicosParaSeguimientoPorTecnico(int.Parse(Tecnico));
            }
            else
            {
                ViewBag.Datos = GestionReporte.ConsultarTecnicosParaSeguimiento();
            }
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("Tecnicos")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult TecnicosInformacion()
        {
            ViewBag.Pendiente = true;
            ViewBag.Datos = GestionReporte.ConsultarTecnicosParaSeguimientoGerencia();
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("Tecnicos")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        public ActionResult Inversion(string Inicio, string Fin)
        {
            CatalogoCabeceraFactura GestionFactura = new CatalogoCabeceraFactura();
            ViewBag.Invertido = GestionFactura.InversionRealizadasPorRangoFecha(Inicio, Fin);
            Decimal?[] Totales = GestionFactura.ConsultarFacturasVendidasEnEfectivo(Inicio, Fin);
            ViewBag.Efectivo = Totales[0];
            ViewBag.Finalizada = Totales[1];
            ViewBag.Cancelada = Totales[2];
            ViewBag.Subtitulo = Inicio + " HASTA " + Fin;
            ViewBag.Inicio = Inicio;
            ViewBag.Fin = Fin;
            string footer = "--footer-center \"Impreso el: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + " Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new Rotativa.MVC.PartialViewAsPdf("Inversion")
            {
                RotativaOptions = new Rotativa.Core.DriverOptions()
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.Core.Options.Size.A4,
                    IsLowQuality = true,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    CustomSwitches = footer
                }
            };
        }
        [HttpPost]
        public string SaveImage(string base64image)
        {
            if (string.IsNullOrEmpty(base64image))
                return "";

            var t = base64image.Substring(22);  // remove data:image/png;base64,

            byte[] bytes = Convert.FromBase64String(t);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            var randomFileName = "Quintales" + ".png";
            var fullPath = Path.Combine(Server.MapPath("~/Imagenes/"), randomFileName);
            image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
            return fullPath;
        }
    }
}