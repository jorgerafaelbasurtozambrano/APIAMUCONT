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
    public class AsignarTecnicoPersonaComunidadController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoAsignarTecnicoPersonaComunidad _CatalogoAsignarTPC = new CatalogoAsignarTecnicoPersonaComunidad();
        [HttpPost]
        [Route("api/Credito/IngresoAsignarTecnicoPersonaComunidad")]
        public object IngresoAsignarTecnicoPersonaComunidad(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
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
                string ClavePostEncripBD = p.desencriptar(_AsignarTecnicoPersonaComunidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePostEncripBD == _clavePost.Descripcion)
                //{
                if (_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico))
                {
                    mensaje = "Ingrese el id del tecnico";
                    codigo = "418";
                }
                else if (_AsignarTecnicoPersonaComunidad.IdPersona == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.IdPersona))
                {
                    mensaje = "Ingrese el id de la persona";
                    codigo = "418";
                }
                else
                {
                    _AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico = Seguridad.DesEncriptar(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico);
                    _AsignarTecnicoPersonaComunidad.IdPersona = Seguridad.DesEncriptar(_AsignarTecnicoPersonaComunidad.IdPersona);
                    TipoUsuario _DataTipoUsuario = new TipoUsuario();
                    _DataTipoUsuario = _CatalogoAsignarTPC.ConsultarTipoUsuarioTecncioDeUnaPersona(int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico)).FirstOrDefault();
                    if (_DataTipoUsuario == null)
                    {
                        mensaje = "El Usuario no es tecnico";
                        codigo = "418";
                        objeto = new { codigo, mensaje };
                        return objeto;
                    }
                    else
                    {
                        bool ejecutado = _CatalogoAsignarTPC.ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura1(_AsignarTecnicoPersonaComunidad);
                        if (ejecutado == true)
                        {
                            respuesta = ejecutado;
                            mensaje = "EXITO";
                            codigo = "200";
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                        else
                        {
                            mensaje = "ERROR";
                            codigo = "418";
                            objeto = new { codigo, mensaje };
                            return objeto;
                        }
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
        [Route("api/Credito/EliminarAsignarTecnicoPersonaComunidad")]
        public object EliminarAsignarTecnicoPersonaComunidad(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
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
                string ClavePutEncripBD = p.desencriptar(_AsignarTecnicoPersonaComunidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (_AsignarTecnicoPersonaComunidad.IdPersona == null || string.IsNullOrEmpty(_AsignarTecnicoPersonaComunidad.IdPersona))
                {
                    mensaje = "Ingrese el id de la persona";
                    codigo = "418";
                }
                else
                {
                    _AsignarTecnicoPersonaComunidad.IdPersona = Seguridad.DesEncriptar(_AsignarTecnicoPersonaComunidad.IdPersona);
                    bool ejecutado = _CatalogoAsignarTPC.EliminarPersonaDeUnTecnico(int.Parse(_AsignarTecnicoPersonaComunidad.IdPersona));
                    if (ejecutado == true)
                    {
                        respuesta = ejecutado;
                        mensaje = "EXITO";
                        codigo = "200";
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
                    else
                    {
                        mensaje = "ERROR";
                        codigo = "418";
                        objeto = new { codigo, mensaje };
                        return objeto;
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
    }
}
