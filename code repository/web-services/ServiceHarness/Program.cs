using AppD.MerchantBank;
using CreditCardNumberGenerator;
using MySql.Data.MySqlClient;
using ServiceHarness.RentalService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ServiceHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a thread for cpu maxing

            
            CardService cs = new CardService();
            string t = RandomCreditCardNumberGenerator.GenerateMasterCardNumber();
            Console.WriteLine(t);
            Console.WriteLine(cs.IsCardNumberValid(t));
            Console.WriteLine(cs.IsValid(t));
            Console.Read();
            //while (true)
            //{
            //    int A = 1 + 1;
            //    //Thread.Sleep(1);
            //}
        }
    }

    public class Worker
    {
        // This method will be called when the thread is started.
        public void DoWork()
        {
            
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
    }
}