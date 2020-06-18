using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Credito;
using Negocio.Logica.Seguridad;
namespace API.Controllers
{
    public class AbonoController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CaatalogoAbono GestionAbono = new CaatalogoAbono();
        CatalogoConfigurarVenta GestionConfigurarVenta = new CatalogoConfigurarVenta();
        [HttpPost]
        [Route("api/Credito/IngresoAbono")]
        public object IngresoAbono(Abono _Abono)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePost = ListaClaves.Where(c => c.Identificador == 1).FirstOrDefault();
                Object resultado = new object();
                string ClavePostEncripBD = p.desencriptar(_Abono.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{

                if (_Abono == null)
                {
                    mensaje = "Error el objeto que envio esta null";
                    codigo = "418";
                }
                else if (_Abono.IdAsignarTU == null || string.IsNullOrEmpty(_Abono.IdAsignarTU))
                {
                    mensaje = "Ingrese el Asignar TU";
                    codigo = "418";
                }
                else if (_Abono.IdConfigurarVenta == null || string.IsNullOrEmpty(_Abono.IdConfigurarVenta))
                {
                    mensaje = "Ingrese el configurar Venta";
                    codigo = "418";
                }
                else if (_Abono.Monto<=0)
                {
                    mensaje = "No se puede añadir un valor menor o igual a cero";
                    codigo = "418";
                }
                else
                {
                    _Abono.IdAsignarTU = Seguridad.DesEncriptar(_Abono.IdAsignarTU);
                    _Abono.IdConfigurarVenta = Seguridad.DesEncriptar(_Abono.IdConfigurarVenta);
                    ConfigurarVenta _ConfigurarVenta = new ConfigurarVenta();
                    _ConfigurarVenta = GestionConfigurarVenta.ConsultarConfigurarVentaPorId(int.Parse(_Abono.IdConfigurarVenta));
                    if (_ConfigurarVenta.EstadoConfVenta == "1")
                    {
                        mensaje = "No se puede abonar a esta factura porque ya esta cancelada";
                        codigo = "418";
                    }
                    else
                    {
                        if (_Abono.Monto > _ConfigurarVenta._SaldoPendiente.Pendiente)
                        {
                            mensaje = "El monto que desea abonar supera la deuda que tiene";
                            codigo = "418";
                        }
                        else
                        {
                            Abono _DatoAbono = new Abono();
                            _DatoAbono = GestionAbono.InsertarAbono(_Abono);
                            if (_DatoAbono.IdAbono == null)
                            {
                                mensaje = "Ocurrio un error al tratar de ingresar el abono";
                                codigo = "500";
                            }
                            else
                            {
                                respuesta = _DatoAbono;
                                mensaje = "EXITO";
                                if (_DatoAbono._ConfigurarVenta._SaldoPendiente.Pendiente == 0)
                                {
                                    codigo = "200";
                                }
                                else
                                {
                                    codigo = "201";
                                }
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                }
                objeto = new { codigo, mensaje};
                return objeto;
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
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
        [Route("api/Credito/ConsutlarAbonoPorFactura")]
        public object ConsutlarAbonoPorFactura(Abono _Abono)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveGet = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                Object resultado = new object();
                string ClaveGetEncripBD = p.desencriptar(_Abono.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_Abono.IdConfigurarVenta == null || string.IsNullOrEmpty(_Abono.IdConfigurarVenta.Trim()))
                {
                    mensaje = "Ingrese la id asignartecnicopersonacomunidad";
                    codigo = "418";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    _Abono.IdConfigurarVenta = Seguridad.DesEncriptar(_Abono.IdConfigurarVenta);
                    respuesta = GestionAbono.ConsultarAbonoPorFactura(int.Parse(_Abono.IdConfigurarVenta));
                    objeto = new { codigo, mensaje, respuesta };
                    return objeto;
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
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
