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
using System.Text;

namespace Ecetera.AppDynamics.Controllers
{
    public class RewardController : Controller
    {
        private VideoRentalService.RentalServiceClient serviceProxy;

        public RewardController()
        {
            serviceProxy = new RentalServiceClient();
        }

        public ViewResult Index(string sortOrder, string currentFilterMMP, string currentFilterMDP, string minMonthlyPurchases, string minDollarPurchases, int? page)
        {
            int cr=0;
            int pageSize = 10;
            if (Request.HttpMethod == "GET")
            {
                minMonthlyPurchases = currentFilterMMP;
                minDollarPurchases = currentFilterMDP;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilterMMP = minMonthlyPurchases;
            ViewBag.CurrentFilterMDP = minDollarPurchases;

            int pageNumber = (page ?? 1);

            if (minMonthlyPurchases != null && minDollarPurchases != null)
            {
                var Rewards = from s in serviceProxy.GetRewardsReport(sbyte.Parse(minMonthlyPurchases), decimal.Parse(minDollarPurchases), cr) select s;

                return View(Rewards.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<SCustomer> sc = new List<SCustomer>();
                return View(sc.ToPagedList(1, 1));
            }
            
        }        

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}