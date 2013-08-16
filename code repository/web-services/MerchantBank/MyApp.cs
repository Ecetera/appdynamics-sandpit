using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace MerchantBank
{
    public class MyApp
    {
        // Define a static logger variable so that it references the
        // Logger instance named "MyApp".
        private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

        static void Main(string[] args)
        {
            string creditcard = "4444333322221111";
            // Set up a simple configuration that logs on the console.
            //BasicConfigurator.Configure();
            AppD.MerchantBank.CardService cardService = new AppD.MerchantBank.CardService();
            if (cardService.IsValid(creditcard))
            {
                cardService.Debit(creditcard, 50);
                cardService.Credit(creditcard, 33);
            }
            Console.Read();
        }
    }
}