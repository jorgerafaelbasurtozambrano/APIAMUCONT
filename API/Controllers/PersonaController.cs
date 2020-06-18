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
    public class PersonaController : ApiController
    {
        static ConsultarUsuariosYPersonas GestionUsuarios = new ConsultarUsuariosYPersonas();
        CatalogoPersona GestionPersona = new CatalogoPersona();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoTipoDocumento GestionTipoDocumento = new CatalogoTipoDocumento();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        [HttpPost]
        [Route("api/TalentoHumano/IngresoPersona")]
        public object IngresoPersona(PersonaEntidad PersonaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PersonaEntidad.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                if (PersonaEntidad.NumeroDocumento == null || string.IsNullOrEmpty(PersonaEntidad.NumeroDocumento.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el numero de documento de la persona";
                }
                else if(PersonaEntidad.ApellidoMaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoMaterno.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el apellido materno";
                }
                else if (PersonaEntidad.ApellidoPaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoPaterno.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el apellido paterno";
                }
                else if (PersonaEntidad.PrimerNombre == null || string.IsNullOrEmpty(PersonaEntidad.PrimerNombre.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta primer nombre";
                }
                else if (PersonaEntidad.SegundoNombre == null || string.IsNullOrEmpty(PersonaEntidad.SegundoNombre.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el segun nombre";
                }
                else if (PersonaEntidad.IdTipoDocumento == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoDocumento.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el id del tipo de documento";
                }
                else if (PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el id de la parroquia";
                }
                else if (PersonaEntidad.AsignacionPersonaComunidad.Referencia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Referencia.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta la referencia";
                }
                else if (PersonaEntidad.ListaTelefono[0].Numero == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].Numero.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].Numero == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].Numero.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el segundo numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese id tipo telefono del primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese id tipo telefono del segundo numero de telefono";
                }
                else
                {
                    //if (PersonaEntidad.ListaCorreo != null)
                    //{
                    //    if (PersonaEntidad.ListaCorreo[0].CorreoValor == null || string.IsNullOrEmpty(PersonaEntidad.ListaCorreo[0].CorreoValor))
                    //    {
                    //        codigo = "418";
                    //        mensaje = "Ingrese el correo";
                    //        objeto = new { codigo, mensaje };
                    //        return objeto;
                    //    }
                    //}
                    PersonaEntidad.IdTipoDocumento = Seguridad.DesEncriptar(PersonaEntidad.IdTipoDocumento);
                    PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono);
                    PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono);
                    PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia = Seguridad.DesEncriptar(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia);
                    TipoDocumento DatoTipoDocumento = new TipoDocumento();
                    DatoTipoDocumento = GestionTipoDocumento.ListarTiposDocumentosPorId(int.Parse(PersonaEntidad.IdTipoDocumento)).FirstOrDefault();
                    if (DatoTipoDocumento == null)
                    {
                        codigo = "418";
                        mensaje = "El tipo de documento que quiere asignar no existe";
                    }
                    else
                    {
                        PersonaEntidad DatoPersona = new PersonaEntidad();
                        DatoPersona = GestionPersona.ConsultarPersonaPorIdentificacion(PersonaEntidad.NumeroDocumento.Trim()).FirstOrDefault();
                        if (DatoPersona == null)
                        {
                            DatoPersona = new PersonaEntidad();
                            DatoPersona = GestionPersona.IngresarPersona(PersonaEntidad);
                            if (DatoPersona.IdPersona == null || string.IsNullOrEmpty(DatoPersona.IdPersona))
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar guardar la persona";
                            }
                            else
                            {
                                respuesta = DatoPersona;
                                mensaje = "EXITO";
                                codigo = "200";
                                objeto = new { codigo, mensaje, respuesta };
                                return objeto;
                            }
                        }
                        else
                        {
                            codigo = "418";
                            mensaje = "Ya existe una persona con el mismo numero de identificación";
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
        [Route("api/TalentoHumano/EliminarPersona")]
        public object EliminarPersona(PersonaEntidad PersonaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PersonaEntidad.encriptada, _claveDelete.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _claveDelete.Descripcion)
                //{
                if (PersonaEntidad.IdPersona == null || string.IsNullOrEmpty(PersonaEntidad.IdPersona.Trim()))
                {
                    codigo = "400";
                    mensaje = "Falta el id de la persona a eliminar";
                }
                else
                {
                    PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                    PersonaEntidad DatoPersona = new PersonaEntidad();
                    DatoPersona = GestionPersona.ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                    if (DatoPersona == null)
                    {
                        codigo = "418";
                        mensaje = "La persona que intenta eliminar no existe";
                    }
                    else
                    {
                        if (DatoPersona.IdUsuario == "1")
                        {
                            if (GestionPersona.EliminarPersona(int.Parse(PersonaEntidad.IdPersona)) == true)
                            {
                                mensaje = "EXITO";
                                codigo = "200";
                            }
                            else
                            {
                                codigo = "500";
                                mensaje = "Ocurrio un error al intentar eliminar la persona";
                            }
                        }
                        else
                        {
                            codigo = "418";
                            mensaje = "No puede eliminar esta persona porque ya ha ejercido actividad dentro del sistema";
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
        [Route("api/TalentoHumano/ActualizarPersona")]
        public object ActualizarPersona(PersonaEntidad PersonaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PersonaEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (PersonaEntidad.IdPersona == null || string.IsNullOrEmpty(PersonaEntidad.IdPersona.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el numero de documento de la persona";
                }
                else if (PersonaEntidad.ApellidoMaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoMaterno.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el apellido materno";
                }
                else if (PersonaEntidad.ApellidoPaterno == null || string.IsNullOrEmpty(PersonaEntidad.ApellidoPaterno.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el apellido paterno";
                }
                else if (PersonaEntidad.PrimerNombre == null || string.IsNullOrEmpty(PersonaEntidad.PrimerNombre.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta primer nombre";
                }
                else if (PersonaEntidad.SegundoNombre == null || string.IsNullOrEmpty(PersonaEntidad.SegundoNombre.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el segun nombre";
                }
                else if (PersonaEntidad.IdTipoDocumento == null || string.IsNullOrEmpty(PersonaEntidad.IdTipoDocumento.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el id del tipo de documento";
                }
                else if (PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el id de la parroquia";
                }
                else if (PersonaEntidad.AsignacionPersonaComunidad.Referencia == null || string.IsNullOrEmpty(PersonaEntidad.AsignacionPersonaComunidad.Referencia.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta la referencia";
                }
                else if (PersonaEntidad.ListaTelefono[0].IdTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].IdTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].IdTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].IdTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id segundo numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[0].Numero == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].Numero.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].Numero == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].Numero.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el segundo numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese id tipo telefono del primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese id tipo telefono del segundo numero de telefono";
                }
                else
                {
                    PersonaEntidad.IdTipoDocumento = Seguridad.DesEncriptar(PersonaEntidad.IdTipoDocumento);
                    TipoDocumento DatoTipoDocumento = new TipoDocumento();
                    DatoTipoDocumento = GestionTipoDocumento.ListarTiposDocumentosPorId(int.Parse(PersonaEntidad.IdTipoDocumento)).FirstOrDefault();
                    if (DatoTipoDocumento == null)
                    {
                        codigo = "418";
                        mensaje = "El tipo de documento que quiere asignar no existe";
                    }
                    else
                    {
                        PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                        PersonaEntidad DatoPersona = new PersonaEntidad();
                        DatoPersona = GestionPersona.ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                        if (DatoPersona == null)
                        {
                            codigo = "418";
                            mensaje = "La persona que intenta actualizar no existe";
                        }
                        else
                        {
                            PersonaEntidad.ListaTelefono[0].IdTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[0].IdTelefono);
                            PersonaEntidad.ListaTelefono[1].IdTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[1].IdTelefono);
                            List<Telefono> ListaTelefono = new List<Telefono>();
                            ListaTelefono = GestionPersona.ConsultarTelefonoPorPersona(int.Parse(PersonaEntidad.IdPersona));
                            if (ListaTelefono.Where(p => Seguridad.DesEncriptar(p.IdTelefono) == PersonaEntidad.ListaTelefono[0].IdTelefono).FirstOrDefault() == null)
                            {
                                codigo = "418";
                                mensaje = "No se a encontrado el primer telefono a modificar";
                            }else if (ListaTelefono.Where(p => Seguridad.DesEncriptar(p.IdTelefono) == PersonaEntidad.ListaTelefono[1].IdTelefono).FirstOrDefault() == null)
                            {
                                codigo = "418";
                                mensaje = "No se a encontrado el segundo telefono a modificar";
                            }
                            else
                            {
                                PersonaEntidad DatoPersonaAModificar = new PersonaEntidad();
                                DatoPersona.PrimerNombre = PersonaEntidad.PrimerNombre;
                                DatoPersona.SegundoNombre = PersonaEntidad.SegundoNombre;
                                DatoPersona.ApellidoMaterno = PersonaEntidad.ApellidoMaterno;
                                DatoPersona.ApellidoPaterno = PersonaEntidad.ApellidoPaterno;


                                DatoPersona.IdPersona = Seguridad.DesEncriptar(DatoPersona.IdPersona);
                                DatoPersona.IdTipoDocumento = PersonaEntidad.IdTipoDocumento;
                                DatoPersona.ListaTelefono[1].Numero = PersonaEntidad.ListaTelefono[1].Numero;
                                DatoPersona.ListaTelefono[0].Numero = PersonaEntidad.ListaTelefono[0].Numero;
                                DatoPersona.ListaTelefono[0].IdTelefono = PersonaEntidad.ListaTelefono[0].IdTelefono;
                                DatoPersona.ListaTelefono[1].IdTelefono = PersonaEntidad.ListaTelefono[1].IdTelefono;

                                PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono);
                                PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono);
                                DatoPersona.ListaTelefono[1].TipoTelefono.IdTipoTelefono = PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono;
                                DatoPersona.ListaTelefono[0].TipoTelefono.IdTipoTelefono = PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono;

                                PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia = Seguridad.DesEncriptar(PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia);
                                if (Seguridad.DesEncriptar(DatoPersona.AsignacionPersonaParroquia.FirstOrDefault().Parroquia.IdParroquia) == PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia)
                                {
                                    DatoPersona.AsignacionPersonaComunidad = new AsignacionPersonaParroquia()
                                    {
                                        IdAsignacionPC = Seguridad.DesEncriptar(DatoPersona.AsignacionPersonaParroquia.FirstOrDefault().IdAsignacionPC),
                                        Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia,
                                        Estado = false,
                                        Parroquia = new Parroquia()
                                        {
                                            IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia,
                                        }
                                    };
                                }
                                else
                                {
                                    DatoPersona.AsignacionPersonaComunidad = new AsignacionPersonaParroquia()
                                    {
                                        Referencia = PersonaEntidad.AsignacionPersonaComunidad.Referencia,
                                        Estado = true,
                                        Parroquia = new Parroquia()
                                        {
                                            IdParroquia = PersonaEntidad.AsignacionPersonaComunidad.Parroquia.IdParroquia,
                                        }
                                    };
                                }

                                if (PersonaEntidad.Correo!=null)
                                {
                                    DatoPersona.Correo = PersonaEntidad.Correo;
                                }
                                DatoPersonaAModificar = GestionPersona.ModificarPersona(DatoPersona);
                                if (DatoPersonaAModificar.IdUsuario == null)
                                {
                                    codigo = "500";
                                    mensaje = "Ocurrio un error al tratar de modificar la persona";
                                    respuesta = DatoPersonaAModificar;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                                else
                                {
                                    codigo = "200";
                                    mensaje = "EXITO";
                                    respuesta = DatoPersonaAModificar;
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
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
        [Route("api/TalentoHumano/BuscarPersona")]
        public object BuscarDatosPersona(PersonaEntidad PersonaEntidad)
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
                string ClaveGetEncripBD = p.desencriptar(PersonaEntidad.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                    mensaje = "EXITO";
                    codigo = "200";
                    PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                    respuesta = GestionUsuarios.FiltrarPersona(int.Parse(PersonaEntidad.IdPersona));
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
        [Route("api/TalentoHumano/ConsultarPersonasDependeDeTipoUsuario")]
        public object ConsultarPersonasDependeDeTipoUsuario(TipoUsuario TipoUsuario)
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
                string ClaveGetEncripBD = p.desencriptar(TipoUsuario.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                if (TipoUsuario.Identificacion == null || string.IsNullOrEmpty(TipoUsuario.Identificacion))
                {
                    mensaje = "Ingrese El ID TIPO USUARIO";
                    codigo = "418";
                    objeto = new { codigo, mensaje};
                    return objeto;
                }
                else
                {
                    mensaje = "EXITO";
                    codigo = "200";
                    respuesta = GestionPersona.ListaPersonasDependiendoDeTipoUsuario(int.Parse(TipoUsuario.Identificacion));
                    objeto = new { codigo, mensaje, respuesta };
                    return objeto;
                }
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
        [Route("api/TalentoHumano/ActualizarTelefonoCorreo")]
        public object ActualizarTelefonoCorreo(PersonaEntidad PersonaEntidad)
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
                string ClavePutEncripBD = p.desencriptar(PersonaEntidad.encriptada, _clavePut.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePut.Descripcion)
                //{
                if (PersonaEntidad.IdPersona == null || string.IsNullOrEmpty(PersonaEntidad.IdPersona.Trim()))
                {
                    codigo = "418";
                    mensaje = "Falta el numero de documento de la persona";
                }
                else if (PersonaEntidad.ListaTelefono[0].IdTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].IdTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].IdTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].IdTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el id segundo numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[0].Numero == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].Numero.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].Numero == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].Numero.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el segundo numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese id tipo telefono del primer numero de telefono";
                }
                else if (PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono == null || string.IsNullOrEmpty(PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese id tipo telefono del segundo numero de telefono";
                }
                else
                {
                    PersonaEntidad.IdPersona = Seguridad.DesEncriptar(PersonaEntidad.IdPersona);
                    PersonaEntidad DatoPersona = new PersonaEntidad();
                    DatoPersona = GestionPersona.ConsultarPersonaPorId(int.Parse(PersonaEntidad.IdPersona)).FirstOrDefault();
                    if (DatoPersona == null)
                    {
                        codigo = "418";
                        mensaje = "La persona que intenta actualizar no existe";
                    }
                    else
                    {
                        PersonaEntidad.ListaTelefono[0].IdTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[0].IdTelefono);
                        PersonaEntidad.ListaTelefono[1].IdTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[1].IdTelefono);
                        PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono);
                        PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono = Seguridad.DesEncriptar(PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono);
                        List<Telefono> ListaTelefono = new List<Telefono>();
                        ListaTelefono = GestionPersona.ConsultarTelefonoPorPersona(int.Parse(PersonaEntidad.IdPersona));
                        if (ListaTelefono.Where(p => Seguridad.DesEncriptar(p.IdTelefono) == PersonaEntidad.ListaTelefono[0].IdTelefono).FirstOrDefault() == null)
                        {
                            codigo = "418";
                            mensaje = "No se a encontrado el primer telefono a modificar";
                        }
                        else if (ListaTelefono.Where(p => Seguridad.DesEncriptar(p.IdTelefono) == PersonaEntidad.ListaTelefono[1].IdTelefono).FirstOrDefault() == null)
                        {
                            codigo = "418";
                            mensaje = "No se a encontrado el segundo telefono a modificar";
                        }
                        else
                        {
                            PersonaEntidad DatoPersonaAModificar = new PersonaEntidad();
                            DatoPersona.IdPersona = Seguridad.DesEncriptar(DatoPersona.IdPersona);
                            DatoPersona.ListaTelefono[1].Numero = PersonaEntidad.ListaTelefono[1].Numero;
                            DatoPersona.ListaTelefono[0].Numero = PersonaEntidad.ListaTelefono[0].Numero;
                            DatoPersona.ListaTelefono[0].IdTelefono = PersonaEntidad.ListaTelefono[0].IdTelefono;
                            DatoPersona.ListaTelefono[1].IdTelefono = PersonaEntidad.ListaTelefono[1].IdTelefono;

                            DatoPersona.ListaTelefono[1].TipoTelefono.IdTipoTelefono = PersonaEntidad.ListaTelefono[1].TipoTelefono.IdTipoTelefono;
                            DatoPersona.ListaTelefono[0].TipoTelefono.IdTipoTelefono = PersonaEntidad.ListaTelefono[0].TipoTelefono.IdTipoTelefono;

                            Telefono DatoTelefono = new Telefono();
                            DatoTelefono = GestionPersona.ModificarTelefono(new TelefonoEntidad() {IdTelefono = DatoPersona.ListaTelefono[0].IdTelefono,IdPersona = DatoPersona.IdPersona ,Numero = DatoPersona.ListaTelefono[0].Numero ,IdTipoTelefono = DatoPersona.ListaTelefono[0].TipoTelefono.IdTipoTelefono });
                            DatoPersona.ListaTelefono[0] = DatoTelefono;
                            Telefono DatoTelefono1 = new Telefono();
                            DatoTelefono1 = GestionPersona.ModificarTelefono(new TelefonoEntidad() { IdTelefono = DatoPersona.ListaTelefono[1].IdTelefono, IdPersona = DatoPersona.IdPersona, Numero = DatoPersona.ListaTelefono[1].Numero, IdTipoTelefono = DatoPersona.ListaTelefono[1].TipoTelefono.IdTipoTelefono });
                            DatoPersona.ListaTelefono[1] = DatoTelefono1;
                            object InformacionActualizada = new object();
                            object Telefonos = new object();
                            Telefonos = DatoPersona.ListaTelefono;
                            object Correo = new object();
                            if (PersonaEntidad.Correo != null || !string.IsNullOrEmpty(PersonaEntidad.Correo.Trim()) || PersonaEntidad.Correo.Trim().ToUpper() != "NULL")
                            {
                                Correo DatoCorreo = new Correo();
                                if (DatoPersona.ListaCorreo.FirstOrDefault()==null)
                                {
                                    DatoCorreo = GestionPersona.IngresoCorreo(new Correo() { IdPersona = DatoPersona.IdPersona,CorreoValor = PersonaEntidad.Correo });
                                }
                                else
                                {
                                    DatoCorreo = GestionPersona.ModificarCorreo(new Correo() {IdCorreo = Seguridad.DesEncriptar(DatoPersona.ListaCorreo.FirstOrDefault().IdCorreo), IdPersona = DatoPersona.IdPersona, CorreoValor = PersonaEntidad.Correo });
                                }
                                Correo = DatoCorreo;
                                codigo = "200";
                                mensaje = "EXITO";
                                InformacionActualizada = new { Correo, Telefonos };
                                respuesta = InformacionActualizada;
                                objeto = new { codigo, mensaje,respuesta};
                                return objeto;
                            }
                            else
                            {
                                codigo = "200";
                                mensaje = "EXITO";
                                InformacionActualizada = new {Telefonos };
                                respuesta = InformacionActualizada;
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
