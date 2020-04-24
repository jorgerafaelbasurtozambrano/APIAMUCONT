using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Negocio;
using Negocio.Logica;
using Negocio.Logica.Seguridad;
using Negocio.Entidades;

namespace API.Controllers
{
    public class SeguridadController : ApiController
    {
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        Prueba p = new Prueba();

        [HttpPost]
        [Route("api/Seguridad/ConsultarClave")]
        public object ConsultarClave(Clave Clave)
        {
            object objeto = new object();
            object informacion = new object();
            object respuesta = new object();
            string mensaje = "";
            string codigo = "";
            try
            {
                mensaje = "EXITO";
                codigo = "200";
                var ListaClaves=GestionSeguridad.ListarTokens().Where(c=>c.Estado == true).ToList();
                informacion = ListaClaves;
                if (ListaClaves.Count != 4)
                {
                    mensaje = "ERROR";
                }
                else
                {
                    var _claveGet = ListaClaves.Where(c => c.Identificador == 4).FirstOrDefault();
                    var _clavePost = ListaClaves.Where(c => c.Identificador == 1).FirstOrDefault();
                    var _clavePut = ListaClaves.Where(c => c.Identificador == 2).FirstOrDefault();
                    var _claveDelete = ListaClaves.Where(c => c.Identificador == 3).FirstOrDefault();

                    string ClaveGetEncrip = p.encriptar(_claveGet.Descripcion.Trim(), _claveGet.Clave.Descripcion.Trim());
                    string ClavePostEncrip = p.encriptar(_clavePost.Descripcion.Trim(), _clavePost.Clave.Descripcion.Trim());
                    string ClavePutEncrip = p.encriptar(_clavePut.Descripcion.Trim(), _clavePut.Clave.Descripcion.Trim());
                    string ClaveDeleteEncrip = p.encriptar(_claveDelete.Descripcion.Trim(), _claveDelete.Clave.Descripcion.Trim());
                    respuesta = new { ClaveGetEncrip, ClavePostEncrip, ClavePutEncrip, ClaveDeleteEncrip };
                }
            }
            catch (Exception e)
            {
                mensaje = "ERROR";
                codigo = "418";
                respuesta = "ERROR";
            }

            objeto = new { mensaje, codigo, respuesta ,informacion};
            return objeto;
        }

    }
}
