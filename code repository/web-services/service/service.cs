//  Copyright (c) Microsoft Corporation.  All Rights Reserved.
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Net.Sockets;
using log4net;
using System.ServiceModel.Discovery;
using System.ServiceModel.Channels;
using System.IO;

namespace Ecetera.AppDynamics.IISService
{
    // Define a service contract.
    [ServiceContract(Namespace = "http://Ecetera.AppDynamics.Service")]

    public interface ICreditCardService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creditcard"></param>
        /// <param name="expiry"></param>
        /// <param name="cvv"></param>
        /// <param name="amount">positive amount represents a debit (or a charge to the credit card), a negative amount means a refund to the card
        /// </param>
        /// <returns></returns>
        [OperationContract(Name = "AuthoriseAndCharge")]
        string Authorise(string creditcard, string expiry, string cvv, double amount);

        [OperationContract(Name = "Authorise")]
        string Authorise(string creditcard, string expiry, string cvv);

        [OperationContract]
        int Credit(string creditcard, double amount);

        [OperationContract]
        int Debit(string creditcard, double amount);
    }

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="creditcard"></param>
        /// <param name="expiry"></param>
        /// <param name="cvv"></param>
        /// <param name="amount"></param>
        /// <returns>this will return {0}|{1} as a string e.g "authorised|2023"
        /// where authorised means it was authorised and 2023 means it was the transaction number
        /// special case notauthorised|-1 ---> means it was not authorisd
        /// special case authorised|-1  -----> authirised but debit or credit failed
        /// </returns>
        public string Authorise(string creditcard, string expiry, string cvv, double amount)
        {
            string isAuthorised = Authorise(creditcard, expiry, cvv);
            int creditcardoperationresult = -1;

            if (isAuthorised == "authorised")
            {
                if (amount > 0)
                {
                    creditcardoperationresult = Debit(creditcard, amount);
                }
                else
                {
                    creditcardoperationresult = Credit(creditcard, amount * -1.0);
                }
            }
            return string.Format("{0}|{1}", isAuthorised, creditcardoperationresult);
        }

        //noticetype : REFUND_NOTICE, BILLING _NOTICE
        private void SendMessage(string message, string type)
        {
        }
        public int Credit(string creditcard, double amount)
        {
            int result;

            log.Info((string.Format("Credit request for {0}-{1}::", creditcard, amount)));

            AppD.MerchantBank.CardService cardService = new AppD.MerchantBank.CardService();

            result = cardService.Credit(creditcard, amount);

            log.Info(string.Format("result: {0}", result));

            //send a message here (exit point as per req)

            SendMessage("a refund message", "REFUND_NOTICE");

            SumOfRefunds(amount);

            return result;
        }

        private void SumOfRefunds(double amount)
        {
            
        }

        public int Debit(string creditcard, double amount)
        {
            int result;

            log.Info((string.Format("Credit request for {0}-{1}::", creditcard, amount)));

            AppD.MerchantBank.CardService cardService = new AppD.MerchantBank.CardService();

            result = cardService.Debit(creditcard, amount);

            log.Info(string.Format("result: {0}", result));
            
            SendMessage("a BILLING message", "BILLING_NOTICE");

            return result;
        }

        public static void MyOwnLog(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
        // Host the service within this EXE console application.
        public static void Main(string[] args)
        {
            
            string port;            

            if (args.Length != 1)
            {
                Console.WriteLine("usage:service [port]");
                return;
            }

            port = args[0];

            /*
            string localIP = "?";
            string hostname = Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(hostname);
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            */
            string serviceAddress = string.Format("http://{0}:{1}/AppDynamicsBank/CreditCardService", "127.0.0.1", port);

            // Create a ServiceHost for the CalculatorService type
            //using (ServiceHost serviceHost = new ServiceHost(typeof(CalculatorService), new Uri("http://localhost:8000/ServiceModelSamples/service"), new Uri("net.tcp://localhost:9000/servicemodelsamples/service")))
            using (ServiceHost serviceHost = new ServiceHost(typeof(CreditCardService), new Uri(serviceAddress)))
            {

                Binding httpBinding = new BasicHttpBinding();

                serviceHost.AddServiceEndpoint(typeof(ICreditCardService), httpBinding, serviceAddress);
                ServiceMetadataBehavior metadataBehavior;
                metadataBehavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (metadataBehavior == null)
                {
                    metadataBehavior = new ServiceMetadataBehavior();
                    metadataBehavior.HttpGetEnabled = true;
                    serviceHost.Description.Behaviors.Add(metadataBehavior);
                }
                serviceHost.Open();

                // Enumerate endpoints
                Console.WriteLine("Service endpoints:");
                ServiceDescription desc = serviceHost.Description;
                foreach (ServiceEndpoint endpoint in desc.Endpoints)
                {
                    Console.WriteLine("Endpoint - address:  {0}", endpoint.Address);
                    Console.WriteLine("           binding:  {0}", endpoint.Binding.Name);
                    Console.WriteLine("           contract: {0}", endpoint.Contract.Name);
                }

                Console.WriteLine();

                // The service can now be accessed.
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.Read();
            }
        }

    }
}