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
                    respuesta = GestionAbono.InsertarAbono(_Abono);
                    mensaje = "EXITO";
                    codigo = "200";
                    objeto = new { codigo, mensaje, respuesta };
                    return objeto;
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
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
    }
}
