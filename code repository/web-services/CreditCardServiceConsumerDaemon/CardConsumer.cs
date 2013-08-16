using CreditCardNumberGenerator;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardServiceConsumerDaemon
{
    public class CardConsumer
    {
        int CARD_LIST_SIZE = 20;
        string interfaceContractName = "ICreditCardService";
        
        AppDMoviesWSAutoDiscovery disco;
        private static readonly ILog log = LogManager.GetLogger(typeof(CardConsumer));


        public void Start(string appDynamicsBankIP, string port, int timerange1, int timerange2)
        {
            string bindingURI = string.Format("http://{0}:{1}/AppDynamicsBank/CreditCardService?wsdl", appDynamicsBankIP, port);
            disco = new AppDMoviesWSAutoDiscovery(bindingURI, interfaceContractName);
            Console.WriteLine(string.Format("Partial auto discovery done, basic http binding done:{0}", bindingURI));
            //sometimes duplicates are generated,but for testing this is not an issue
            List<string> ccnumbers = RandomCreditCardNumberGenerator.GenerateMasterCardNumbers(CARD_LIST_SIZE).ToList();
            int next = 0;
            Random rnd = new Random();

            while (true)
            {
                double amount = GetRandomAmount();
                string creditcard = ccnumbers[next++];
                //write a method to generate different credit cards
                if (next >= CARD_LIST_SIZE) next = 0;//this is just to recycle the crecit card numbers for the purpose of this test
                ValidateAndChargeCard(creditcard, amount);
                int interval = rnd.Next(timerange1 * 1000, timerange2 * 1000);
                System.Threading.Thread.Sleep(interval);
            }
        }

        private void ValidateAndChargeCard(string creditcard, double amount)
        {
            object[] operationParameters = new object[] { creditcard, "1212", "123", amount};
            Console.WriteLine(string.Format("Authorisation request sent for {0}-{1}-{2}-${3}::", operationParameters[0], operationParameters[1], operationParameters[2], operationParameters[3]));
            string operationName = "AuthoriseAndCharge";
            Object result = disco.InvokeMethod(operationName, operationParameters);
            Console.WriteLine(result);
        }

        private double GetRandomAmount()
        {
            Random rnd = new Random();
            if (rnd.NextDouble() < 0.2)
                return Math.Round(rnd.NextDouble() * -10.0, 2);
            else
                return Math.Round(rnd.NextDouble() * 10.0, 2);
        }
    }
}