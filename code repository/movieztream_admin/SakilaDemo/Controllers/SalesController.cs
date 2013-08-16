using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecetera.AppDynamics.Models;

using PagedList;
using Ecetera.AppDynamics.VideoRentalService;

namespace Ecetera.AppDynamics.Controllers
{
    public class SalesController : Controller
    {
        private RentalServiceClient serviceProxy;

        public SalesController()
        {
            serviceProxy = new RentalServiceClient();
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var sales = from s in serviceProxy.GetSalesByCategory()
                           select s;            

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(sales.ToPagedList(pageNumber, pageSize));
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}