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
    public class CustomerController : Controller
    {
        private VideoRentalService.RentalServiceClient serviceProxy;

        public CustomerController()
        {
            serviceProxy = new RentalServiceClient();
        }

        //
        // GET: /Customer/

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

            var customers = from s in serviceProxy.GetCustomersByText(searchString)
                           select s;            

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(customers.ToPagedList(pageNumber, pageSize));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Payments(int customerid)
        {
            ViewBag.CurrentFilter = customerid;
            var Payments = from s in serviceProxy.GetPayments(customerid)
                           select s;

            SCustomer co = serviceProxy.GetCustomer(customerid);
            ViewData["customername"] = co.first_name + " " + co.last_name;

            int pageSize = 1000;
            int pageNumber = 1;
            return View(Payments.ToPagedList(pageNumber, pageSize));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Rentals(int customerid)
        {
            ViewBag.CurrentFilter = customerid;
            var rentals = from r in serviceProxy.GetRentals(customerid)
                           select r;

            
            if (rentals.ToArray().Length == 0)
            {
                throw new Exception("no rentals found for this customer");
            }
            SCustomer co = serviceProxy.GetCustomer(customerid);
            ViewData["customername"] = co.first_name + " " + co.last_name;

            int pageSize = 1000;
            int pageNumber = 1;
            return View(rentals.ToPagedList(pageNumber, pageSize));
        }
        
        public ViewResult Details(int id)
        {
            SCustomer customer = serviceProxy.GetCustomer(id);
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}