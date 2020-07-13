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
    public class TicketController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoTicket GestionTicket = new CatalogoTicket();
        [HttpPost]
        [Route("api/Rubros/IngresarTicket")]
        public object IngresarTicket(Ticket _Ticket)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Ticket.encriptada == null || string.IsNullOrEmpty(_Ticket.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Ticket.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Ticket._TipoPresentacionRubro == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese el tipo presentacion rubro";
                        }
                        else if (_Ticket._TipoPresentacionRubro.IdTipoPresentacionRubro == null || string.IsNullOrEmpty(_Ticket._TipoPresentacionRubro.IdTipoPresentacionRubro.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tipo presentacion del rubro";
                        }
                        else if (_Ticket._TipoPresentacionRubro.Identificador == 0)
                        {
                            codigo = "418";
                            mensaje = "Ingrese el identificador del tipo presentacion del rubro";
                        }
                        if (_Ticket._TipoRubro == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese el tipo de rubro";
                        }
                        else if (_Ticket._TipoRubro.IdTipoRubro == null || string.IsNullOrEmpty(_Ticket._TipoRubro.IdTipoRubro.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tipo rubro";
                        }
                        else if (_Ticket.IdPersona == null || string.IsNullOrEmpty(_Ticket.IdPersona.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id de la persona";
                        }
                        else if (_Ticket.IdAsignarTU == null || string.IsNullOrEmpty(_Ticket.IdAsignarTU.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id asignacion TU";
                        }
                        else
                        {
                            _Ticket.IdAsignarTU = Seguridad.DesEncriptar(_Ticket.IdAsignarTU);
                            _Ticket.IdPersona = Seguridad.DesEncriptar(_Ticket.IdPersona);
                            _Ticket._TipoRubro.IdTipoRubro = Seguridad.DesEncriptar(_Ticket._TipoRubro.IdTipoRubro);
                            _Ticket._TipoPresentacionRubro.IdTipoPresentacionRubro = Seguridad.DesEncriptar(_Ticket._TipoPresentacionRubro.IdTipoPresentacionRubro);
                            if (_Ticket._TipoPresentacionRubro.Identificador == 1)
                            {
                                if (_Ticket._Vehiculo == null)
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese el vehiculo";
                                }
                                else if (_Ticket._Vehiculo.Placa == null || string.IsNullOrEmpty(_Ticket._Vehiculo.Placa.Trim()))
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese la placa del vehiculo";
                                }
                                else if (_Ticket.PesoBruto == null)
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese el peso bruto";
                                }
                                else if (_Ticket.PesoBruto <= 0)
                                {
                                    codigo = "418";
                                    mensaje = "El peso bruto no puede ser menor o igual a cero";
                                }
                                else
                                {
                                    Ticket _DatoTicket = new Ticket();
                                    _DatoTicket = Seguridad.ConsultarTiketsPorPlaca(_Ticket._Vehiculo.Placa).FirstOrDefault();
                                    if (_DatoTicket == null)
                                    {
                                        TicketVenta DataVenta = new TicketVenta();
                                        DataVenta = Seguridad.ConsultarTicketVentaRubroPorPlaca(_Ticket._Vehiculo.Placa).FirstOrDefault();
                                        if (DataVenta == null)
                                        {
                                            _DatoTicket = GestionTicket.InsertarTiketPorCarro(_Ticket);
                                            if (_DatoTicket.IdTicket == null)
                                            {
                                                codigo = "500";
                                                mensaje = "Ocurrio un error al ingresar el ticket";
                                            }
                                            else
                                            {
                                                codigo = "200";
                                                mensaje = "EXITO";
                                                respuesta = _DatoTicket;
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
                                if (_Ticket.PesoNeto == null)
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese el peso neto";
                                }
                                else if (_Ticket.PesoNeto <= 0)
                                {
                                    codigo = "418";
                                    mensaje = "El peso bruto no puede ser menor o igual a cero";
                                }
                                else if (_Ticket.PorcentajeHumedad <= 0 || _Ticket.PorcentajeHumedad > 100 || _Ticket.PorcentajeHumedad == null)
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese un valor correcto del porcentaje de humedad ";
                                }
                                else if (_Ticket.PrecioPorQuintal <= 0 || _Ticket.PrecioPorQuintal == null)
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese un valor de precio por quintal correcto";
                                }
                                else if (_Ticket.PorcentajeImpureza <= 0 || _Ticket.PorcentajeImpureza > 100 || _Ticket.PorcentajeImpureza == null)
                                {
                                    codigo = "418";
                                    mensaje = "Ingrese un valor de porcentaje de impureza correcta";
                                }
                                else
                                {
                                    Ticket _DatoTicket = new Ticket();
                                    _DatoTicket = GestionTicket.InsertarTicketPorSaco(_Ticket);
                                    if (_DatoTicket.IdTicket == null)
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al ingresar el ticket";
                                    }
                                    else
                                    {
                                        codigo = "200";
                                        mensaje = "EXITO";
                                        respuesta = _DatoTicket;
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
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
        [Route("api/Rubros/ConsultarTicket")]
        public object ConsultarTicket(Tokens _Tokens)
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
                        respuesta = GestionTicket.ConsultarTikets();
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
        [Route("api/Rubros/EliminarTicket")]
        public object EliminarTicket(Ticket _Ticket)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Ticket.encriptada == null || string.IsNullOrEmpty(_Ticket.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Ticket.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Ticket.IdTicket == null || string.IsNullOrEmpty(_Ticket.IdTicket.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id ticket a eliminar";
                        }
                        else
                        {
                            _Ticket.IdTicket = Seguridad.DesEncriptar(_Ticket.IdTicket);
                            Ticket _DatoTicket = new Ticket();
                            _DatoTicket = GestionTicket.ConsultarTiketsPorId(int.Parse(_Ticket.IdTicket)).FirstOrDefault();
                            if (_DatoTicket == null)
                            {
                                codigo = "418";
                                mensaje = "El ticket que desea eliminar no existe";
                            }
                            else
                            {
                                if (GestionTicket.EliminarTicket(int.Parse(_Ticket.IdTicket)) == true)
                                {
                                    codigo = "200";
                                    mensaje = "EXITO";
                                }
                                else
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al tratar de eliminar el ticket";
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
        [Route("api/Rubros/FinalizarTicket")]
        public object FinalizarTicket(Ticket _Ticket)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Ticket.encriptada == null || string.IsNullOrEmpty(_Ticket.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Ticket.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Ticket.IdTicket == null || string.IsNullOrEmpty(_Ticket.IdTicket.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tiket a finalizar";
                        }
                        else if (_Ticket.PesoTara <= 0  || _Ticket.PesoTara ==null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un valor correcto de peso tara ";
                        }
                        else if (_Ticket.PorcentajeHumedad <= 0 || _Ticket.PorcentajeHumedad > 100 || _Ticket.PorcentajeHumedad == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un valor correcto del porcentaje de humedad";
                        }
                        else if (_Ticket.PrecioPorQuintal <= 0 || _Ticket.PrecioPorQuintal == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un valor de precio por quintal correcto";
                        }
                        else if (_Ticket.PorcentajeImpureza <= 0 || _Ticket.PorcentajeImpureza > 100 || _Ticket.PorcentajeImpureza == null)
                        {
                            codigo = "418";
                            mensaje = "Ingrese un valor de porcentaje de impureza correcta";
                        }
                        else
                        {
                            _Ticket.IdTicket = Seguridad.DesEncriptar(_Ticket.IdTicket);
                            Ticket _DatoTicket = new Ticket();
                            _DatoTicket = GestionTicket.ConsultarTiketsPorId(int.Parse(_Ticket.IdTicket)).FirstOrDefault();
                            if (_DatoTicket == null)
                            {
                                codigo = "418";
                                mensaje = "El ticket a finalizar no existe";
                            }
                            else
                            {
                                if (_DatoTicket.Estado == false)
                                {
                                    codigo = "418";
                                    mensaje = "El ticket ya se encuentra finalizado";
                                }
                                else
                                {
                                    if (_Ticket.PesoTara >= _DatoTicket.PesoBruto)
                                    {
                                        codigo = "418";
                                        mensaje = "El peso tara no puede ser mayor al peso bruto";
                                    }
                                    else
                                    {
                                        _DatoTicket = new Ticket();
                                        _DatoTicket = GestionTicket.FinalizarTicket(_Ticket);
                                        if (_DatoTicket.IdTicket == null)
                                        {
                                            codigo = "500";
                                            mensaje = "Ocurrio un error al tratar de finalizar el ticket";
                                        }
                                        else
                                        {
                                            codigo = "200";
                                            mensaje = "EXITO";
                                            respuesta = _DatoTicket;
                                            objeto = new { codigo, mensaje, respuesta };
                                            return objeto;
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
        [Route("api/Rubros/ConsultarStockRubro")]
        public object ConsultarStockRubro(Tokens _Tokens)
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
                        respuesta = GestionTicket.ConsultarStockRubro();
                        objeto = new { codigo, mensaje ,respuesta };
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
        [Route("api/Rubros/ConsultarTicketFinalizados")]
        public object ConsultarTicketFinalizados(Tokens _Tokens)
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
                        respuesta = GestionTicket.ConsultarTiketsFinalizados();
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
        [Route("api/Rubros/AnularTicket")]
        public object AnularTicket(Ticket _Ticket)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Ticket.encriptada == null || string.IsNullOrEmpty(_Ticket.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Ticket.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Ticket.IdTicket == null || string.IsNullOrEmpty(_Ticket.IdTicket.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id tiket a finalizar";
                        }
                        else if (_Ticket.IdAsignarTU == null || string.IsNullOrEmpty(_Ticket.IdAsignarTU.Trim()))
                        {
                            codigo = "418";
                            mensaje = "Ingrese el id asignartu";
                        }
                        else
                        {
                            _Ticket.IdTicket = Seguridad.DesEncriptar(_Ticket.IdTicket);
                            _Ticket.IdAsignarTU = Seguridad.DesEncriptar(_Ticket.IdAsignarTU);
                            Ticket _DatoTicket = new Ticket();
                            _DatoTicket = GestionTicket.ConsultarTiketsPorId(int.Parse(_Ticket.IdTicket)).FirstOrDefault();
                            if (_DatoTicket == null)
                            {
                                codigo = "418";
                                mensaje = "El ticket para anular no existe";
                            }
                            else
                            {
                                if (_DatoTicket.Estado == false)
                                {
                                    if (_DatoTicket.Anulada == true)
                                    {
                                        codigo = "418";
                                        mensaje = "El ticket ya se encuentra anulado";
                                    }
                                    else
                                    {
                                        Ticket _TicketAnular = new Ticket();
                                        _TicketAnular = GestionTicket.AnularTicket(_Ticket);
                                        if (_TicketAnular.IdTicket == null)
                                        {
                                            codigo = "500";
                                            mensaje = "Ocurrio un error al tratar de anular";
                                        }
                                        else
                                        {
                                            codigo = "200";
                                            mensaje = "EXITO";
                                        }
                                    }
                                }
                                else
                                {
                                    codigo = "418";
                                    mensaje = "El ticket no se encuentra finalizado";
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
        [Route("api/Rubros/ConsultarTicketAnulados")]
        public object ConsultarTicketAnulados(Tokens _Tokens)
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
                        respuesta = GestionTicket.ConsultarTiketsAnulados();
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
