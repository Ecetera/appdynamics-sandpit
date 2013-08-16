//  Copyright (c) Microsoft Corporation.  All Rights Reserved.
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace Ecetera.AppDynamics.IISService
{
    // Define a service contract.
    [ServiceContract]
    public interface ICreditCardService
    {
        [OperationContract]
        string Authorise(string creditcard, string expiry, string cvv);

        [OperationContract]
        int Credit(string creditcard, double amount);

        [OperationContract]
        int Debit(string creditcard, double amount);        
    }
}