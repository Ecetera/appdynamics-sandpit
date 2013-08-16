using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;

using Ecetera.AppDynamics.Models;

namespace Ecetera.AppDynamics
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
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
        }
    }
}