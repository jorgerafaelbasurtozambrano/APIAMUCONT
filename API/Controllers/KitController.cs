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
    public class KitController : ApiController
    {
        CatalogoKit GestionKit = new CatalogoKit();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        [HttpPost]
        [Route("api/Inventario/IngresoKit")]
        public object IngresoKit(Kit Kit)
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
                string ClavePutEncripBD = p.desencriptar(Kit.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (Kit.Codigo == null || string.IsNullOrEmpty(Kit.Codigo.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el codigo del kit";
                }
                else if (Kit.Descripcion == null || string.IsNullOrEmpty(Kit.Descripcion.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese la descripcion del kit";
                }
                else if(Kit.AsignarDescuentoKit.Descuento.Porcentaje == null || string.IsNullOrEmpty(Kit.AsignarDescuentoKit.Descuento.Porcentaje.ToString().Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el porcentaje del kit";
                }
                else
                {
                    Kit DatoKit = new Kit();
                    DatoKit = GestionKit.ConsultarKitPorCodigo(Kit.Codigo).FirstOrDefault();
                    if (DatoKit == null)
                    {
                        DatoKit = new Kit();
                        DatoKit = GestionKit.InsertarKit(Kit);
                        if (DatoKit.IdKit == null || string.IsNullOrEmpty(DatoKit.IdKit.Trim()))
                        {
                            codigo = "500";
                            mensaje = "Ocurrio un error al ingresar el kit";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            respuesta = DatoKit;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                    else
                    {
                        codigo = "418";
                        mensaje = "El kit "+ DatoKit.Descripcion+" con el codigo "+ DatoKit.Codigo + " ya existe";
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
        [Route("api/Inventario/EliminarKit")]
        public object EliminarKit(Kit Kit)
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
                string ClavePutEncripBD = p.desencriptar(Kit.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (Kit.IdKit == null || string.IsNullOrEmpty(Kit.IdKit.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id kit a eliminar";
                }
                else
                {
                    Kit.IdKit = Seguridad.DesEncriptar(Kit.IdKit);
                    Kit DatoKit = new Kit();
                    DatoKit = GestionKit.ConsultarKitPorId(int.Parse(Kit.IdKit)).FirstOrDefault();
                    if (DatoKit == null)
                    {
                        codigo = "500";
                        mensaje = "El kit que desea a eliminar no existe";
                    }
                    else
                    {
                        if (DatoKit.KitUtilizado == "1")
                        {
                            codigo = "418";
                            mensaje = "El kit que desea eliminar esta siendo utilizado";
                        }
                        else
                        {
                            if (GestionKit.EliminarKit(int.Parse(Kit.IdKit)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                mensaje = "Ocurrio un error al eliminar el kit";
                                codigo = "500";
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
        [Route("api/Inventario/ActualizarKit")]
        public object ActualizarKit(Kit Kit)
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
                string ClavePutEncripBD = p.desencriptar(Kit.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (Kit.IdKit == null || string.IsNullOrEmpty(Kit.IdKit.Trim()))
                {
                    codigo = "418";
                    mensaje = "ingrese el id kit a modificar";
                }
                else if (Kit.Codigo == null || string.IsNullOrEmpty(Kit.Codigo.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el nuevo codigo";
                }
                else if (Kit.Descripcion == null || string.IsNullOrEmpty(Kit.Descripcion.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese la descripcion del kit";
                }
                else
                {
                    Kit.IdKit = Seguridad.DesEncriptar(Kit.IdKit);
                    Kit DatoKit = new Kit();
                    DatoKit = GestionKit.ConsultarKitPorId(int.Parse(Kit.IdKit)).FirstOrDefault();
                    if (DatoKit == null)
                    {
                        codigo = "500";
                        mensaje = "El kit que intenta modificar no existe";
                    }
                    else
                    {
                        DatoKit = new Kit();
                        DatoKit = GestionKit.ModificarKit(Kit);
                        if (DatoKit.IdKit == null || string.IsNullOrEmpty(DatoKit.IdKit.Trim()))
                        {
                            mensaje = "Ocurrio un error al tratar de modificar el kit";
                            codigo = "500";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            respuesta = DatoKit;
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
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
        [Route("api/Inventario/ListaKit")]
        public object ListaKit([FromBody] Tokens Tokens)
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
                respuesta = GestionKit.ListarKit();
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
