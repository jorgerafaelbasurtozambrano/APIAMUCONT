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
    public class PresentacionController : ApiController
    {
        CatalogoPresentacion GestionPresentacion = new CatalogoPresentacion();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoPresentacion")]
        public object IngresoPresentacion(Presentacion Presentacion)
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
                string ClavePutEncripBD = p.desencriptar(Presentacion.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (Presentacion.Descripcion == null || string.IsNullOrEmpty(Presentacion.Descripcion.Trim()))
                {
                    codigo = "418";
                    mensaje = "Por favor ingrese la descripcion a guardar";
                }
                else
                {
                    Presentacion DatoPresentacion = new Presentacion();
                    DatoPresentacion = GestionPresentacion.ConsultarPresentacionPorDescripcion(Presentacion.Descripcion).FirstOrDefault();
                    if (DatoPresentacion == null)
                    {
                        DatoPresentacion = new Presentacion();
                        DatoPresentacion = GestionPresentacion.InsertarPresentacion(Presentacion);
                        if (DatoPresentacion.IdPresentacion == null || string.IsNullOrEmpty(DatoPresentacion.IdPresentacion))
                        {
                            mensaje = "Ocurrio un error al ingresar la presentacion";
                            codigo = "500";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            respuesta = DatoPresentacion;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        mensaje = "La presentacion " + DatoPresentacion.Descripcion + " ya existe";
                        codigo = "418";
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
        [Route("api/Inventario/EliminarPresentacion")]
        public object EliminarPresentacion(Presentacion Presentacion)
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
                string ClavePutEncripBD = p.desencriptar(Presentacion.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (Presentacion.IdPresentacion == null || string.IsNullOrEmpty(Presentacion.IdPresentacion.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id de la presentacion a eliminar";
                }
                else
                {
                    Presentacion.IdPresentacion = Seguridad.DesEncriptar(Presentacion.IdPresentacion);
                    Presentacion DatoPresentacion = new Presentacion();
                    DatoPresentacion = GestionPresentacion.ConsultarPresentacionPorId(int.Parse(Presentacion.IdPresentacion)).FirstOrDefault();
                    if (DatoPresentacion == null)
                    {
                        codigo = "500";
                        mensaje = "La presentacion que intenta eliminar no existe";
                    }
                    else if (DatoPresentacion.PresentacionUtilizado == "1")
                    {
                        codigo = "500";
                        mensaje = "La presentacion que intenta eliminar ya esta siendo usada";
                    }
                    else
                    {
                        if (GestionPresentacion.EliminarPresentacion(int.Parse(Presentacion.IdPresentacion)) == true)
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                        }
                        else
                        {
                            mensaje = "Ocurrio un error al intentar eliminar la presentacion";
                            codigo = "500";
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
        [Route("api/Inventario/ActualizarPresentacion")]
        public object ActualizarPresentacion(Presentacion Presentacion)
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
                string ClavePutEncripBD = p.desencriptar(Presentacion.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (Presentacion.IdPresentacion == null || string.IsNullOrEmpty(Presentacion.IdPresentacion.Trim()))
                {
                    mensaje = "Ingrese el id presentacion";
                    codigo = "418";
                }
                else if (Presentacion.Descripcion == null || string.IsNullOrEmpty(Presentacion.Descripcion.Trim()))
                {
                    mensaje = "Ingrese la presentacion";
                    codigo = "418";
                }
                else
                {
                    Presentacion DatoPresentacion = new Presentacion();
                    DatoPresentacion = GestionPresentacion.ConsultarPresentacionPorDescripcion(Presentacion.Descripcion).FirstOrDefault();
                    if (DatoPresentacion == null)
                    {
                        DatoPresentacion = new Presentacion();
                        Presentacion.IdPresentacion = Seguridad.DesEncriptar(Presentacion.IdPresentacion);
                        DatoPresentacion = GestionPresentacion.ConsultarPresentacionPorId(int.Parse(Presentacion.IdPresentacion)).FirstOrDefault();
                        if (DatoPresentacion == null)
                        {
                            mensaje = "La presentacion que desea actualizar no existe";
                            codigo = "500";
                        }
                        else
                        {
                            DatoPresentacion = new Presentacion();
                            DatoPresentacion = GestionPresentacion.ModificarPresentacion(Presentacion);
                            if (DatoPresentacion.IdPresentacion == null || string.IsNullOrEmpty(DatoPresentacion.IdPresentacion.Trim()))
                            {
                                mensaje = "Ocurrio un error al tratar de modificar la presentacion";
                                codigo = "500";
                            }
                            else
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                                respuesta = DatoPresentacion;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                    else
                    {
                        mensaje = "La presentacion"+ DatoPresentacion.Descripcion + " que desea colocar ya existe";
                        codigo = "418";
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
        [Route("api/Inventario/ListaPresentacion")]
        public object ListaPresentacion([FromBody] Tokens Tokens)
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
                respuesta = GestionPresentacion.ListarPresentaciones();
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
