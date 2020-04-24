using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Inventario;
using Negocio;
using Negocio.Logica.Seguridad;
using Negocio.Entidades;

namespace API.Controllers
{
    public class ConfigurarProductoController : ApiController
    {
        CatalogoConfigurarProducto GestionConfigurarProducto = new CatalogoConfigurarProducto();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoConfigurarProducto")]
        public object IngresoConfigurarProducto(ConfigurarProducto ConfigurarProducto)
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
                string ClavePutEncripBD = p.desencriptar(ConfigurarProducto.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                ConfigurarProducto.IdAsignacionTu = Seguridad.DesEncriptar(ConfigurarProducto.IdAsignacionTu);
                ConfigurarProducto.IdMedida = Seguridad.DesEncriptar(ConfigurarProducto.IdMedida);
                ConfigurarProducto.IdPresentacion = Seguridad.DesEncriptar(ConfigurarProducto.IdPresentacion);
                ConfigurarProducto.IdProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdProducto);
                respuesta = GestionConfigurarProducto.InsertarConfigurarProducto(ConfigurarProducto);
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
        [Route("api/Inventario/EliminarConfigurarProducto")]
        public object EliminarConfigurarProducto(ConfigurarProducto ConfigurarProducto)
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
                string ClavePutEncripBD = p.desencriptar(ConfigurarProducto.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                ConfigurarProducto.IdConfigurarProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdConfigurarProducto);
                ConfigurarProducto.IdProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdProducto);
                respuesta = GestionConfigurarProducto.EliminarConfigurarProducto(int.Parse(ConfigurarProducto.IdConfigurarProducto),int.Parse(ConfigurarProducto.IdProducto));
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
        [Route("api/Inventario/ActualizarConfigurarProducto")]
        public object ActualizarConfigurarProducto(ConfigurarProducto ConfigurarProducto)
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
                string ClavePutEncripBD = p.desencriptar(ConfigurarProducto.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                ConfigurarProducto.IdAsignacionTu = Seguridad.DesEncriptar(ConfigurarProducto.IdAsignacionTu);
                ConfigurarProducto.IdMedida = Seguridad.DesEncriptar(ConfigurarProducto.IdMedida);
                ConfigurarProducto.IdPresentacion = Seguridad.DesEncriptar(ConfigurarProducto.IdPresentacion);
                ConfigurarProducto.IdProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdProducto);
                ConfigurarProducto.IdConfigurarProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdConfigurarProducto);
                respuesta = GestionConfigurarProducto.ModificarConfigurarProducto(ConfigurarProducto);
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
        [Route("api/Inventario/ListaConfigurarProductos")]
        public object ListaConfigurarProductos([FromBody] Tokens Tokens)
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
                respuesta = GestionConfigurarProducto.ListarConfigurarProductos();
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


        [HttpPost]
        [Route("api/Inventario/ListaConfigurarProductosTodos")]
        public object ListaConfigurarProductosTodos([FromBody] Tokens Tokens)
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
                respuesta = GestionConfigurarProducto.ListarConfigurarProductosTodos();
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



        [HttpPost]
        [Route("api/Inventario/ListaConfigurarProductosQueNoTieneUnKit")]
        public object ListaConfigurarProductosQueNoTieneUnKit(Kit Kit)
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
                respuesta = GestionConfigurarProducto.CargarConfigurarProductosQueNoTieneUnKit(int.Parse(Kit.IdKit));
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
