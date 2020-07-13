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
                if (_AsignarComunidadFactura.encriptada == null || string.IsNullOrEmpty(_AsignarComunidadFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_AsignarComunidadFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
        [Route("api/Credito/EliminarAsignarComunidadFactura")]
        public object EliminarAsignarComunidadFactura(AsignarComunidadFactura _AsignarComunidadFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_AsignarComunidadFactura.encriptada == null || string.IsNullOrEmpty(_AsignarComunidadFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_AsignarComunidadFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_AsignarComunidadFactura.IdAsignarComunidadFactura == null || string.IsNullOrEmpty(_AsignarComunidadFactura.IdAsignarComunidadFactura))
                        {
                            mensaje = "Ingrese la comunidad a eliminar";
                            codigo = "418";
                        }
                        else
                        {
                            _AsignarComunidadFactura.IdAsignarComunidadFactura = Seguridad.DesEncriptar(_AsignarComunidadFactura.IdAsignarComunidadFactura);
                            bool ejecutado = _GestionAsignarComunidadConfigurarVenta.EliminarAsignarComunidadFactura(int.Parse(_AsignarComunidadFactura.IdAsignarComunidadFactura));
                            if (ejecutado == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                                respuesta = ejecutado;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                            else
                            {
                                mensaje = "Ocurrio un error al eliminar la comunidad";
                                codigo = "418";
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
        [Route("api/Credito/ListaAsignarComunidadFacturaPorFactura")]
        public object ListaAsignarComunidadFacturaPorFactura(AsignarComunidadFactura _AsignarComunidadFactura)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_AsignarComunidadFactura.encriptada == null || string.IsNullOrEmpty(_AsignarComunidadFactura.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_AsignarComunidadFactura.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                    }
                }
                objeto = new { codigo, mensaje};
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
        [Route("api/Credito/ConsultarPersonasEnFacturasParaSeguimientoPorCanton")]
        public object ConsultarPersonasEnFacturasParaSeguimientoPorCanton(Canton _Canton)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Canton.encriptada == null || string.IsNullOrEmpty(_Canton.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Canton.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
        [Route("api/Credito/ConsultarPersonasEnFacturasParaSeguimientoPorParroquia")]
        public object ConsultarPersonasEnFacturasParaSeguimientoPorParroquia(Parroquia _Parroquia)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Parroquia.encriptada == null || string.IsNullOrEmpty(_Parroquia.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Parroquia.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
                            respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasEnFacturasParaSeguimientoPorParroquia(int.Parse(_Parroquia.IdParroquia));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
        [Route("api/Credito/ConsultarPersonasParaSeguimientoPorComunidad")]
        public object ConsultarPersonasParaSeguimientoPorComunidad(Comunidad _Comunidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Comunidad.encriptada == null || string.IsNullOrEmpty(_Comunidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Comunidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
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
        [Route("api/Credito/ConsultarPersonasParaSeguimientoPorProvincia")]
        public object ConsultarPersonasParaSeguimientoPorProvincia(Provincia _Provincia)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Provincia.encriptada == null || string.IsNullOrEmpty(_Provincia.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Provincia.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Provincia.IdProvincia == null || string.IsNullOrEmpty(_Provincia.IdProvincia))
                        {
                            mensaje = "Ingrese la id provincia";
                            codigo = "418";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            _Provincia.IdProvincia = Seguridad.DesEncriptar(_Provincia.IdProvincia);
                            respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasParaSeguimientoPorProvincia(int.Parse(_Provincia.IdProvincia));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
        [Route("api/Credito/ConsultarPersonasPorTecnico")]
        public object ConsultarPersonasPorTecnico(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_AsignarTecnicoPersonaComunidad.encriptada == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_AsignarTecnicoPersonaComunidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico))
                        {
                            mensaje = "Ingrese la id del tecnico";
                            codigo = "418";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            _AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico = Seguridad.DesEncriptar(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico);
                            respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasAsignadasPorTecnico(int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
        [Route("api/Credito/ConsultarPersonasConComunidadesPorTecnico")]
        public object ConsultarPersonasConComunidadesPorTecnico(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_AsignarTecnicoPersonaComunidad.encriptada == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_AsignarTecnicoPersonaComunidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico))
                        {
                            mensaje = "Ingrese la id del tecnico";
                            codigo = "418";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            _AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico = Seguridad.DesEncriptar(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico);
                            respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasAsignadasPorTecnicoConSuscomunidades(int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
        [Route("api/Credito/ConsultarVisitasFinalizadasPorTecnico")]
        public object ConsultarVisitasFinalizadasPorTecnico(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_AsignarTecnicoPersonaComunidad.encriptada == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_AsignarTecnicoPersonaComunidad.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico))
                        {
                            mensaje = "Ingrese la id del tecnico";
                            codigo = "418";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            _AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico = Seguridad.DesEncriptar(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico);
                            respuesta = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasSeguimientoFinalizadoPorTecnico(int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
    }
}
