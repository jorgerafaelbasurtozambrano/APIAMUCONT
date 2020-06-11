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
    public class CantonController : ApiController
    {
        CatalogoCanton GestionCanton = new CatalogoCanton();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoProvincia GestionProvincia = new CatalogoProvincia();
        [HttpPost]
        [Route("api/TalentoHumano/IngresoCanton")]
        public object IngresoCanton(CantonEntidad CantonEntidad)
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
                string ClavePutEncripBD = p.desencriptar(CantonEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (CantonEntidad.Descripcion == null || string.IsNullOrEmpty(CantonEntidad.Descripcion.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion del canton";
                }
                else if (CantonEntidad.IdProvincia == null || string.IsNullOrEmpty(CantonEntidad.IdProvincia.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la provincia a la que quiere asignar el canton";
                }
                else
                {
                    CantonEntidad.IdProvincia = Seguridad.DesEncriptar(CantonEntidad.IdProvincia);
                    Provincia _DatoProvincia = new Provincia();
                    _DatoProvincia = GestionProvincia.ConsultarProvinciaPorId(int.Parse(CantonEntidad.IdProvincia)).FirstOrDefault();
                    if (_DatoProvincia != null)
                    {
                        Canton DatoCanton = new Canton();
                        DatoCanton = GestionCanton.ConsultarCantonPorDescripcion(CantonEntidad.Descripcion.ToUpper()).FirstOrDefault();
                        if (DatoCanton == null)
                        {
                            DatoCanton = new Canton();
                            DatoCanton = GestionCanton.IngresarCanton(CantonEntidad);
                            if (DatoCanton.IdCanton == null || string.IsNullOrEmpty(DatoCanton.IdCanton.Trim()))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error en el servidor";
                            }
                            else
                            {
                                respuesta = DatoCanton;
                                codigo = "200";
                                mensaje = "EXITO";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                        else
                        {
                            codigo = "418";
                            mensaje = "Ya existe el canton que quiere insertar";
                        }
                    }
                    else
                    {
                        codigo = "500";
                        mensaje = "La provincia a la que desea asignar no existe";
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
        [Route("api/TalentoHumano/EliminarCanton")]
        public object EliminarCanton(CantonEntidad CantonEntidad)
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
                string ClavePutEncripBD = p.desencriptar(CantonEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (CantonEntidad.IdCanton == null || string.IsNullOrEmpty(CantonEntidad.IdCanton.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id del canton a eliminar";
                }
                else
                {
                    CantonEntidad.IdCanton = Seguridad.DesEncriptar(CantonEntidad.IdCanton);
                    Canton DatoCanton = new Canton();
                    DatoCanton = GestionCanton.ConsultarCantonPorId(int.Parse(CantonEntidad.IdCanton)).FirstOrDefault();
                    if (DatoCanton == null)
                    {
                        codigo = "418";
                        mensaje = "El canton que intenta eliminar no existe";
                    }
                    else
                    {
                        if (DatoCanton.PermitirEliminacion == true)
                        {
                            if (GestionCanton.EliminarCanton(int.Parse(CantonEntidad.IdCanton)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar eliminar el canton";
                            }
                        }
                        else
                        {
                            codigo = "500";
                            mensaje = "No se puede eliminar el canton porque esta siendo usado";
                        }
                    }
                }
                //}
                //else
                //{
                //mensaje = "ERROR";
                //codigo = "401";
                //}
                objeto = new {codigo,mensaje};
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
        [Route("api/TalentoHumano/ActualizarCanton")]
        public object ActualizarCanton(CantonEntidad CantonEntidad)
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
                string ClavePutEncripBD = p.desencriptar(CantonEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (CantonEntidad.IdCanton == null || string.IsNullOrEmpty(CantonEntidad.IdCanton.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion del canton";
                }
                else if (CantonEntidad.Descripcion == null || string.IsNullOrEmpty(CantonEntidad.Descripcion.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la descripcion del canton";
                }
                else if (CantonEntidad.IdProvincia == null || string.IsNullOrEmpty(CantonEntidad.IdProvincia.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta la provincia a la que quiere asignar el canton";
                }
                else
                {
                    CantonEntidad.IdProvincia = Seguridad.DesEncriptar(CantonEntidad.IdProvincia);
                    Provincia _DatoProvincia = new Provincia();
                    _DatoProvincia = GestionProvincia.ConsultarProvinciaPorId(int.Parse(CantonEntidad.IdProvincia)).FirstOrDefault();
                    if (_DatoProvincia != null)
                    {
                        CantonEntidad.IdCanton = Seguridad.DesEncriptar(CantonEntidad.IdCanton);
                        Canton DatoCanton = new Canton();
                        DatoCanton = GestionCanton.ConsultarCantonPorId(int.Parse(CantonEntidad.IdCanton)).FirstOrDefault();
                        if (DatoCanton == null)
                        {
                            codigo = "418";
                            mensaje = "El canton que intenta modificar no existe";
                        }
                        else
                        {
                            DatoCanton = new Canton();
                            DatoCanton = GestionCanton.ModificarCanton(CantonEntidad);
                            if (DatoCanton.IdCanton == null || string.IsNullOrEmpty(DatoCanton.IdCanton))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar modificar el canton";
                            }
                            else
                            {
                                respuesta = DatoCanton;
                                mensaje = "EXITO";
                                codigo = "200";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                    else
                    {
                        codigo = "500";
                        mensaje = "La provincia a la que desea asignar no existe";
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
        [Route("api/TalentoHumano/ListaCantones")]
        public object ListaCantones([FromBody] Tokens Tokens)
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
                    respuesta = GestionCanton.ObtenerListaCanton();
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
        [Route("api/TalentoHumano/ListaCantonesProvincia")]
        public object ListaCantonesProvincia(Provincia Provincia)
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
                string ClaveGetEncripBD = p.desencriptar(Provincia.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                    respuesta = GestionCanton.ListaCantonesProvincia(int.Parse(Provincia.IdProvincia));
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
        [Route("api/TalentoHumano/ConsultarCantonesParaSeguimiento")]
        public object ConsultarCantonesParaSeguimiento(Provincia Provincia)
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
                string ClaveGetEncripBD = p.desencriptar(Provincia.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (Provincia.IdProvincia == null || string.IsNullOrEmpty(Provincia.IdProvincia.Trim()))
                {
                    mensaje = "Ingrese el id provincia";
                    codigo = "418";
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    Provincia.IdProvincia = Seguridad.DesEncriptar(Provincia.IdProvincia);
                    respuesta = GestionCanton.ConsultarCantonesParaSeguimiento(int.Parse(Provincia.IdProvincia));
                    objeto = new { codigo, mensaje, respuesta };
                    return objeto;
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

    }
}
