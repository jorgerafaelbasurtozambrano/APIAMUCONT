using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Rubros;
namespace API.Controllers
{
    public class TicketVentaController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoVentaRubro GestionTicketVenta = new CatalogoVentaRubro();
        CatalogoTipoPresentacionRubro GestionTipoPresentacionRubro = new CatalogoTipoPresentacionRubro();
        [HttpPost]
        [Route("api/Rubros/IngresarVentaRubro")]
        public object IngresarVentaRubro(TicketVenta _TicketVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_TicketVenta.encriptada == null || string.IsNullOrEmpty(_TicketVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_TicketVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_TicketVenta._TipoPresentacionRubro == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese el tipo presentacion rubro";
                        }
                        else if (_TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro == null || string.IsNullOrEmpty(_TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tipo presentacion del rubro";
                        }
                        if (_TicketVenta._TipoRubro == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese el tipo de rubro";
                        }
                        else if (_TicketVenta._TipoRubro.IdTipoRubro == null || string.IsNullOrEmpty(_TicketVenta._TipoRubro.IdTipoRubro.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tipo rubro";
                        }
                        else if (_TicketVenta.IdPersonaCliente == null || string.IsNullOrEmpty(_TicketVenta.IdPersonaCliente.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id de la persona";
                        }
                        else if (_TicketVenta.IdAsignarTU == null || string.IsNullOrEmpty(_TicketVenta.IdAsignarTU.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id asignacion TU";
                        }
                        else
                        {
                            _TicketVenta.IdAsignarTU = Seguridad.DesEncriptar(_TicketVenta.IdAsignarTU);
                            _TicketVenta.IdPersonaCliente = Seguridad.DesEncriptar(_TicketVenta.IdPersonaCliente);
                            _TicketVenta._TipoRubro.IdTipoRubro = Seguridad.DesEncriptar(_TicketVenta._TipoRubro.IdTipoRubro);
                            _TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro = Seguridad.DesEncriptar(_TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro);
                            TipoPresentacionRubro TipoPresentacion = new TipoPresentacionRubro();
                            TipoPresentacion = GestionTipoPresentacionRubro.ConsultarTipoPresentacionRubroPorId(int.Parse(_TicketVenta._TipoPresentacionRubro.IdTipoPresentacionRubro)).FirstOrDefault();
                            if (TipoPresentacion == null)
                            {
                                codigo = "418";
                                mensaje = "El tipo de promocion no existe";
                            }
                            else
                            {
                                if (TipoPresentacion.Identificador == 1)
                                {
                                    if (_TicketVenta._Vehiculo.Placa == null || string.IsNullOrEmpty(_TicketVenta._Vehiculo.Placa.Trim()))
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese la placa del vehiculo";
                                    }
                                    else if (_TicketVenta.IdPersonaChofer == null || string.IsNullOrEmpty(_TicketVenta.IdPersonaChofer.Trim()))
                                    {
                                        codigo = "418";
                                        mensaje = "Seleccione el chofer";
                                    }
                                    else if (_TicketVenta.PesoTara <= 0)
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese un peso tara correcto";
                                    }
                                    else
                                    {
                                        _TicketVenta.IdPersonaChofer = Seguridad.DesEncriptar(_TicketVenta.IdPersonaChofer);
                                        TicketVenta DatoTicketVenta = new TicketVenta();
                                        DatoTicketVenta = Seguridad.ConsultarTicketVentaRubroPorPlaca(_TicketVenta._Vehiculo.Placa).FirstOrDefault();
                                        if (DatoTicketVenta == null)
                                        {
                                            Ticket _DatoTicket = new Ticket();
                                            _DatoTicket = Seguridad.ConsultarTiketsPorPlaca(_TicketVenta._Vehiculo.Placa).FirstOrDefault();
                                            if (_DatoTicket == null)
                                            {
                                                DatoTicketVenta = new TicketVenta();
                                                DatoTicketVenta = GestionTicketVenta.InsertarTicketVentaRubroPorCarro(_TicketVenta);
                                                if (DatoTicketVenta.IdTicketVenta == null)
                                                {
                                                    codigo = "500";
                                                    mensaje = "Ocurrio un error al ingresar la venta";
                                                }
                                                else
                                                {
                                                    codigo = "200";
                                                    mensaje = "EXITO";
                                                    respuesta = DatoTicketVenta;
                                                    objeto = new { codigo, mensaje, respuesta };
                                                    return objeto;
                                                }
                                            }
                                            else
                                            {
                                                codigo = "418";
                                                mensaje = "El carro ya esta dentro de la instalacion";
                                            }
                                        }
                                        else
                                        {
                                            codigo = "418";
                                            mensaje = "El carro ya esta dentro de la instalacion";
                                        }
                                    }
                                }
                                else
                                {
                                    if (_TicketVenta.PesoNeto <= 0)
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese un peso neto correcto";
                                    }
                                    else if (_TicketVenta.PorcentajeImpureza < 0 || _TicketVenta.PorcentajeImpureza >100)
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese un porcentaje de impureza correcto";
                                    }
                                    else if (_TicketVenta.PorcentajeHumedad < 11 || _TicketVenta.PorcentajeHumedad > 100)
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese un porcentaje de humedad correcto";
                                    }
                                    else if (_TicketVenta.PrecioPorQuintal <= 0)
                                    {
                                        codigo = "418";
                                        mensaje = "Ingrese un precio por quintal correcto";
                                    }
                                    else
                                    {
                                        StockRubro _Stock = new StockRubro();
                                        _Stock = GestionTicketVenta.ConsultarStockRubroPorTipoRubro(int.Parse(_TicketVenta._TipoRubro.IdTipoRubro)).FirstOrDefault();
                                        if (_Stock == null)
                                        {
                                            codigo = "418";
                                            mensaje = "El tipo de rubro a vender no existe";
                                        }
                                        else
                                        {
                                            if (_TicketVenta.PesoNeto > _Stock.Stock)
                                            {
                                                codigo = "418";
                                                mensaje = "Solo existe en bodega "+ _Stock.Stock.ToString() + " quintales";
                                            }
                                            else
                                            {
                                                TicketVenta DatoTicketVenta = new TicketVenta();
                                                DatoTicketVenta = GestionTicketVenta.InsertarTicketVentaRubroPorSaco(_TicketVenta);
                                                if (DatoTicketVenta.IdTicketVenta == null)
                                                {
                                                    codigo = "500";
                                                    mensaje = "Ocurrio un error al ingresar la venta";
                                                }
                                                else
                                                {
                                                    codigo = "200";
                                                    mensaje = "EXITO";
                                                    respuesta = DatoTicketVenta;
                                                    objeto = new { codigo, mensaje, respuesta };
                                                    return objeto;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Rubros/ConsultarTicketVentaFinalizados")]
        public object ConsultarTicketVentaFinalizados(Tokens _Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Tokens.encriptada == null || string.IsNullOrEmpty(_Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        codigo = "200";
                        mensaje = "EXITO";
                        respuesta = GestionTicketVenta.ConsultarTicketVentaFinalizados();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Rubros/ConsultarTicketVentaSinFinalizar")]
        public object ConsultarTicketVentaSinFinalizar(Tokens _Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Tokens.encriptada == null || string.IsNullOrEmpty(_Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        codigo = "200";
                        mensaje = "EXITO";
                        respuesta = GestionTicketVenta.ConsultarTicketVentaSinFinalizar();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Rubros/EliminarVentaRubro")]
        public object EliminarVentaRubro(TicketVenta _TicketVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_TicketVenta.encriptada == null || string.IsNullOrEmpty(_TicketVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_TicketVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_TicketVenta.IdTicketVenta == null || string.IsNullOrEmpty(_TicketVenta.IdTicketVenta.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id ticket venta a eliminar";
                        }
                        else
                        {
                            _TicketVenta.IdTicketVenta = Seguridad.DesEncriptar(_TicketVenta.IdTicketVenta);
                            TicketVenta DatoTicketVenta = new TicketVenta();
                            DatoTicketVenta = GestionTicketVenta.ConsultarTicketVentaRubroPorId(int.Parse(_TicketVenta.IdTicketVenta)).FirstOrDefault();
                            if (DatoTicketVenta == null)
                            {
                                codigo = "418";
                                mensaje = "El ticket a eliminar no existe";
                            }
                            else if(DatoTicketVenta.Estado == false)
                            {
                                codigo = "418";
                                mensaje = "El ticket ya esta finalizado";
                            }
                            else
                            {
                                if (GestionTicketVenta.EliminarTicketVenta(int.Parse(_TicketVenta.IdTicketVenta)) == true)
                                {
                                    codigo = "200";
                                    mensaje = "EXITO";
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al eliminar el ticket";
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Rubros/FinalizarVentaRubro")]
        public object FinalizarVentaRubro(TicketVenta _TicketVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_TicketVenta.encriptada == null || string.IsNullOrEmpty(_TicketVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_TicketVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_TicketVenta.IdTicketVenta == null || string.IsNullOrEmpty(_TicketVenta.IdTicketVenta.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id ticket de la venta";
                        }
                        else if (_TicketVenta.PesoBruto <=0 || _TicketVenta.PesoBruto == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese el peso bruto correcto";
                        }
                        else if (_TicketVenta.PorcentajeImpureza < 0 || _TicketVenta.PorcentajeImpureza > 100)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un porcentaje de impureza correcto";
                        }
                        else if (_TicketVenta.PorcentajeHumedad < 11 || _TicketVenta.PorcentajeHumedad > 100)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un porcentaje de humedad correcto";
                        }
                        else if (_TicketVenta.PrecioPorQuintal <= 0)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un precio por quintal correcto";
                        }
                        else
                        {
                            _TicketVenta.IdTicketVenta = Seguridad.DesEncriptar(_TicketVenta.IdTicketVenta);
                            TicketVenta DatoTicketVenta = new TicketVenta();
                            DatoTicketVenta = GestionTicketVenta.ConsultarTicketVentaRubroPorId(int.Parse(_TicketVenta.IdTicketVenta)).FirstOrDefault();
                            if (DatoTicketVenta == null)
                            {
                                codigo = "418";
                                mensaje = "El ticket a eliminar no existe";
                            }
                            else if (DatoTicketVenta.Estado == false)
                            {
                                codigo = "418";
                                mensaje = "El ticket ya esta finalizado";
                            }
                            else
                            {
                                if (_TicketVenta.PesoBruto > DatoTicketVenta.PesoTara)
                                {
                                    StockRubro _Stock = new StockRubro();
                                    _Stock = GestionTicketVenta.ConsultarStockRubroPorTipoRubro(int.Parse(Seguridad.DesEncriptar(DatoTicketVenta.IdTipoRubro))).FirstOrDefault();
                                    if (_Stock == null)
                                    {
                                        codigo = "418";
                                        mensaje = "El tipo de rubro a vender no existe";
                                    }
                                    else
                                    {
                                        decimal? pesoNeto = 0;
                                        decimal? pesoEnQuintales = 0;
                                        pesoNeto = _TicketVenta.PesoBruto - DatoTicketVenta.PesoTara;
                                        pesoEnQuintales = pesoNeto * Convert.ToDecimal(0.022046);
                                        if (pesoEnQuintales> _Stock.Stock)
                                        {
                                            codigo = "418";
                                            mensaje = "Solo existe en bodega " + _Stock.Stock.ToString() + " quintales";
                                        }
                                        else
                                        {
                                            DatoTicketVenta = new TicketVenta();
                                            DatoTicketVenta = GestionTicketVenta.FinalizarTicketVentaRubro(_TicketVenta);
                                            if (DatoTicketVenta.IdTicketVenta == null)
                                            {
                                                codigo = "500";
                                                mensaje = "Ocurrio un error al finalizar la venta";
                                            }
                                            else
                                            {
                                                codigo = "200";
                                                mensaje = "EXITO";
                                                respuesta = DatoTicketVenta;
                                                objeto = new { codigo, mensaje, respuesta };
                                                return objeto;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    codigo = "418";
                                    mensaje = "El peso bruto no puede ser menor al peso tara";
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Rubros/AnularVentaRubro")]
        public object AnularVentaRubro(TicketVenta _TicketVenta)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_TicketVenta.encriptada == null || string.IsNullOrEmpty(_TicketVenta.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_TicketVenta.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_TicketVenta.IdTicketVenta == null || string.IsNullOrEmpty(_TicketVenta.IdTicketVenta.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id ticket de la venta";
                        }
                        else if (_TicketVenta.IdAsignarTU == null || string.IsNullOrEmpty(_TicketVenta.IdAsignarTU.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id asignacion tu";
                        }
                        else
                        {
                            _TicketVenta.IdTicketVenta = Seguridad.DesEncriptar(_TicketVenta.IdTicketVenta);
                            _TicketVenta.IdAsignarTU = Seguridad.DesEncriptar(_TicketVenta.IdAsignarTU);
                            TicketVenta DatoTicketVenta = new TicketVenta();
                            DatoTicketVenta = GestionTicketVenta.ConsultarTicketVentaRubroPorId(int.Parse(_TicketVenta.IdTicketVenta)).FirstOrDefault();
                            if (DatoTicketVenta == null)
                            {
                                codigo = "418";
                                mensaje = "El ticket a anular no existe";
                            }
                            else if (DatoTicketVenta.Estado == true)
                            {
                                codigo = "418";
                                mensaje = "El ticket todabia no esta finalizado";
                            }
                            else if (DatoTicketVenta.Anulada == true)
                            {
                                codigo = "418";
                                mensaje = "El ticket ya se encuentra anulado";
                            }
                            else
                            {
                                if (GestionTicketVenta.AnularTicketVenta(_TicketVenta) ==true)
                                {
                                    codigo = "200";
                                    mensaje = "EXITO";
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al anular la venta";
                                }
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Rubros/ConsultarTicketVentaAnulados")]
        public object ConsultarTicketVentaAnulados(Tokens _Tokens)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Tokens.encriptada == null || string.IsNullOrEmpty(_Tokens.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Tokens.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        codigo = "200";
                        mensaje = "EXITO";
                        respuesta = GestionTicketVenta.ConsultarTicketVentaAnulados();
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                }
                objeto = new { codigo, mensaje };
                return objeto;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
    }
}
