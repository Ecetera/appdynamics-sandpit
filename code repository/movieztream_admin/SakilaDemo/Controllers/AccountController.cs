using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Ecetera.AppDynamics.Models;
using System.Security.Principal;
using System.Globalization;

namespace Ecetera.AppDynamics.Controllers
{
    public class AccountController : Controller
    {
        
        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = new AccountMembershipService();
            
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {

            return View();
        }

        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {

            if (!ValidateLogOn(userName, password))
            {
                ViewData["rememberMe"] = rememberMe;
                return View();
            }

            FormsAuthenticationTicket authTicket = new
                            FormsAuthenticationTicket(1, //version
                            userName, // user name
                            DateTime.Now,             //creation
                            DateTime.Now.AddMinutes(30), //Expiration
                            rememberMe, //Persistent
                            userName); //since Classic logins don't have a "Friendly Name"

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {

            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //public ActionResult Register()
        //{
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View();
        //}

        public ActionResult Search()
        {
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        #region Validation Methods

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }
            if (!MembershipService.ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }
            if (ModelState.IsValid) 
                ViewData["staffid"] = MembershipService.GetUserId();
            return ModelState.IsValid;
        }
        #endregion
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService
    {

        bool ValidateUser(string userName, string password);
        int  GetUserId();
    }

    public class AccountMembershipService : IMembershipService
    {
        private SakilaLoginProvider _provider;
        private int userID;
        public AccountMembershipService()
            : this(new SakilaLoginProvider())
        {
        }

        public AccountMembershipService(SakilaLoginProvider provider)
        {
            _provider = provider;
        }

        public bool ValidateUser(string userName, string password)
        {
            bool isValid = _provider.ValidateUser(userName, password, ref userID);
            return isValid;

        }

        public int GetUserId()
        {
            return userID;
        }


    }

    public class SakilaLoginProvider
    {
        private VideoRentalService.RentalServiceClient serviceProxy = new VideoRentalService.RentalServiceClient();
        public bool ValidateUser(string username, string password, ref int userID)
        {
            bool isValid = serviceProxy.Logon(username, password, ref userID);            
            return isValid;
        }
    }
}
