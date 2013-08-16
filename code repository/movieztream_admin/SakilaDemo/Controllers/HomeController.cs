using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecetera.AppDynamics.Models;
using Ecetera.AppDynamics.ViewModels;

namespace Ecetera.AppDynamics.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to AppDynamics .NET monitoring demo (by Ecetera)!";
            return View();
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
