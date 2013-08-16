using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundService
{
    public class CardConsumer
    {
        string interfaceContractName = "ICreditCardService";
        private static readonly ILog log = LogManager.GetLogger(typeof(CardConsumer));
        private AppDMoviesWSAutoDiscovery disco;

        public CardConsumer()
        {
            //here do the actual refund
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("MerchantBankServiceIPAndPort", typeof(String));
            //class name to load
            string line;

            // Read the file and display it line by line.
            using (System.IO.StreamReader file = new System.IO.StreamReader(path))
            {
                line = file.ReadToEnd();
                file.Close();
            }

            string[] connectionInfo = line.Split(";".ToCharArray());
            string ip = connectionInfo[0];
            string port = connectionInfo[1];

            string bindingURI = string.Format("http://{0}:{1}/AppDynamicsBank/CreditCardService?wsdl", ip, port);
            disco = new AppDMoviesWSAutoDiscovery(bindingURI, interfaceContractName);
        }

        public void Credit(string creditcardno, double amount)
        {
            object[] operationParameters = new object[] { creditcardno, amount };
            Object result = disco.InvokeMethod("Credit", operationParameters);

        }

        public void Debit(string creditcardno, double amount)
        {
            object[] operationParameters = new object[] {creditcardno, amount };
            Object result = disco.InvokeMethod("Debit", operationParameters);
        }
    }
}