using CreditCardServiceConsumerDaemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecetera.AppDynamics
{
    public class CCTerminal
    {
        CardConsumer cc;
        public CCTerminal()
        {
            cc = new CardConsumer();
            
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string ip = (string)r.GetValue("CreditCardServiceIP", typeof(String));
            string port = (string)r.GetValue("CreditCardServicePort", typeof(String));
            
            cc.Bind(ip, port);
         
        }
        public void TerminalValidate(string creditcard, string expiry, string cvv, double amount)
        {
            cc.ValidateAndChargeCard(creditcard, expiry, cvv, amount);
        }
    }
}