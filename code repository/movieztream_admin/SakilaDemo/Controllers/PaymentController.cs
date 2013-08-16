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
using Ecetera.AppDynamics.ViewModels;

namespace Ecetera.AppDynamics.Controllers
{
    public class PaymentController : Controller
    {
        private VideoRentalService.RentalServiceClient serviceProxy;

        public PaymentController()
        {
            serviceProxy = new RentalServiceClient();
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Rentals(int id)
        {
            ViewBag.CurrentFilter = id;
            SRental rental = serviceProxy.GetRentalByPayment(id);

            List<SRental> rentals = new List<SRental>();

            rentals.Add(rental);

            int pageSize = 1000;
            int pageNumber = 1;

            return View(rentals.ToPagedList(pageNumber, pageSize));
        }
        //
        // GET: /Payment/

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

            var Payments = from s in serviceProxy.GetPayments(int.Parse(searchString))
                           select s;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Payments.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Payment/Create

        public ActionResult Refund(int id)
        {
            SPayment payment = serviceProxy.GetPayment(id);

            PaymentData pd = new PaymentData();
            pd.Amount = payment.amount;
            pd.FilmTitle = payment.filmtitle;
            pd.ID = payment.rental_id.Value;
            pd.customerID = payment.customer_id;
            pd.rentalID = payment.rental_id.Value;
            pd.staffID = payment.staff_id;
            pd.PaymentDate = payment.payment_date;

            return View(pd);
        }

        [HttpPost]
        public ActionResult Refund(PaymentData payment)
        {
            RefundService.RefundServiceClient localServiceProxy = new RefundService.RefundServiceClient();
            int staffID = localServiceProxy.GetStaffID(HttpContext.User.Identity.Name);
            string info = localServiceProxy.RefundPayment(payment.ID, payment.customerID, staffID, payment.rentalID, -1 * (payment.Amount), payment.PaymentDate, payment.FilmTitle);

            //cause a redirectaction here
            return RedirectToAction("PaymentDone");
        }

        public ActionResult PaymentDone()
        {
            ViewBag.Message = "Payment refunded!";
            return View();
        }
    }
}