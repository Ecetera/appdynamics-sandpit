using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;

namespace Ecetera.AppDynamics.IISService
{
    // Service class which implements the service contract.
    // Added code to write output to the console window
    public class CreditCardService : ICreditCardService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CreditCardService));

        public string Authorise(string creditcard, string expiry, string cvv)
        {
            string result;
            log.Info(string.Format("Authorisation request for {0}-{1}-{2}::", creditcard, expiry, cvv));
            AppD.MerchantBank.CardService cardService = new AppD.MerchantBank.CardService();
            if (cardService.IsValid(creditcard))
                result = "authorised";
            else
                result = "notauthorised";
            log.Info(string.Format("result: {0}", result));
            return result;
        }

        public int Credit(string creditcard, double amount)
        {
            int result;

            log.Info((string.Format("Credit request for {0}-{1}::", creditcard, amount)));

            AppD.MerchantBank.CardService cardService = new AppD.MerchantBank.CardService();

            result = cardService.Credit(creditcard, amount);

            log.Info(string.Format("result: {0}", result));

            return result;
        }

        public int Debit(string creditcard, double amount)
        {
            int result;

            log.Info((string.Format("Credit request for {0}-{1}::", creditcard, amount)));

            AppD.MerchantBank.CardService cardService = new AppD.MerchantBank.CardService();

            result = cardService.Debit(creditcard, amount);

            log.Info(string.Format("result: {0}", result));

            return result;
        }
    }
}
