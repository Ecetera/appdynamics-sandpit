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
    public class RentalsController : Controller
    {
        private VideoRentalService.RentalServiceClient serviceProxy;

        public RentalsController()
        {
            serviceProxy = new RentalServiceClient();
        }

        //
        // GET: /Customer/
        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Detail(int paymentid)
        {
            ViewBag.CurrentFilter = paymentid;
            SRental sr = serviceProxy.GetRentalByPayment(paymentid);

            //SCustomer co = serviceProxy.GetCustomer(customerid);
            //ViewData["customername"] = co.first_name + " " + co.last_name;

            List<SRental> rentals = new List<SRental>();

            rentals.Add(sr);

            int pageSize = 1000;
            int pageNumber = 1;
            return View(rentals.ToPagedList(pageNumber, pageSize));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Payments(int rentalid)
        {
            ViewBag.CurrentFilter = rentalid;
            var Payments = from s in serviceProxy.GetPaymentsByRental(rentalid)
                           select s;

            //SCustomer co = serviceProxy.GetCustomer(customerid);
            //ViewData["customername"] = co.first_name + " " + co.last_name;

            int pageSize = 1000;
            int pageNumber = 1;
            return View(Payments.ToPagedList(pageNumber, pageSize));
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}