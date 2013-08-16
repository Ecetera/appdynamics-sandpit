using CreditCardNumberGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardServiceConsumerDaemon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            string port,host;
            int timerange1, timerange2;
            if (args.Length != 4)
            {
                Console.WriteLine("<usage:> [IP] [port] [timerange1seconds] [timerange2seconds]--sample 10.0.2.1 9090 60 90");
                Console.Read();
                return;
            }
            try
            {
                timerange1 = int.Parse(args[2]);
                timerange2 = int.Parse(args[3]);
            }
            catch
            {
                Console.WriteLine("timerange must be an integer");
                Console.WriteLine("<usage:> [IP] [port] [timerange1seconds] [timerange2seconds]--sample 10.0.2.1 9090 60 90");
                Console.Read();
                return;
            }

            host = args[0];
            port = args[1];
            

            CardConsumer cardconsumer = new CardConsumer();
            Console.WriteLine("About to start cc authorise service consumer");
            cardconsumer.Start(host,port, timerange1, timerange2);
        }
    }
}