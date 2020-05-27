using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio.Logica.Credito;
using Negocio;

namespace API.Controllers
{
    public class AsignarSeguroController : ApiController
    {
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoAsignarSeguro _CatalogoAsignarSeguro = new CatalogoAsignarSeguro();
        CatalogoAsignarComunidadFactura _CatalogoAsignarComunidadFactura = new CatalogoAsignarComunidadFactura();

        [HttpPost]
        [Route("api/Credito/IngresoAsignarSeguro")]
        public object IngresoAsignarSeguro(AsignarSeguro AsignarSeguro)
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
                string ClavePutEncripBD = p.desencriptar(AsignarSeguro.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (AsignarSeguro.IdAsignarTUResp == null || string.IsNullOrEmpty(AsignarSeguro.IdAsignarTUResp))
                {
                    mensaje = "Ingrese el responsable";
                    codigo = "418";
                }
                else if (AsignarSeguro.IdAsignarTUTecn == null || string.IsNullOrEmpty(AsignarSeguro.IdAsignarTUTecn))
                {
                    mensaje = "Ingrese el tecnico";
                    codigo = "418";
                }
                else if(AsignarSeguro.IdConfigurarVenta == null || string.IsNullOrEmpty(AsignarSeguro.IdConfigurarVenta))
                {
                    mensaje = "Ingrese el configurar venta";
                    codigo = "418";
                }
                else
                {
                    AsignarSeguro _DataAsignarSeguro = new AsignarSeguro();
                    AsignarSeguro.IdAsignarTUResp = Seguridad.DesEncriptar(AsignarSeguro.IdAsignarTUResp);
                    AsignarSeguro.IdAsignarTUTecn = Seguridad.DesEncriptar(AsignarSeguro.IdAsignarTUTecn);
                    AsignarSeguro.IdConfigurarVenta = Seguridad.DesEncriptar(AsignarSeguro.IdConfigurarVenta);
                    _DataAsignarSeguro = _CatalogoAsignarSeguro.ConsultarAsignarSeguroPorConfigurarVenta(int.Parse(AsignarSeguro.IdConfigurarVenta)).FirstOrDefault();
                    if (_DataAsignarSeguro == null)
                    {
                        respuesta = _CatalogoAsignarSeguro.IngresoAsignarSeguro(AsignarSeguro);
                        mensaje = "EXITO";
                        codigo = "200";
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                    else
                    {
                        mensaje = "Ya existe asignar seguro en esta factura";
                        codigo = "418";
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
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/Credito/ListaPersonasParaSeguimiento")]
        public object ListaPersonasParaSeguimiento([FromBody] Tokens Tokens)
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
                string ClaveGetEncripBD = p.desencriptar(Tokens.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                respuesta = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimiento();
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje, respuesta };
                return objeto;
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
