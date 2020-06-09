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
    public class MedidaController : ApiController
    {
        CatalogoMedida GestionMedida = new CatalogoMedida();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/Inventario/IngresoMedida")]
        public object IngresoMedida(Medida Medida)
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
                string ClavePutEncripBD = p.desencriptar(Medida.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (Medida.Descripcion == null || string.IsNullOrEmpty(Medida.Descripcion.Trim()))
                {
                    codigo = "418";
                    mensaje = "Por favor ingrese la descripcion a guardar";
                }
                else
                {
                    Medida DatoMedida = new Medida();
                    DatoMedida = GestionMedida.ConsultarMedidaPorDescripcion(Medida.Descripcion).FirstOrDefault();
                    if (DatoMedida == null)
                    {
                        DatoMedida = new Medida();
                        DatoMedida = GestionMedida.InsertarMedida(Medida);
                        if (DatoMedida.IdMedida == null || string.IsNullOrEmpty(DatoMedida.IdMedida))
                        {
                            mensaje = "Ocurrio un error al ingresar la medida";
                            codigo = "500";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            respuesta = DatoMedida;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        mensaje = "La medida "+ DatoMedida.Descripcion+ " ya existe";
                        codigo = "500";
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
        [Route("api/Inventario/EliminarMedida")]
        public object EliminarMedida(Medida Medida)
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
                string ClavePutEncripBD = p.desencriptar(Medida.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (Medida.IdMedida == null || string.IsNullOrEmpty(Medida.IdMedida.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id de la medida a eliminar";
                }
                else
                {
                    Medida.IdMedida = Seguridad.DesEncriptar(Medida.IdMedida);
                    Medida DatoMedida = new Medida();
                    DatoMedida = GestionMedida.ConsultarMedidaPorId(int.Parse(Medida.IdMedida)).FirstOrDefault();
                    if (DatoMedida == null)
                    {
                        codigo = "500";
                        mensaje = "La medida que intenta eliminar no existe";
                    }
                    else if(DatoMedida.MedidaUtilizado == "1")
                    {
                        codigo = "500";
                        mensaje = "La medida que intenta eliminar ya esta siendo usada";
                    }
                    else
                    {
                        if (GestionMedida.EliminarMedida(int.Parse(Medida.IdMedida)) == true)
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                        }
                        else
                        {
                            mensaje = "Ocurrio un error al intentar eliminar la medida";
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
        [Route("api/Inventario/ActualizarMedida")]
        public object ActualizarMedida(Medida Medida)
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
                string ClavePutEncripBD = p.desencriptar(Medida.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (Medida.IdMedida == null || string.IsNullOrEmpty(Medida.IdMedida.Trim()))
                {
                    mensaje = "Ingrese el id medida";
                    codigo = "418";
                }
                else if(Medida.Descripcion == null || string.IsNullOrEmpty(Medida.Descripcion.Trim()))
                {
                    mensaje = "Ingrese la medida";
                    codigo = "418";
                }
                else
                {
                    Medida DatoMedida = new Medida();
                    DatoMedida = GestionMedida.ConsultarMedidaPorDescripcion(Medida.Descripcion).FirstOrDefault();
                    if (DatoMedida == null)
                    {
                        DatoMedida = new Medida();
                        Medida.IdMedida = Seguridad.DesEncriptar(Medida.IdMedida);
                        DatoMedida = GestionMedida.ConsultarMedidaPorId(int.Parse(Medida.IdMedida)).FirstOrDefault();
                        if (DatoMedida == null)
                        {
                            mensaje = "La medida que desea actualizar no existe";
                            codigo = "500";
                        }
                        else
                        {
                            DatoMedida = new Medida();
                            DatoMedida = GestionMedida.ModificarMedida(Medida);
                            if (DatoMedida.IdMedida == null || string.IsNullOrEmpty(DatoMedida.IdMedida.Trim()))
                            {
                                mensaje = "Ocurrio un error al tratar de modificar la medida";
                                codigo = "500";
                            }
                            else
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                                respuesta = DatoMedida;
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                    }
                    else
                    {
                        mensaje = "La medida "+ DatoMedida.Descripcion+ " ya existe";
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
        [Route("api/Inventario/ListaMedidas")]
        public object ListaMedidas([FromBody] Tokens Tokens)
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
                respuesta = GestionMedida.ListarMedidas();
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
