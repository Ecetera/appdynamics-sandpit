using CreditCardNumberGenerator;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RefundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class RefundService : IRefundService
    {
        Random rnd = new Random();
        public int GetStaffID(string loginName)
        {
            int id = 0;
            using (MySqlConnection connection = new MySqlConnection(GetConnection()))
            {
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT staff_id FROM sakila.staff where first_name = '{0}'", loginName);
                try
                {
                    connection.Open();
                    MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (myReader.Read())
                    {
                        id = myReader.GetInt32(0);
                        break;
                    }
                    myReader.Close();
                }
                finally
                {
                    connection.Close();
                }
            }
            return id;
        }

        public string RefundPayment(int paymentid, int customer_id, int staff_id, int rental_id, decimal amount, DateTime paymentDate, string movie_title)
        {
            //string conn = "server=localhost; userid=root;password=ecetera;database=sakila;";
            //Use the 'client' variable to call operations on the service.
            string info = "";

            
            CardConsumer cc = new CardConsumer();
            //here just use a randon credti card since we are not fully simulating a bank and accounts etc
            string  ccnumber = RandomCreditCardNumberGenerator.GenerateMasterCardNumber();
            

            //apply credit via the bank service
            cc.Credit(ccnumber,double.Parse(amount.ToString()));


            //account this refund in sakila database payment table

            using (MySqlConnection connection = new MySqlConnection(GetConnection()))
            {
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@staff_id", staff_id);
                cmd.Parameters.AddWithValue("@rental_id", rental_id);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@payment_date", paymentDate);
                cmd.Parameters.AddWithValue("@lastupdate", DateTime.Now);

                cmd.CommandText = "INSERT INTO payment(customer_id,staff_id,rental_id,amount,payment_date,last_update) VALUES" +
                    "(@customer_id, @staff_id, @rental_id, @amount, @payment_date, @lastupdate);";
                try
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result == -1) info = "error in refunding amount";
                    else info = "success";
                }
                finally
                {
                    connection.Close();
                    
                }
            }
            return info;
        }

       

        private string GetConnection()
        {
            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/RefundService");
            System.Configuration.ConnectionStringSettings connString;
            connString = rootWebConfig.ConnectionStrings.ConnectionStrings["SakilaContext"];
            return connString.ConnectionString;
        }
    }
}
