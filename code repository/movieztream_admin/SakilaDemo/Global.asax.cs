using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;

using Ecetera.AppDynamics.Models;
using System.ComponentModel;

namespace Ecetera.AppDynamics
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        static Dictionary<string, object> cache = new Dictionary<string, object>();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            
            routes.MapRoute(
                name: "Payment",
                url: "Payment/{action}/{id}", // URL with parameters
                defaults:new
                {
                    controller = "Rental",
                    action ="Index",
                    id = UrlParameter.Optional
                }                                
            );            

            routes.MapRoute(
                name: "Rental",
                url: "Rentals/Detail/{id}", // URL with parameters
                defaults: new
                {
                    controller = "Rentals",
                    action = "Detail",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "AllPayments",
                url: "Rentals/Payments/{id}", // URL with parameters
                defaults: new
                {
                    controller = "Rentals",
                    action = "Detail",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Refunds",
                url: "Payment/Refund/{id}", // URL with parameters
                defaults: new
                {
                    controller = "Payment",
                    action = "Refund",
                    id = UrlParameter.Optional
                }
            );
        }

        

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Code that runs on application startup
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.WorkerReportsProgress = false;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted +=
                   new RunWorkerCompletedEventHandler(WorkerCompleted);

            //Add this BackgroundWorker object instance to the cache (custom cache implementation)
            //so it can be cleared when the Application_End event fires.


            cache.Add("BackgroundWorker", worker);

            // Calling the DoWork Method Asynchronously
            worker.RunWorkerAsync(); //we can also pass parameters to the async method....
        }
        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
            CCTerminal cc = new CCTerminal();
            cc.TerminalValidate("4444333322221111", "1212", "123", 1.99);
           
        }

        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
               
                System.Threading.Thread.Sleep(30000);
                worker.RunWorkerAsync();
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            //If background worker process is running then clean up that object.
            if (cache.ContainsKey("BackgroundWorker"))
            {
                BackgroundWorker worker = (BackgroundWorker)cache["BackgroundWorker"];
                if (worker != null)
                    worker.CancelAsync();
            }
        }
 
    }
}