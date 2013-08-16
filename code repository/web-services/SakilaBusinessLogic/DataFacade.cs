using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SakilaBusinessLogic
{
    public class DataFacade
    {
        public bool PayForMovie(byte staffid, byte customerid, byte rentalid, double paymentAmount)
        {
            payment payment = new payment();
            payment.amount = (decimal)paymentAmount;
            payment.customer_id = customerid;
            payment.staff_id = staffid;
            payment.rental_id = rentalid;            
            
            SakilaBusinessLogic.SakilaEntities dc = new SakilaEntities();
            dc.AddTopayments(payment);
            dc.SaveChanges();

            return true;
        }
    }
}
