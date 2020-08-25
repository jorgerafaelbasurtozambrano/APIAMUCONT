using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Negocio;
using Negocio.Entidades;
using Negocio.Logica.Credito;
using Negocio.Logica.Seguridad;
using Newtonsoft.Json;
using Raven.Abstractions.Connection;
using Rotativa.MVC;

namespace API.Controllers
{
    public class ReportesController : ApiController
    {
        Prueba p = new Prueba();
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();
        CatalogoSeguridad GestionSeguridad = new CatalogoSeguridad();
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Reportes/ReportePrueba")]
        public ActionResult ConsutlarAbonoPorFactura(Abono _Abono)
        {

            return new Rotativa.MVC.ViewAsPdf("~/View/Home/Index");
            //var path = System.Web.HttpContext.Current.Server.MapPath("~/Models/Reporte/dato.pdf");
            //var stream = new FileStream(path, FileMode.Open);
            //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //result.Content.Headers.ContentDisposition.FileName = System.IO.Path.GetFileName(path);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //result.Content.Headers.ContentLength = stream.Length;
            //return result;


            //object objeto = new object();
            //object respuesta = new object();
            //string mensaje = "";
            //string codigo = "";
            //try
            //{
            //    if (_Abono.encriptada == null || string.IsNullOrEmpty(_Abono.encriptada.Trim()))
            //    {
            //        codigo = "418";
            //        mensaje = "Ingrese el token";
            //    }
            //    else
            //    {
            //        if (Seguridad.ConsultarUsuarioPorToken(_Abono.encriptada).FirstOrDefault() == null)
            //        {
            //            codigo = "403";
            //            mensaje = "No tiene los permisos para poder realizar dicha consulta";
            //        }
            //        else
            //        {
            //            //codigo = "200";
            //            //mensaje = "EXITO";
            //            //respuesta = HttpContext.Current.Request.PhysicalApplicationPath + @"Models\\Reporte\\dato.pdf";
            //            //PdfDocument pdf = new PdfDocument(new PdfWriter(respuesta.ToString()));
            //            //Document document = new Document(pdf);
            //            //String line = "Hello! Welcome to iTextPdf";
            //            //document.Add(new Paragraph(line));
            //            //document.Close();
            //            //objeto = new { codigo, mensaje, respuesta };

            //            //return Request.CreateResponse<object>(HttpStatusCode.OK, path);
            //        }
            //    }
            //    objeto = new { codigo, mensaje };
            //    return objeto;
            //}
            //catch (Exception e)
            //{
            //    mensaje = e.Message;
            //    codigo = "500";
            //    objeto = new { codigo, mensaje };
            //    return objeto;
            //}
        }
    }
}
