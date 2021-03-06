﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Credito;
using Negocio.Logica.Seguridad;
namespace API.Controllers
{
    public class VisitaController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        CatalogoVisita _GestionVisita = new CatalogoVisita();
        CatalogoAsignarTecnicoPersonaComunidad _GestionATPC = new CatalogoAsignarTecnicoPersonaComunidad();

        [HttpPost]
        [Route("api/Credito/IngresoVisita")]
        public object IngresoVisita(Visita _Visita)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Visita.encriptada == null || string.IsNullOrEmpty(_Visita.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Visita.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Visita == null)
                        {
                            mensaje = "Error el objeto que envio esta null";
                            codigo = "418";
                        }
                        else if (_Visita.IdAsignarTecnicoPersonaComunidad == null || string.IsNullOrEmpty(_Visita.IdAsignarTecnicoPersonaComunidad))
                        {
                            mensaje = "Ingrese el Asignar TU tecnico persona comunidad";
                            codigo = "418";
                        }
                        else if (_Visita.IdAsignarTU == null || string.IsNullOrEmpty(_Visita.IdAsignarTU))
                        {
                            mensaje = "Ingrese el id de la asignacion tu";
                            codigo = "418";
                        }
                        else if (_Visita.Observacion == null || string.IsNullOrEmpty(_Visita.Observacion))
                        {
                            mensaje = "Ingrese la observacion";
                            codigo = "418";
                        }
                        else
                        {
                            _Visita.IdAsignarTecnicoPersonaComunidad = Seguridad.DesEncriptar(_Visita.IdAsignarTecnicoPersonaComunidad);
                            _Visita.IdAsignarTU = Seguridad.DesEncriptar(_Visita.IdAsignarTU);
                            AsignarTecnicoPersonaComunidad _DatoAsignarTPC = new AsignarTecnicoPersonaComunidad();
                            _DatoAsignarTPC = _GestionATPC.ConsultarPorEstadoAsignarTPC(int.Parse(_Visita.IdAsignarTecnicoPersonaComunidad)).FirstOrDefault();
                            if (_DatoAsignarTPC != null)
                            {
                                Visita _DatoVisita = new Visita();
                                _DatoVisita = _GestionVisita.InsertarVisita(_Visita);
                                if (_DatoVisita.IdVisita == null)
                                {
                                    mensaje = "Error al ingresar la visita";
                                    codigo = "418";
                                }
                                else
                                {
                                    respuesta = _DatoVisita;
                                    mensaje = "EXITO";
                                    codigo = "200";
                                    objeto = new { codigo, mensaje, respuesta };
                                    return objeto;
                                }
                            }
                            else
                            {
                                mensaje = "O NO ESTA ACTIVADO O NO EXISTE EL TECNICO AL QUE QUIERE ANADIR LA OBSERVACION";
                                codigo = "418";
                            }
                        }
                    }
                }
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
        [HttpPost]
        [Route("api/Credito/ConsutlarVisita")]
        public object ConsutlarVisita(Visita _Visita)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Visita.encriptada == null || string.IsNullOrEmpty(_Visita.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Visita.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Visita.IdAsignarTecnicoPersonaComunidad == null || string.IsNullOrEmpty(_Visita.IdAsignarTecnicoPersonaComunidad))
                        {
                            mensaje = "Ingrese la id asignartecnicopersonacomunidad";
                            codigo = "418";
                        }
                        else
                        {
                            mensaje = "EXITO";
                            codigo = "200";
                            _Visita.IdAsignarTecnicoPersonaComunidad = Seguridad.DesEncriptar(_Visita.IdAsignarTecnicoPersonaComunidad);
                            respuesta = _GestionVisita.ConsultarHistorialDeVisita(int.Parse(_Visita.IdAsignarTecnicoPersonaComunidad));
                            objeto = new { codigo, mensaje, respuesta };
                            return objeto;
                        }
                    }
                }
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
        [HttpPost]
        [Route("api/Credito/ModificarVisita")]
        public object ModificarVisita(Visita _Visita)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Visita.encriptada == null || string.IsNullOrEmpty(_Visita.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Visita.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Visita.IdVisita == null || string.IsNullOrEmpty(_Visita.IdVisita))
                        {
                            mensaje = "Ingrese el id de la visita";
                            codigo = "418";
                        }
                        else if (_Visita.IdAsignarTU == null || string.IsNullOrEmpty(_Visita.IdAsignarTU))
                        {
                            mensaje = "Ingrese el id de la asignacion tu";
                            codigo = "418";
                        }
                        else if (_Visita.Observacion == null || string.IsNullOrEmpty(_Visita.Observacion))
                        {
                            mensaje = "Ingrese la observacion";
                            codigo = "418";
                        }
                        else
                        {
                            Visita _DatoVisita = new Visita();
                            _Visita.IdVisita = Seguridad.DesEncriptar(_Visita.IdVisita);
                            _Visita.IdAsignarTU = Seguridad.DesEncriptar(_Visita.IdAsignarTU);
                            _DatoVisita = _GestionVisita.ConsultarVisitaPorId(int.Parse(_Visita.IdVisita)).FirstOrDefault();
                            if (_DatoVisita == null)
                            {
                                mensaje = "La visita que intenta modificar no existe";
                                codigo = "418";
                            }
                            else
                            {
                                AsignarTecnicoPersonaComunidad _DatoAsignarTPC = new AsignarTecnicoPersonaComunidad();
                                _DatoVisita.IdAsignarTecnicoPersonaComunidad = Seguridad.DesEncriptar(_DatoVisita.IdAsignarTecnicoPersonaComunidad);
                                _DatoAsignarTPC = _GestionATPC.ConsultarPorEstadoAsignarTPC(int.Parse(_DatoVisita.IdAsignarTecnicoPersonaComunidad)).FirstOrDefault();
                                if (_DatoAsignarTPC != null)
                                {
                                    Visita _DatoVisitaModificar = new Visita();
                                    _DatoVisitaModificar = _GestionVisita.ModificarVisita(_Visita);
                                    if (_DatoVisitaModificar.IdVisita == null)
                                    {
                                        mensaje = "Error al modificar la visita";
                                        codigo = "418";
                                    }
                                    else
                                    {
                                        respuesta = _DatoVisitaModificar;
                                        mensaje = "EXITO";
                                        codigo = "200";
                                        objeto = new { codigo, mensaje, respuesta };
                                        return objeto;
                                    }
                                }
                                else
                                {
                                    mensaje = "O NO ESTA ACTIVADO O NO EXISTE EL TECNICO AL QUE QUIERE MODIFICAR LA OBSERVACION";
                                    codigo = "418";
                                }
                            }
                        }
                    }
                }
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
        [Route("api/Credito/EliminarVisita")]
        public object EliminarVisita(Visita _Visita)
        {
            object objeto = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                if (_Visita.encriptada == null || string.IsNullOrEmpty(_Visita.encriptada.Trim()))
                {
                    codigo = "418";
                    mensaje = "Ingrese el token";
                }
                else
                {
                    if (Seguridad.ConsultarUsuarioPorToken(_Visita.encriptada).FirstOrDefault() == null)
                    {
                        codigo = "403";
                        mensaje = "No tiene los permisos para poder realizar dicha consulta";
                    }
                    else
                    {
                        if (_Visita.IdVisita == null || string.IsNullOrEmpty(_Visita.IdVisita))
                        {
                            mensaje = "Ingrese el id de la visita";
                            codigo = "418";
                        }
                        else
                        {
                            Visita _DatoVisita = new Visita();
                            _Visita.IdVisita = Seguridad.DesEncriptar(_Visita.IdVisita);
                            _DatoVisita = _GestionVisita.ConsultarVisitaPorId(int.Parse(_Visita.IdVisita)).FirstOrDefault();
                            if (_DatoVisita == null)
                            {
                                mensaje = "La visita que quiere eliminar no existe";
                                codigo = "418";
                            }
                            else
                            {
                                AsignarTecnicoPersonaComunidad _DatoAsignarTPC = new AsignarTecnicoPersonaComunidad();
                                _DatoVisita.IdAsignarTecnicoPersonaComunidad = Seguridad.DesEncriptar(_DatoVisita.IdAsignarTecnicoPersonaComunidad);
                                _DatoAsignarTPC = _GestionATPC.ConsultarPorEstadoAsignarTPC(int.Parse(_DatoVisita.IdAsignarTecnicoPersonaComunidad)).FirstOrDefault();
                                if (_DatoAsignarTPC != null)
                                {
                                    if (_GestionVisita.EliminarVisita(int.Parse(_Visita.IdVisita)) == true)
                                    {
                                        mensaje = "EXITO";
                                        codigo = "200";
                                    }
                                    else
                                    {
                                        mensaje = "Ocurrio un error al eliminar la visita";
                                        codigo = "418";
                                    }
                                }
                                else
                                {
                                    mensaje = "No se puede eliminar porque el seguimiento ya a sido finalizado";
                                    codigo = "418";
                                }
                            }
                        }

                    }
                }
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
