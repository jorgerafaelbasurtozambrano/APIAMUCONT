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
using Negocio;

namespace API.Controllers
{
    public class ProvinciaController : ApiController
    {
        CatalogoProvincia GestionProvincia = new CatalogoProvincia();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoProvincia")]
        public object IngresoProvincia(Provincia Provincia)
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
                string ClavePutEncripBD = p.desencriptar(Provincia.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (Provincia.Descripcion == null || string.IsNullOrEmpty(Provincia.Descripcion.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion de la provincia";
                }
                else
                {
                    Provincia DatoProvincia = new Provincia();
                    DatoProvincia = GestionProvincia.ConsultarProvinciaPorDescripcion(Provincia.Descripcion.ToUpper()).FirstOrDefault();
                    if (DatoProvincia == null)
                    {
                        DatoProvincia = new Provincia();
                        DatoProvincia = GestionProvincia.IngresoProvincia(Provincia);
                        if (DatoProvincia.IdProvincia == null || string.IsNullOrEmpty(DatoProvincia.IdProvincia.Trim()))
                        {
                            codigo = "500";
                            mensaje = "Ocurrio un error en el servidor";
                        }
                        else
                        {
                            respuesta = DatoProvincia;
                            codigo = "200";
                            mensaje = "EXITO";
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        codigo = "418";
                        mensaje = "Ya existe la provincia que quiere insertar";
                    }
                }
                objeto = new {codigo, mensaje};
                return objeto;
                //GestionProvincia.IngresoProvincia(Provincia);
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
        [Route("api/TalentoHumano/EliminarProvincia")]
        public object EliminarProvincia(Provincia Provincia)
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
                string ClavePutEncripBD = p.desencriptar(Provincia.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (Provincia.IdProvincia == null || string.IsNullOrEmpty(Provincia.IdProvincia.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la provincia";
                }
                else
                {
                    Provincia DatoProvincia = new Provincia();
                    Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                    DatoProvincia = GestionProvincia.ConsultarProvinciaPorId(int.Parse(Provincia.IdProvincia)).FirstOrDefault();
                    if (DatoProvincia == null)
                    {
                        codigo = "500";
                        mensaje = "La provincia que quiere eliminar no existe";
                    }
                    else
                    {
                        if (DatoProvincia.PermitirEliminacion == true)
                        {
                            if (GestionProvincia.EliminarProvincia(int.Parse(Provincia.IdProvincia))== true)
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al eliminar la provincia";
                            }
                        }
                        else
                        {
                            codigo = "500";
                            mensaje = "No se puede eliminar porque esta siendo utilizado";
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
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }
        [HttpPost]
        [Route("api/TalentoHumano/ActualizarProvincia")]
        public object ActualizarProvincia(Provincia Provincia)
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
                string ClavePutEncripBD = p.desencriptar(Provincia.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (Provincia.IdProvincia == null || string.IsNullOrEmpty(Provincia.IdProvincia.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la provincia";
                }
                else if (Provincia.Descripcion == null || string.IsNullOrEmpty(Provincia.Descripcion.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion de la provincia";
                }
                else
                {
                    Provincia DatoProvincia = new Provincia();
                    DatoProvincia = GestionProvincia.ConsultarProvinciaPorDescripcion(Provincia.Descripcion.ToUpper()).FirstOrDefault();
                    if (DatoProvincia == null)
                    {
                        Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                        DatoProvincia = new Provincia();
                        DatoProvincia = GestionProvincia.ConsultarProvinciaPorId(int.Parse(Provincia.IdProvincia)).FirstOrDefault();
                        if (DatoProvincia == null)
                        {
                            codigo = "500";
                            mensaje = "La provincia que quiere modificar no existe";
                        }
                        else
                        {
                            DatoProvincia = new Provincia();
                            DatoProvincia = GestionProvincia.ModificarProvincia(Provincia);
                            if (DatoProvincia.IdProvincia == null)
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error en el servidor";
                            }
                            else
                            {
                                respuesta = DatoProvincia;
                                codigo = "200";
                                mensaje = "EXITO";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                    else
                    {
                        codigo = "418";
                        mensaje = "Ya existe la provincia que quiere modificar";
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
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/TalentoHumano/ListaProvincia")]
        public object ObtenerProvincias([FromBody] Tokens Tokens)
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
                    respuesta = GestionProvincia.ConsultarProvincias();
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
        [Route("api/TalentoHumano/ConsultarProvinciaParaSeguimiento")]
        public object ConsultarProvinciaParaSeguimiento([FromBody] Tokens Tokens)
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
                respuesta = GestionProvincia.ConsultarProvinciaParaSeguimiento();
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
    }
}
