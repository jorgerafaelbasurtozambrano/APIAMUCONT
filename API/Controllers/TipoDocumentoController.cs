﻿using System;
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
    public class TipoDocumentoController : ApiController
    {
        CatalogoTipoDocumento GestionTipoDocumento = new CatalogoTipoDocumento();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();

        [HttpPost]
        [Route("api/TalentoHumano/ConsultarTipoDocumento")]
        public object ConsultarTipoDocumento([FromBody] Tokens Tokens)
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
                    respuesta = GestionTipoDocumento.ListarTiposDocumentos();
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
                objeto = new { codigo,mensaje };
                return objeto;
            }
        }
    }
}