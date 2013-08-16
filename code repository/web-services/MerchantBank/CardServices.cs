using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Threading;
using System.Configuration;
using System.IO;

namespace AppD.MerchantBank
{
    public class CardService
    {
        // Declaration
        private static int safeInstanceCount = 0;

        private static readonly ILog log = LogManager.GetLogger(typeof(CardService));

        private string credittransactionformatter = "credit, {0},{1},{2}";
        private string debittransactionformatter = "debit , {0},{1},{2}";

        public bool IsValid(string cardNumber)
        {
            //if (!ValidationDisabled())
            //{
            return IsCardNumberValid(cardNumber, false);
            //}
            //else { return true; }
        }

        private bool ValidationDisabled()
        {
            string filename = new AppSettingsReader().GetValue("simulationconfigurationfile", typeof(System.String)).ToString();
            string line;
            string deviceName = string.Empty;
            // Read the file and display it line by line.
            StreamReader file = new
            System.IO.StreamReader(filename);
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith("ValidateCreditCard"))
                    {
                        string[] onoff = line.Split('=');
                        return (onoff[1].ToLower() == "off");
                    }
                }
            }
            return false;
        }

        public bool IsCardNumberValid(string cardNumber, bool allowSpaces = false)
        {
            if (allowSpaces)
            {
                cardNumber = cardNumber.Replace(" ", "");
            }
            if (cardNumber.Any(c => !Char.IsDigit(c)))
            {
                return false;
            }
            int checksum = cardNumber
               .Select((c, i) => (c - '0') << ((cardNumber.Length - i - 1) & 1))
               .Sum(n => n > 9 ? n - 9 : n);
            return (checksum % 10) == 0 && checksum > 0;
        }

        public int Credit(string cardNumber, double amount)
        {
            int id = -1;
            if (IsValid(cardNumber))
            {
                Interlocked.Increment(ref safeInstanceCount);
                id = safeInstanceCount;
                string record = string.Format(credittransactionformatter, id, cardNumber, amount);
                log.Info(record);
            }
            return id;
        }

        public int Debit(string cardNumber, double amount)
        {
            int id = -1;
            if (IsValid(cardNumber))
            {
                Interlocked.Increment(ref safeInstanceCount);
                id = safeInstanceCount;
                string record = string.Format(debittransactionformatter, id, cardNumber, amount);
                log.Info(record);
            }
            return id;
        }
    }
}