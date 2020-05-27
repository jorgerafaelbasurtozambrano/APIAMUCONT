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
    public class AsignarComunidadFacturaController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoAsignarComunidadFactura _GestionAsignarComunidadConfigurarVenta = new CatalogoAsignarComunidadFactura();
        [HttpPost]
        [Route("api/Credito/IngresoAsignarComunidadFactura")]
        public object IngresoAbono(AsignarComunidadFactura _AsignarComunidadFactura)
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
                string ClavePostEncripBD = p.desencriptar(_AsignarComunidadFactura.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{

                if (_AsignarComunidadFactura == null)
                {
                    mensaje = "Error el objeto que envio esta null";
                    codigo = "418";
                }
                else if (_AsignarComunidadFactura.IdComunidad == null || string.IsNullOrEmpty(_AsignarComunidadFactura.IdComunidad))
                {
                    mensaje = "Ingrese el Asignar TU";
                    codigo = "418";
                }
                else if (_AsignarComunidadFactura.IdCabeceraFactura == null || string.IsNullOrEmpty(_AsignarComunidadFactura.IdCabeceraFactura))
                {
                    mensaje = "Ingrese el configurar Venta";
                    codigo = "418";
                }
                else
                {
                    _AsignarComunidadFactura.IdComunidad = Seguridad.DesEncriptar(_AsignarComunidadFactura.IdComunidad);
                    _AsignarComunidadFactura.IdCabeceraFactura = Seguridad.DesEncriptar(_AsignarComunidadFactura.IdCabeceraFactura);
                    AsignarComunidadFactura _DatoEncontrado = new AsignarComunidadFactura();
                    _DatoEncontrado = _GestionAsignarComunidadConfigurarVenta.ConsultarAsignarComunidadFacturaPorFacturaYPorComunidad(int.Parse(_AsignarComunidadFactura.IdCabeceraFactura), int.Parse(_AsignarComunidadFactura.IdComunidad)).FirstOrDefault();
                    if (_DatoEncontrado == null)
                    {
                        AsignarComunidadFactura _Dato = new AsignarComunidadFactura();
                        _Dato = _GestionAsignarComunidadConfigurarVenta.InsertarComunidadConfigurarVenta(_AsignarComunidadFactura);
                        if (_Dato.IdAsignarComunidadFactura == null)
                        {
                            mensaje = "Error al ingresar el registro";
                            codigo = "418";
                        }
                        else
                        {
                            respuesta = _Dato;
                            mensaje = "EXITO";
                            codigo = "200";
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        mensaje = "Error ya existe la comunidad que intenta ingresar";
                        codigo = "418";
                    }
                }
                objeto = new { codigo, mensaje };
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


        [HttpPost]
        [Route("api/Credito/EliminarAsignarComunidadFactura")]
        public object EliminarAsignarComunidadFactura(AsignarComunidadFactura _AsignarComunidadFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _claveDelete = ListaClaves.Where(c => c.Identificador == 3).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(_AsignarComunidadFactura.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (_AsignarComunidadFactura.IdAsignarComunidadFactura == null || string.IsNullOrEmpty(_AsignarComunidadFactura.IdAsignarComunidadFactura))
                {
                    mensaje = "Ingrese la comunidad a eliminar";
                    codigo = "418";
                }
                else
                {
                    _AsignarComunidadFactura.IdAsignarComunidadFactura = Seguridad.DesEncriptar(_AsignarComunidadFactura.IdAsignarComunidadFactura);
                    bool ejecutado = _GestionAsignarComunidadConfigurarVenta.EliminarAsignarComunidadFactura(int.Parse(_AsignarComunidadFactura.IdAsignarComunidadFactura));
                    if (ejecutado==true)
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = ejecutado;
                        objeto = new { codigo, mensaje,respuesta };
                        return objeto;
                    }
                    else
                    {
                        mensaje = "Ocurrio un error al eliminar la comunidad";
                        codigo = "418";
                    }
                }
                objeto = new { codigo, mensaje };
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

        [HttpPost]
        [Route("api/Credito/ListaAsignarComunidadFacturaPorFactura")]
        public object ListaAsignarComunidadFacturaPorFactura(AsignarComunidadFactura _AsignarComunidadFactura)
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
                string ClaveGetEncripBD = p.desencriptar(_AsignarComunidadFactura.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_AsignarComunidadFactura.IdCabeceraFactura == null || string.IsNullOrEmpty(_AsignarComunidadFactura.IdCabeceraFactura))
                {
                    mensaje = "Ingrese la id de la factura";
                    codigo = "418";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    _AsignarComunidadFactura.IdCabeceraFactura = Seguridad.DesEncriptar(_AsignarComunidadFactura.IdCabeceraFactura);
                    respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarAsignarComunidadFactura(int.Parse(_AsignarComunidadFactura.IdCabeceraFactura));
                    objeto = new { codigo, mensaje, respuesta };
                    return objeto;
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new { codigo, mensaje};
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

        [HttpPost]
        [Route("api/Credito/ConsultarPersonasEnFacturasParaSeguimientoPorCanton")]
        public object ConsultarPersonasEnFacturasParaSeguimientoPorCanton(Canton _Canton)
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
                string ClaveGetEncripBD = p.desencriptar(_Canton.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_Canton.IdCanton == null || string.IsNullOrEmpty(_Canton.IdCanton))
                {
                    mensaje = "Ingrese la id canton";
                    codigo = "418";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    _Canton.IdCanton = Seguridad.DesEncriptar(_Canton.IdCanton);
                    respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasEnFacturasParaSeguimientoPorCanton(int.Parse(_Canton.IdCanton));
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
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/Credito/ConsultarPersonasEnFacturasParaSeguimientoPorParroquia")]
        public object ConsultarPersonasEnFacturasParaSeguimientoPorParroquia(Parroquia _Parroquia)
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
                string ClaveGetEncripBD = p.desencriptar(_Parroquia.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_Parroquia.IdParroquia == null || string.IsNullOrEmpty(_Parroquia.IdParroquia))
                {
                    mensaje = "Ingrese la id parroquia";
                    codigo = "418";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    _Parroquia.IdParroquia = Seguridad.DesEncriptar(_Parroquia.IdParroquia);
                    respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasEnFacturasParaSeguimientoPorCanton(int.Parse(_Parroquia.IdParroquia));
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
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/Credito/ConsultarPersonasParaSeguimientoPorComunidad")]
        public object ConsultarPersonasParaSeguimientoPorComunidad(Comunidad _Comunidad)
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
                string ClaveGetEncripBD = p.desencriptar(_Comunidad.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_Comunidad.IdComunidad == null || string.IsNullOrEmpty(_Comunidad.IdComunidad))
                {
                    mensaje = "Ingrese la id parroquia";
                    codigo = "418";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    _Comunidad.IdComunidad = Seguridad.DesEncriptar(_Comunidad.IdComunidad);
                    respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasParaSeguimientoPorComunidad(int.Parse(_Comunidad.IdComunidad));
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
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

    }
}
