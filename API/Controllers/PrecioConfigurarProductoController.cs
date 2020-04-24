﻿using System;
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
    public class PrecioConfigurarProductoController : ApiController
    {
        CatalogoCabeceraFactura GestionCabeceraFactura = new CatalogoCabeceraFactura();
        CatalogoPrecioConfigurarProducto GestionPrecioConfigurarProducto = new CatalogoPrecioConfigurarProducto();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();


        [HttpPost]
        [Route("api/Inventario/IngresoPrecioConfigurarProducto")]
        public object IngresoPrecioConfigurarProducto(PrecioConfigurarProducto PrecioConfigurarProducto)
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
                string ClavePutEncripBD = p.desencriptar(PrecioConfigurarProducto.encriptada, _clavePost.Clave.Descripcion.Trim());
                //if (ClavePutEncripBD == _clavePost.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";
                PrecioConfigurarProducto.IdConfigurarProducto = Seguridad.DesEncriptar(PrecioConfigurarProducto.IdConfigurarProducto);
                respuesta = GestionPrecioConfigurarProducto.InsertarPrecioConfigurarProducto(PrecioConfigurarProducto);
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
        [Route("api/Inventario/BuscarPrecioConfigurarProducto")]
        public object BuscarPrecioConfigurarProducto(AsignarProductoLote AsignarProductoLote)
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
                string ClaveGetEncripBD = p.desencriptar(AsignarProductoLote.encriptada, _claveGet.Clave.Descripcion.Trim());
                //if (ClaveGetEncripBD == _claveGet.Descripcion)
                //{
                mensaje = "EXITO";
                codigo = "200";

                AsignarProductoLote.IdAsignarProductoLote = Seguridad.DesEncriptar(AsignarProductoLote.IdAsignarProductoLote);
                respuesta = GestionCabeceraFactura.BuscarPrecioDeUnProducto(AsignarProductoLote);
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
