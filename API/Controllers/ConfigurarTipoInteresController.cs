using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades;
using Negocio.Logica.Seguridad;
using Negocio.Logica.Credito;
using Negocio;
namespace API.Controllers
{
    public class ConfigurarTipoInteresController : ApiController
    {
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoTipoInteres GestionTipoInteres = new CatalogoTipoInteres();
        CatalogoConfiguracionInteres GestionConfigurarInteres = new CatalogoConfiguracionInteres();
        [HttpPost]
        [Route("api/Credito/ListaTipoInteres")]
        public object ListaTipoInteres([FromBody] Tokens Tokens)
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
                respuesta = GestionTipoInteres.ListarTipoInteres();
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
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Credito/ListaConfiguracionInteres")]
        public object ListaConfiguracionInteres([FromBody] Tokens Tokens)
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
                respuesta = GestionConfigurarInteres.ListarConfiguracionInteres();
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
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Credito/IngresoConfiguracionInteres")]
        public object IngresoConfiguracionInteres(ConfiguracionInteres _ConfiguracionInteres)
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
                string ClavePutEncripBD = p.desencriptar(_ConfiguracionInteres.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (_ConfiguracionInteres.IdTipoInteres == null || string.IsNullOrEmpty(_ConfiguracionInteres.IdTipoInteres.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id del tipo de interes normal";
                }
                else if (_ConfiguracionInteres.IdTipoInteresMora == null || string.IsNullOrEmpty(_ConfiguracionInteres.IdTipoInteresMora.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id del tipo de interes de mora";
                }
                else if (_ConfiguracionInteres.TasaInteres == null)
                {
                    codigo = "400";
                    mensaje = "Falta la tasa de interes normal";
                }
                else if (_ConfiguracionInteres.TasaInteresMora == null)
                {
                    codigo = "400";
                    mensaje = "Falta la tasa de interes de mora";
                }
                else
                {
                    if (_ConfiguracionInteres.TasaInteres <=0 || _ConfiguracionInteres.TasaInteresMora <= 0)
                    {
                        codigo = "400";
                        mensaje = "No se puede guardar la tasa de interes menor o igual a cero";
                    }
                    else
                    {
                        _ConfiguracionInteres.IdTipoInteresMora = Seguridad.DesEncriptar(_ConfiguracionInteres.IdTipoInteresMora);
                        _ConfiguracionInteres.IdTipoInteres = Seguridad.DesEncriptar(_ConfiguracionInteres.IdTipoInteres);
                        if (_ConfiguracionInteres.IdTipoInteres == _ConfiguracionInteres.IdTipoInteresMora)
                        {
                            codigo = "400";
                            mensaje = "Los dos intereses son iguales y eso no esta permitido";
                        }
                        else
                        {
                            ConfiguracionInteres DatoConfiguracionInteres = new ConfiguracionInteres();
                            DatoConfiguracionInteres = GestionConfigurarInteres.ConsultarConfiguracionInteresExiste(_ConfiguracionInteres).FirstOrDefault();
                            if (DatoConfiguracionInteres == null)
                            {
                                DatoConfiguracionInteres = new ConfiguracionInteres();
                                DatoConfiguracionInteres = GestionConfigurarInteres.InsertarInteres(_ConfiguracionInteres);
                                if (DatoConfiguracionInteres.IdConfiguracionInteres == null)
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al ingresar la configuracion del interes";
                                }
                                else
                                {
                                    codigo = "200";
                                    mensaje = "EXITO";
                                    respuesta = DatoConfiguracionInteres;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                codigo = "418";
                                mensaje = "Ya existe la configuracion que desea añadir";
                            }
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
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Credito/EliminarDeshabilitarConfiguracionInteres")]
        public object EliminarConfiguracionInteres(ConfiguracionInteres _ConfiguracionInteres)
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
                string ClaveGetEncripBD = p.desencriptar(_ConfiguracionInteres.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_ConfiguracionInteres.IdConfiguracionInteres == null || string.IsNullOrEmpty(_ConfiguracionInteres.IdConfiguracionInteres.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la configuracion interes a eliminar";
                }
                else
                {
                    _ConfiguracionInteres.IdConfiguracionInteres = Seguridad.DesEncriptar(_ConfiguracionInteres.IdConfiguracionInteres);
                    ConfiguracionInteres DatoConfiguracionInteres = new ConfiguracionInteres();
                    DatoConfiguracionInteres = GestionConfigurarInteres.ConsultarConfiguracionInteresPorId(int.Parse(_ConfiguracionInteres.IdConfiguracionInteres)).FirstOrDefault();
                    if (DatoConfiguracionInteres == null)
                    {
                        codigo = "500";
                        mensaje = "No existe la configuracion a eliminar";
                    }
                    else
                    {
                        if (DatoConfiguracionInteres.utilizado == "1")
                        {
                            if (GestionConfigurarInteres.DesHabilitarInteres(int.Parse(_ConfiguracionInteres.IdConfiguracionInteres)) == true)
                            {
                                codigo = "201";
                                mensaje = "EXITO";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al tratar de habiliar el interes";
                            }
                        }
                        else
                        {
                            if (GestionConfigurarInteres.EliminarConfiguracionInteres(int.Parse(_ConfiguracionInteres.IdConfiguracionInteres))== true)
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al tratar de eliminar";
                            }
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
        [Route("api/Credito/ActualizarConfiguracionInteres")]
        public object ActualizarConfiguracionInteres(ConfiguracionInteres _ConfiguracionInteres)
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
                string ClavePutEncripBD = p.desencriptar(_ConfiguracionInteres.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (_ConfiguracionInteres.IdConfiguracionInteres == null || string.IsNullOrEmpty(_ConfiguracionInteres.IdConfiguracionInteres.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la configuracion a modificar";
                }
                else if (_ConfiguracionInteres.TasaInteres == null)
                {
                    codigo = "400";
                    mensaje = "Falta la tasa de interes normal";
                }
                else if (_ConfiguracionInteres.TasaInteresMora == null)
                {
                    codigo = "400";
                    mensaje = "Falta la tasa de interes de mora";
                }
                else
                {
                    if (_ConfiguracionInteres.TasaInteres <= 0 || _ConfiguracionInteres.TasaInteresMora <= 0)
                    {
                        codigo = "400";
                        mensaje = "No se puede guardar la tasa de interes menor o igual a cero";
                    }
                    else
                    {
                        _ConfiguracionInteres.IdConfiguracionInteres = Seguridad.DesEncriptar(_ConfiguracionInteres.IdConfiguracionInteres);
                        ConfiguracionInteres DatoConfiguracionInteres = new ConfiguracionInteres();
                        DatoConfiguracionInteres = GestionConfigurarInteres.ConsultarConfiguracionInteresPorId(int.Parse(_ConfiguracionInteres.IdConfiguracionInteres)).FirstOrDefault();
                        if (DatoConfiguracionInteres == null)
                        {
                            codigo = "500";
                            mensaje = "No existe la configuracion a eliminar";
                        }
                        else
                        {
                            if (DatoConfiguracionInteres.utilizado == "1")
                            {
                                codigo = "418";
                                mensaje = "No se puede modificar porque ya esta siendo usado";
                            }
                            else
                            {
                                DatoConfiguracionInteres.IdConfiguracionInteres = Seguridad.DesEncriptar(DatoConfiguracionInteres.IdConfiguracionInteres);
                                DatoConfiguracionInteres.IdTipoInteres = Seguridad.DesEncriptar(DatoConfiguracionInteres.IdTipoInteres);
                                DatoConfiguracionInteres.IdTipoInteresMora = Seguridad.DesEncriptar(DatoConfiguracionInteres.IdTipoInteresMora);
                                DatoConfiguracionInteres.TasaInteres = _ConfiguracionInteres.TasaInteres;
                                DatoConfiguracionInteres.TasaInteresMora = _ConfiguracionInteres.TasaInteresMora;
                                ConfiguracionInteres DatosConfiguracionInteres = new ConfiguracionInteres();
                                DatosConfiguracionInteres = GestionConfigurarInteres.ConsultarConfiguracionInteresExiste(DatoConfiguracionInteres).FirstOrDefault();
                                if (DatosConfiguracionInteres == null)
                                {
                                    DatosConfiguracionInteres = new ConfiguracionInteres();
                                    DatosConfiguracionInteres = GestionConfigurarInteres.ModificarConfiguracionInteres(DatoConfiguracionInteres);
                                    if (DatosConfiguracionInteres.IdConfiguracionInteres == null)
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al tratar de modificar el interes";
                                    }
                                    else
                                    {
                                        codigo = "200";
                                        mensaje = "Exito";
                                        respuesta = DatosConfiguracionInteres;
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                                else
                                {
                                    codigo = "418";
                                    mensaje = "Ya existe la configuracion que desea actualizar";
                                }
                            }
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
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/Credito/HabilitarConfiguracionInteres")]
        public object HabilitarConfiguracionInteres(ConfiguracionInteres _ConfiguracionInteres)
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
                string ClaveGetEncripBD = p.desencriptar(_ConfiguracionInteres.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (_ConfiguracionInteres.IdConfiguracionInteres == null || string.IsNullOrEmpty(_ConfiguracionInteres.IdConfiguracionInteres.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la configuracion interes a habilitar";
                }
                else
                {
                    _ConfiguracionInteres.IdConfiguracionInteres = Seguridad.DesEncriptar(_ConfiguracionInteres.IdConfiguracionInteres);
                    ConfiguracionInteres DatoConfiguracionInteres = new ConfiguracionInteres();
                    DatoConfiguracionInteres = GestionConfigurarInteres.ConsultarConfiguracionInteresPorId(int.Parse(_ConfiguracionInteres.IdConfiguracionInteres)).FirstOrDefault();
                    if (DatoConfiguracionInteres == null)
                    {
                        codigo = "500";
                        mensaje = "No existe la configuracion a habilitar";
                    }
                    else
                    {
                        if (GestionConfigurarInteres.HabilitarInteres(int.Parse(_ConfiguracionInteres.IdConfiguracionInteres))==true)
                        {
                            codigo = "200";
                            mensaje = "EXITO";
                        }
                        else
                        {
                            codigo = "500";
                            mensaje = "Ocurrio un error al tratar de habiliar el interes";
                        }
                    }
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
                mensaje = e.Message;
                codigo = "500";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
    }
}
