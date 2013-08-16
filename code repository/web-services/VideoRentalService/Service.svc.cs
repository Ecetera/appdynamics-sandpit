using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;
using VideoRentalService.AppDBank;

namespace VideoRentalService
{
    //this service (SOAP) is going to call other services such as a bank service to take payment from credti or debit payment from credit card
    public class RentalService : IRentalService
    {
        private DataFacade df;
        private static readonly ILog log = LogManager.GetLogger(typeof(RentalService));

        public RentalService()
        {
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("DataFaceSimulationsettings", typeof(String));
            //class name to load
            string line;
            
            // Read the file and display it line by line.
            using (System.IO.StreamReader file = new System.IO.StreamReader(path))
            {
                line = file.ReadToEnd();
                file.Close();
            }
            SimulationManager.Simulator s = new SimulationManager.Simulator();
            
            //s.Check("DataFaceSimulationsettings: " + line);
            if (line=="DataFacade1")
            {
                df = new DataFacade1();
                //s.Check("DataFaceSimulationsettings: instantiated " + line);
            }
            else if (line == "DataFacade2")
            {
                df = new DataFacade2();
                //s.Check("DataFaceSimulationsettings: instantiated " + line);
            }
            else
            {
                throw new Exception("DataFacade not loaded, check the following appsetting in web.config -> 'DataFaceSimulationsettings'");
            }
        }


        public int ServiceCheck()
        {
            return 12;
        }

        public int HireMovie(int staffid, int customerid, int rentalid, double paymentAmount, string creditcardno, string expiry, string cvv)
        {
            int transactionID = -1;
            CreditCardServiceClient cc = new CreditCardServiceClient();
            transactionID = cc.Debit(creditcardno, paymentAmount);
            if (transactionID != -1)
            {
                df.PayForMovie((byte)staffid, (byte)customerid, (byte)rentalid, paymentAmount);
            }
            return transactionID;
        }

        public int Refund(int staffid, int customerid, double paymentAmount, string creditcardno, string expiry, string cvv)
        {
            return 1;
        }

        public int RefundT(int staffid, int customerid, double paymentAmount, string creditcardno, string expiry, string cvv)
        {
            //return string.Format("{0}:{1}:{2}:{3}:{4}:{5}",staffid, customerid, paymentAmount, creditcardno, expiry, cvv) ;                       
            int tID = -1;
            try
            {
                CreditCardServiceClient cc = new CreditCardServiceClient();
                tID = cc.Credit(creditcardno, paymentAmount);
                return tID;
            }
            catch (Exception e)
            {
            }
            return tID;
        }

        public SCustomer[] GetCustomersByText(string searchtext)
        {
            return df.GetCustomersByText(searchtext);
        }

        public SCustomer GetCustomer(int id)
        {
            return df.GetCustomer(id);
        }

        public SCustomer[] GetCustomers()
        {
            return df.GetCustomers();
        }

        public SRental[] GetRentals(int customerid)
        {
            return df.GetRentals(customerid);
        }

        public SPayment[] GetPayments(int customerid)
        {
            return df.GetPayments(customerid);
        }

        public SPayment[] GetPaymentsByRental(int rentalid)
        {
            return df.GetPaymentsByRental(rentalid);
        }

        public SRental GetRentalByPayment(int id)
        {
            return df.GetRentalByPayment(id);
        }

        public CategorySale[] GetSalesByCategory()
        {
            return df.GetSalesByFilmCategory("");
        }

        public SCustomer[] GetRewardsReport(
                                            Nullable<global::System.SByte> min_monthly_purchases,
                                            Nullable<global::System.Decimal> min_dollar_amount_purchased,
                                            int count_rewardees)
        {
            return df.GetRewardsReport(min_monthly_purchases.Value, min_dollar_amount_purchased.Value, count_rewardees);
        }

        public bool Logon(string user, string password, ref int userID)
        {
            SecurityModule sm = new SecurityModule();
            return sm.Logon(user, password, ref userID);
        }


        public SPayment GetPayment(int id)
        {
            return df.GetPayment(id);
        }


        public bool RefundPayment(int paymentid)
        {
            return df.RefundPayment(paymentid);
        }
    }
}