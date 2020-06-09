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
                if (ConfigurarProducto.IdAsignacionTu == null || string.IsNullOrEmpty(ConfigurarProducto.IdAsignacionTu.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id del usuario logeado";
                }
                else if (ConfigurarProducto.IdMedida == null || string.IsNullOrEmpty(ConfigurarProducto.IdMedida.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id de la medida";
                }
                else if (ConfigurarProducto.IdPresentacion == null || string.IsNullOrEmpty(ConfigurarProducto.IdPresentacion.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id de la presentacion";
                }
                else if (ConfigurarProducto.IdProducto == null || string.IsNullOrEmpty(ConfigurarProducto.IdProducto.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id del producto";
                }
                else if (ConfigurarProducto.Codigo == null || string.IsNullOrEmpty(ConfigurarProducto.Codigo.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el codigo";
                }
                else
                {
                    if (ConfigurarProducto.Iva < 0)
                    {
                        codigo = "500";
                        mensaje = "Ingrese correctamente el valor del iva";
                    }
                    else
                    {
                        if (ConfigurarProducto.Iva == null || string.IsNullOrEmpty(ConfigurarProducto.Iva.ToString().Trim()))
                        {
                            ConfigurarProducto.Iva = 0;
                        }
                        ConfigurarProductos DatoConfigurarProductos = new ConfigurarProductos();
                        DatoConfigurarProductos = GestionConfigurarProducto.ConsultarConfiguracionProductoPorCodigo(ConfigurarProducto.Codigo).FirstOrDefault();
                        if (DatoConfigurarProductos == null)
                        {

                            ConfigurarProducto.IdAsignacionTu = Seguridad.DesEncriptar(ConfigurarProducto.IdAsignacionTu);
                            ConfigurarProducto.IdMedida = Seguridad.DesEncriptar(ConfigurarProducto.IdMedida);
                            ConfigurarProducto.IdPresentacion = Seguridad.DesEncriptar(ConfigurarProducto.IdPresentacion);
                            ConfigurarProducto.IdProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdProducto);
                            ConfigurarProducto DatoConfigurarProducto = new ConfigurarProducto();
                            DatoConfigurarProducto = GestionConfigurarProducto.ConsultarSiExisteYaUnaConfiguracion(ConfigurarProducto).FirstOrDefault();
                            if (DatoConfigurarProducto == null)
                            {
                                DatoConfigurarProducto = new ConfigurarProducto();
                                DatoConfigurarProducto = GestionConfigurarProducto.InsertarConfigurarProducto(ConfigurarProducto);
                                if (DatoConfigurarProducto.IdConfigurarProducto == null || string.IsNullOrEmpty(DatoConfigurarProducto.IdConfigurarProducto.Trim()))
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al intentar guardar la configuracion del producto";
                                }
                                else
                                {
                                    respuesta = DatoConfigurarProducto;
                                    codigo = "200";
                                    mensaje = "EXITO";
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                codigo = "418";
                                mensaje = "Ya existe un producto con la misma configuracion";
                            }
                        }
                        else
                        {
                            codigo = "418";
                            mensaje = "Ya existe una configuracion de un producto con el mismo codigo";
                        }
                    }
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
                mensaje = e.Message;
                codigo = "500";
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
                if (ConfigurarProducto.IdConfigurarProducto == null || string.IsNullOrEmpty(ConfigurarProducto.IdConfigurarProducto.Trim()))
                {
                    mensaje = "Ingrese el id configurar producto";
                    codigo = "418";
                }
                else
                {
                    ConfigurarProducto.IdConfigurarProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdConfigurarProducto.Trim());
                    ConfigurarProductos DatoConfigurarProducto = new ConfigurarProductos();
                    DatoConfigurarProducto = GestionConfigurarProducto.ConsultarConfigurarProductoPorId(int.Parse(ConfigurarProducto.IdConfigurarProducto)).FirstOrDefault();
                    if (DatoConfigurarProducto == null)
                    {
                        mensaje = "La configuracion del producto que intenta eliminar no existe";
                        codigo = "500";
                    }
                    else if(DatoConfigurarProducto.ConfigurarProductosUtilizado == "1")
                    {
                        mensaje = "No se puede eliminar la configuracion porque ya esta siendo utilizada";
                        codigo = "500";
                    }
                    else
                    {
                        if (GestionConfigurarProducto.EliminarConfigurarProducto(DatoConfigurarProducto) == true)
                        {
                            codigo = "200";
                            mensaje = "EXITO";
                        }
                        else
                        {
                            codigo = "200";
                            mensaje = "Error al intentar eliminar la configuracion producto";
                        }
                    }
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
                mensaje = e.Message;
                codigo = "500";
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
                if (ConfigurarProducto.IdAsignacionTu == null || string.IsNullOrEmpty(ConfigurarProducto.IdAsignacionTu.Trim()))
                {
                    mensaje = "Falta el id asignacion tu";
                    codigo = "418";
                }
                else if (ConfigurarProducto.IdMedida == null || string.IsNullOrEmpty(ConfigurarProducto.IdMedida.Trim()))
                {
                    mensaje = "Falta el id de la medida";
                    codigo = "418";
                }
                else if (ConfigurarProducto.IdPresentacion == null || string.IsNullOrEmpty(ConfigurarProducto.IdPresentacion.Trim()))
                {
                    mensaje = "Falta el id de la presentacion";
                    codigo = "418";
                }
                else if (ConfigurarProducto.IdProducto == null  || string.IsNullOrEmpty(ConfigurarProducto.IdProducto.Trim()))
                {
                    mensaje = "Falta el id del producto";
                    codigo = "418";
                }
                else if(ConfigurarProducto.IdConfigurarProducto == null || string.IsNullOrEmpty(ConfigurarProducto.IdConfigurarProducto.Trim()))
                {
                    mensaje = "Falta el id de la configuracion";
                    codigo = "418";
                }
                else
                {
                    if (ConfigurarProducto.Iva == null || ConfigurarProducto.Iva <= 0)
                    {
                        ConfigurarProducto.Iva = 0;
                    }
                    ConfigurarProducto.IdAsignacionTu = Seguridad.DesEncriptar(ConfigurarProducto.IdAsignacionTu);
                    ConfigurarProducto.IdMedida = Seguridad.DesEncriptar(ConfigurarProducto.IdMedida);
                    ConfigurarProducto.IdPresentacion = Seguridad.DesEncriptar(ConfigurarProducto.IdPresentacion);
                    ConfigurarProducto.IdProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdProducto);
                    ConfigurarProducto.IdConfigurarProducto = Seguridad.DesEncriptar(ConfigurarProducto.IdConfigurarProducto);
                    ConfigurarProductos DatoConfigurarProductos = new ConfigurarProductos();
                    DatoConfigurarProductos = GestionConfigurarProducto.ModificarConfigurarProducto(ConfigurarProducto);
                    if (DatoConfigurarProductos.IdConfigurarProducto == null)
                    {
                        codigo = "500";
                        mensaje = "Ocurrio un error al intentar modificar la configuracion";
                    }
                    else
                    {
                        mensaje = "EXITO";
                        codigo = "200";
                        respuesta = DatoConfigurarProductos;
                        objeto = new { codigo, mensaje, respuesta };
                        return objeto;
                    }
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
                mensaje = e.Message;
                codigo = "500";
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
