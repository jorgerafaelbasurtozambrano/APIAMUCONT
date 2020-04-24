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
    public class AsignarProductoKitController : ApiController
    {
        CatalogoAsignarProductoKit GestionAsignarProductoKit = new CatalogoAsignarProductoKit();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoAsignarProductoKit")]
        public object IngresoAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
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
                string ClavePutEncripBD = p.desencriptar(AsignarProductoKit.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                AsignarProductoKit.IdConfigurarProducto = Seguridad.DesEncriptar(AsignarProductoKit.IdConfigurarProducto);
                AsignarProductoKit.IdAsignarDescuentoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarDescuentoKit);
                respuesta = GestionAsignarProductoKit.InsertarAsignarProductoKit(AsignarProductoKit);
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

        [HttpPost]
        [Route("api/Inventario/EliminarAsignarProductoKit")]
        public object EliminarAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
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
                string ClavePutEncripBD = p.desencriptar(AsignarProductoKit.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                AsignarProductoKit.IdAsignarProductoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarProductoKit);
                respuesta = GestionAsignarProductoKit.EliminarAsignarProductoKit(int.Parse(AsignarProductoKit.IdAsignarProductoKit));
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

        [HttpPost]
        [Route("api/Inventario/ActualizarAsignarProductoKit")]
        public object ActualizarAsignarProductoKit(AsignarProductoKit AsignarProductoKit)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                var ListaClaves = GestionSeguridad.ListarTokens().Where(c => c.Estado == true).ToList();
                var _clavePut = ListaClaves.Where(c => c.Identificador == 2).FirstOrDefault();
                Object resultado = new object();
                string ClavePutEncripBD = p.desencriptar(AsignarProductoKit.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                AsignarProductoKit.IdAsignarProductoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarProductoKit);
                AsignarProductoKit.IdConfigurarProducto = Seguridad.DesEncriptar(AsignarProductoKit.IdConfigurarProducto);
                AsignarProductoKit.IdAsignarDescuentoKit = Seguridad.DesEncriptar(AsignarProductoKit.IdAsignarDescuentoKit);
                respuesta = GestionAsignarProductoKit.ModificarAsignarProductoKit(AsignarProductoKit);

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

        [HttpPost]
        [Route("api/Inventario/ListaAsignarProductoKit")]
        public object ListaAsignarProductoKit(Kit Kit)
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
                string ClaveGetEncripBD = p.desencriptar(Kit.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                Kit.IdKit = Seguridad.DesEncriptar(Kit.IdKit);
                //respuesta = GestionAsignarProductoKit.ListarAsignarProductosKit();
                respuesta = GestionAsignarProductoKit.ListarProductosDeUnKit(int.Parse(Kit.IdKit));
                //}
                //else
                //{
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
