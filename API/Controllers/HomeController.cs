using Rotativa.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult Index1()
        {
            //return new Rotativa.MVC.PartialViewAsPdf("Index")
            //{
            //    RotativaOptions = new Rotativa.Core.DriverOptions()
            //    {
            //        PageOrientation = Orientation.Landscape,
            //        PageSize = Rotativa.Core.Options.Size.A5,
            //        IsLowQuality = true
            //    }
            //};
            ViewBag.Title = "Home Page";
            return new Rotativa.MVC.ActionAsPdf("Index");
            //return View();
        }

    }
}
