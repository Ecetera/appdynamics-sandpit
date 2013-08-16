using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecetera.AppDynamics.ViewModels
{
    public class PaymentData
    {
        public decimal Amount{ get; set; }
        public DateTime PaymentDate{ get; set; }
        public int ID{ get; set; }
        public int customerID { get; set; }
        public int rentalID { get; set; }
        public int staffID { get; set; }
        public string FilmTitle { get; set; }

    }
}