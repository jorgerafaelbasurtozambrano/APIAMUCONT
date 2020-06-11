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
using Negocio.Logica.Usuarios;
using Negocio;
using Negocio.Logica.Credito;

namespace API.Controllers
{
    public class AsignacionTipoUsuarioController : ApiController
    {
        //static ConsultarUsuariosYPersonas GestionUsuarios = new ConsultarUsuariosYPersonas();
        CatalogoAsignacionTipoUsuario GestionTipoUsuario = new CatalogoAsignacionTipoUsuario();
        CatalogoAsignarComunidadFactura _GestionAsignarComunidadConfigurarVenta = new CatalogoAsignarComunidadFactura();
        Prueba p = new Prueba();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        [HttpPost]
        [Route("api/TalentoHumano/IngresoTipoUsuario")]
        public object IngresoTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionTipoUsuarioEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (AsignacionTipoUsuarioEntidad.IdTipoUsuario == null || string.IsNullOrEmpty(AsignacionTipoUsuarioEntidad.IdTipoUsuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id tipo usuario";
                }
                else if(AsignacionTipoUsuarioEntidad.IdUsuario == null || string.IsNullOrEmpty(AsignacionTipoUsuarioEntidad.IdUsuario.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id usuario";
                }
                else
                {
                    AsignacionTipoUsuarioEntidad.IdTipoUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdTipoUsuario);
                    AsignacionTipoUsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdUsuario);
                    AsignacionTipoUsuario DatoAsignacionTipoUsuario = new AsignacionTipoUsuario();
                    DatoAsignacionTipoUsuario = GestionTipoUsuario.ConsultarTiposUsuarioQueTieneUnUsuario(int.Parse(AsignacionTipoUsuarioEntidad.IdUsuario)).Where(p => Seguridad.DesEncriptar(p.TipoUsuario.IdTipoUsuario) == AsignacionTipoUsuarioEntidad.IdTipoUsuario).FirstOrDefault();
                    if (DatoAsignacionTipoUsuario == null)
                    {
                        DatoAsignacionTipoUsuario = new AsignacionTipoUsuario();
                        DatoAsignacionTipoUsuario = GestionTipoUsuario.crearAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad);
                        if (DatoAsignacionTipoUsuario.IdAsignacionTUEncriptada == null || string.IsNullOrEmpty(DatoAsignacionTipoUsuario.IdAsignacionTUEncriptada.Trim()))
                        {
                            codigo = "500";
                            mensaje = "Ocurrio un error al agregar la asignacion";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            respuesta = DatoAsignacionTipoUsuario;
                        }
                    }
                    else
                    {
                        mensaje = "No se puede asignar este rol porque ya lo tiene activo";
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
        [Route("api/TalentoHumano/ActualizarAsignacionTipoUsuario")]
        public object ActualizarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionTipoUsuarioEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                AsignacionTipoUsuarioEntidad.IdTipoUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdTipoUsuario);
                AsignacionTipoUsuarioEntidad.IdUsuario = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdUsuario);
                GestionTipoUsuario.modificarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad);
                respuesta = AsignacionTipoUsuarioEntidad;
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
        [Route("api/TalentoHumano/EliminarAsignacionTipoUsuario")]
        public object EliminarAsignacionTipoUsuario(AsignacionTipoUsuarioEntidad AsignacionTipoUsuarioEntidad)
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
                string ClavePutEncripBD = p.desencriptar(AsignacionTipoUsuarioEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (AsignacionTipoUsuarioEntidad.IdAsignacionTU == null || string.IsNullOrEmpty(AsignacionTipoUsuarioEntidad.IdAsignacionTU.Trim()))
                {
                    codigo = "500";
                    mensaje = "Ingrese el id asignacion tu a eliminar";
                }
                else
                {
                    AsignacionTipoUsuarioEntidad.IdAsignacionTU = Seguridad.DesEncriptar(AsignacionTipoUsuarioEntidad.IdAsignacionTU);
                    AsignacionTipoUsuario DataAsignacionTipoUsuario = new AsignacionTipoUsuario();
                    DataAsignacionTipoUsuario = GestionTipoUsuario.ConsultarAsignarTUPorId(int.Parse(AsignacionTipoUsuarioEntidad.IdAsignacionTU)).FirstOrDefault();
                    if (DataAsignacionTipoUsuario == null)
                    {
                        codigo = "418";
                        mensaje = "El rol que desea eliminar no existe";
                    }
                    else
                    {
                        List<PersonaEntidad> PersonasAsignadas = new List<PersonaEntidad>();
                        PersonasAsignadas = _GestionAsignarComunidadConfigurarVenta.ConsultarPersonasAsignadasPorTecnico(int.Parse(AsignacionTipoUsuarioEntidad.IdAsignacionTU));
                        if (PersonasAsignadas.Count == 0)
                        {
                            if (GestionTipoUsuario.eliminarAsignacionTipoUsuario(int.Parse(AsignacionTipoUsuarioEntidad.IdAsignacionTU)) == true)
                            {
                                List<AsignacionTipoUsuario> DatoAsignacionTipoUsuario = new List<AsignacionTipoUsuario>();
                                DatoAsignacionTipoUsuario = GestionTipoUsuario.ConsultarTiposUsuarioQueTieneUnUsuario(DataAsignacionTipoUsuario.IdUsuario);
                                if (DatoAsignacionTipoUsuario.Count() == 0)
                                {
                                    if (GestionTipoUsuario.EliminarUsuario(DataAsignacionTipoUsuario.IdUsuario) == true)
                                    {
                                        codigo = "201";
                                        mensaje = "Se elimino el rol y se inhabilito el usuario";
                                    }
                                    else
                                    {
                                        codigo = "500";
                                        mensaje = "Ocurrio un error al deshabilitar el usuario";
                                    }
                                }
                                else
                                {
                                    codigo = "200";
                                    mensaje = "Se elimino el rol";
                                }
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al tratar de eliminar el rol";
                            }
                        }
                        else
                        {
                            codigo = "409";
                            mensaje = "No se puede eliminar este rol porque tiene cliente para seguimiento asignados, por favor vaya a la seccion de trasnferencia de técnico";
                            respuesta = PersonasAsignadas;
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
    }
}
