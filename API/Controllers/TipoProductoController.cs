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
    public class TipoProductoController : ApiController
    {
        CatalogoTipoProducto GestionTipoProducto = new CatalogoTipoProducto();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoTipoProducto")]
        public object IngresoTipoProducto(TipoProducto TipoProducto)
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
                string ClavePutEncripBD = p.desencriptar(TipoProducto.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                respuesta = GestionTipoProducto.IngresarTipoProducto(TipoProducto);
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
        [Route("api/Inventario/EliminarTipoProducto")]
        public object EliminarTipoProducto(TipoProducto TipoProducto)
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
                string ClavePutEncripBD = p.desencriptar(TipoProducto.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                TipoProducto.IdTipoProducto = Seguridad.DesEncriptar(TipoProducto.IdTipoProducto);

                respuesta = GestionTipoProducto.EliminarTipoProducto(int.Parse(TipoProducto.IdTipoProducto));
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
        [Route("api/Inventario/ActualizarTipoProducto")]
        public object ActualizarSembrio(TipoProducto TipoProducto)
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
                string ClavePutEncripBD = p.desencriptar(TipoProducto.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                TipoProducto.IdTipoProducto = Seguridad.DesEncriptar(TipoProducto.IdTipoProducto);
                respuesta = GestionTipoProducto.ModificarTipoProducto(TipoProducto);

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
        [Route("api/Inventario/ListaTipoProductos")]
        public object ListaTipoProductos([FromBody] Tokens Tokens)
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
                respuesta = GestionTipoProducto.ListarTipoProductos();
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
