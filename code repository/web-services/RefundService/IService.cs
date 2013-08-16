using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RefundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRefundService
    {
        [OperationContract]
        string RefundPayment(int paymentid, int customer_id, int staff_id, int rental_id, decimal amount, DateTime paymentDate, string movie_title);

        [OperationContract]
        int GetStaffID(string loginName);
    }
}
