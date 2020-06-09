using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio.Logica;
using Negocio.Logica.TalentHumano;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.Seguridad;
using Negocio;

namespace API.Controllers
{
    public class ComunidadController : ApiController
    {
        CatalogoComunidad GestionComunidad = new CatalogoComunidad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoParroquia Gestionparroquia = new CatalogoParroquia();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoComunidad")]
        public object IngresoComunidad(ComunidadEntidad ComunidadEntidad)
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
                string ClavePutEncripBD = p.desencriptar(ComunidadEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (ComunidadEntidad.Descripcion == null || string.IsNullOrEmpty(ComunidadEntidad.Descripcion.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion de la comunidad";
                }
                else if (ComunidadEntidad.IdParroquia == null || string.IsNullOrEmpty(ComunidadEntidad.IdParroquia.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la parroquia";
                }
                else
                {
                    ComunidadEntidad.IdParroquia = Seguridad.DesEncriptar(ComunidadEntidad.IdParroquia);
                    Parroquia DatoParroquia = new Parroquia();
                    DatoParroquia = Gestionparroquia.ConsultarParroquiaPorId(int.Parse(ComunidadEntidad.IdParroquia)).FirstOrDefault();
                    if (DatoParroquia == null)
                    {
                        codigo = "500";
                        mensaje = "La parroquia a la que desea asignar no existe";
                    }
                    else
                    {
                        Comunidad DatoComunidad = new Comunidad();
                        //DatoComunidad = GestionComunidad.ConsultarComunidadPorDescripcion(ComunidadEntidad.Descripcion.ToUpper()).FirstOrDefault();
                        DatoComunidad = GestionComunidad.ConsultarComunidadPorDescripcion(new ComunidadEntidad() { Descripcion = ComunidadEntidad.Descripcion.ToUpper(),IdParroquia = ComunidadEntidad.IdParroquia.Trim()}).FirstOrDefault();
                        if (DatoComunidad == null)
                        {
                            DatoComunidad = new Comunidad();
                            DatoComunidad = GestionComunidad.IngresarComunidad(ComunidadEntidad);
                            if (DatoComunidad.IdComunidad == null || string.IsNullOrEmpty(DatoComunidad.IdComunidad.Trim()))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error en el servidor";
                            }
                            else
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                                respuesta = DatoComunidad;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                        else
                        {
                            codigo = "418";
                            mensaje = "Ya existe la comunidad que quiere insertar";
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
                mensaje = "ERROR";
                codigo = "418";
                objeto = new { codigo, mensaje };
                return objeto;
            }
        }

        [HttpPost]
        [Route("api/TalentoHumano/EliminarComunidad")]
        public object EliminarComunidad(ComunidadEntidad ComunidadEntidad)
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
                string ClavePutEncripBD = p.desencriptar(ComunidadEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (ComunidadEntidad.IdComunidad == null || string.IsNullOrEmpty(ComunidadEntidad.IdComunidad.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la comunidad a eliminar";
                }
                else
                {
                    ComunidadEntidad.IdComunidad = Seguridad.DesEncriptar(ComunidadEntidad.IdComunidad);
                    Comunidad DatoComunidad = new Comunidad();
                    DatoComunidad = GestionComunidad.ConsultarComunidadPorId(int.Parse(ComunidadEntidad.IdComunidad)).FirstOrDefault();
                    if (DatoComunidad == null)
                    {
                        codigo = "418";
                        mensaje = "La comunidad que intenta eliminar no existe";
                    }
                    else
                    {
                        if (DatoComunidad.PermitirEliminacion == true)
                        {
                            if (GestionComunidad.EliminarComunidad(int.Parse(ComunidadEntidad.IdComunidad)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar eliminar la comunidad";
                            }
                        }
                        else
                        {
                            codigo = "500";
                            mensaje = "No se puede eliminar la comunidad porque esta siendo usado";
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
        [Route("api/TalentoHumano/ActualizarComunidad")]
        public object ActualizarComunidad(ComunidadEntidad ComunidadEntidad)
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
                string ClavePutEncripBD = p.desencriptar(ComunidadEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (ComunidadEntidad.IdComunidad == null || string.IsNullOrEmpty(ComunidadEntidad.IdComunidad.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la comunidad a eliminar";
                }
                else if (ComunidadEntidad.IdParroquia == null || string.IsNullOrEmpty(ComunidadEntidad.IdParroquia.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la parroquia";
                }
                else if (ComunidadEntidad.Descripcion == null || string.IsNullOrEmpty(ComunidadEntidad.Descripcion.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion de la comunidad";
                }
                else
                {
                    ComunidadEntidad.IdParroquia = Seguridad.DesEncriptar(ComunidadEntidad.IdParroquia);
                    Parroquia DatoParroquia = new Parroquia();
                    DatoParroquia = Gestionparroquia.ConsultarParroquiaPorId(int.Parse(ComunidadEntidad.IdParroquia)).FirstOrDefault();
                    if (DatoParroquia == null)
                    {
                        codigo = "500";
                        mensaje = "La parroquia a la que desea asignar no existe";
                    }
                    else
                    {
                        ComunidadEntidad.IdComunidad = Seguridad.DesEncriptar(ComunidadEntidad.IdComunidad);
                        Comunidad DatoComunidad = new Comunidad();
                        DatoComunidad = GestionComunidad.ConsultarComunidadPorId(int.Parse(ComunidadEntidad.IdComunidad)).FirstOrDefault();
                        if (DatoComunidad == null)
                        {
                            codigo = "418";
                            mensaje = "La comunidad que intenta actualizar no existe";
                        }
                        else
                        {
                            DatoComunidad = new Comunidad();
                            DatoComunidad = GestionComunidad.ModificarComunidad(ComunidadEntidad);
                            if (DatoComunidad.IdComunidad ==null || string.IsNullOrEmpty(DatoComunidad.IdComunidad))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar modificar la comunidad";
                            }
                            else
                            {
                                respuesta = DatoComunidad;
                                mensaje = "EXITO";
                                codigo = "200";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
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
        [Route("api/TalentoHumano/ListaComunidad")]
        public object ListaComunidad([FromBody] Tokens Tokens)
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
                    respuesta = GestionComunidad.ObtenerListaComunidad();
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
        [Route("api/TalentoHumano/ListaComunidadParroquia")]
        public object ListaComunidadParroquia(Parroquia Parroquia)
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
                string ClaveGetEncripBD = p.desencriptar(Parroquia.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Parroquia.IdParroquia = Seguridad.DesEncriptar(Parroquia.IdParroquia);
                    respuesta = GestionComunidad.ListarComunidadParroquia(int.Parse(Parroquia.IdParroquia));
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
    }
}
