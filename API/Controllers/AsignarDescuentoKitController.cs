using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades;
using Negocio.Logica.Inventario;
using Negocio;
using Negocio.Logica.Seguridad;
namespace API.Controllers
{
    public class AsignarDescuentoKitController : ApiController
    {
        CatalogoAsignarDescuentoKit GestionAsignarDescuentoKit = new CatalogoAsignarDescuentoKit();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoAsignarDescuentoKit")]
        public object IngresoAsignarDescuentoKit(AsignarDescuentoKit AsignarDescuentoKit)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (AsignarDescuentoKit.encriptada == null || string.IsNullOrEmpty(AsignarDescuentoKit.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(AsignarDescuentoKit.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        AsignarDescuentoKit.IdDescuento = Seguridad.DesEncriptar(AsignarDescuentoKit.IdDescuento);
                        AsignarDescuentoKit.IdKit = Seguridad.DesEncriptar(AsignarDescuentoKit.IdKit);
                        respuesta = GestionAsignarDescuentoKit.InsertarAsignarDescuentoKit(AsignarDescuentoKit);
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
