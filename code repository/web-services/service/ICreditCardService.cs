//  Copyright (c) Microsoft Corporation.  All Rights Reserved.
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace Ecetera.AppDynamics.Service
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
}