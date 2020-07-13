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
    public class TipoRubroController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoTipoRubro GestionTipoRubro = new CatalogoTipoRubro();
        [HttpPost]
        [Route("api/Rubros/ConsultarTipoRubro")]
        public object ConsultarTipoRubro(Tokens _Tokens)
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
                        respuesta = GestionTipoRubro.ConsultarTipoRubro();
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
