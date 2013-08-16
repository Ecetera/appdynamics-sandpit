using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace VideoRentalService
{
    public class SPayment
    {
        #region Factory Method

        public SPayment Createpayment(global::System.Int32 rental_id, global::System.Int32 payment_id, global::System.Int32 customer_id, global::System.Byte staff_id, global::System.Decimal amount, global::System.DateTime payment_date, global::System.DateTime last_update,global::System.Int32 inventory_id, global::System.String filmtitle)
        {
            SPayment payment = new SPayment();
            payment.payment_id = payment_id;
            payment.rental_id = rental_id;
            payment.customer_id = customer_id;
            payment.staff_id = staff_id;
            payment.amount = amount;
            payment.payment_date = payment_date;
            payment.last_update = last_update;
            payment.inventory_id = inventory_id;
            payment.filmtitle = filmtitle;

            return payment;
        }

        #endregion
        public global::System.Int32 payment_id;
        public global::System.Int32 customer_id;
        public global::System.Byte staff_id;
        public Nullable<global::System.Int32> rental_id;
        public global::System.Decimal amount;
        public global::System.DateTime payment_date;
        public global::System.DateTime last_update;
        public global::System.Int32 inventory_id;
        public global::System.String filmtitle;
    }

    public class CategorySale
    {
        #region Factory Method
        /// <summary>
        /// Create a new sales_by_film_category object.
        /// </summary>
        /// <param name="category">Initial value of the category property.</param>
        public CategorySale Create(string category, Decimal total_sales)
        {
            CategorySale cs= new  CategorySale();
            cs.category = category;
            cs.total_sales = total_sales;
            return cs;
        }

        public CategorySale Create(string category)
        {
            CategorySale cs = new CategorySale();
            cs.category = category;
            return cs;
        }

        #endregion

        #region Primitive Properties

        private global::System.String _category;
        public global::System.String category
        {
            get {return _category; }
            set {_category = value;}
        }
        
        private Nullable<global::System.Decimal> _total_sales;
        public Nullable<global::System.Decimal> total_sales
        {
            get{return _total_sales;}
            set{_total_sales = value;}
        }
        #endregion
    }
    
    public class SCustomer
    {
        #region Factory Method
        public SCustomer Createcustomer(global::System.Int32 customer_id, global::System.Byte store_id, global::System.String first_name, global::System.String last_name, global::System.Int32 address_id, global::System.Boolean active, global::System.DateTime create_date, global::System.DateTime last_update, string email)
        {
            SCustomer customer = new SCustomer();
            customer.customer_id = customer_id;
            customer.store_id = store_id;
            customer.first_name = first_name;
            customer.last_name = last_name;
            customer.address_id = address_id;
            customer.active = active;
            customer.create_date = create_date;
            customer.last_update = last_update;
            customer.email = email;
            return customer;
        }
        #endregion
        public global::System.Int32 customer_id;
        public global::System.Byte store_id;
        public global::System.String first_name;
        public global::System.String last_name;
        public global::System.String email;
        public global::System.Int32 address_id;
        public global::System.Boolean active;
        public global::System.DateTime create_date;
        public global::System.DateTime last_update;
    }






    public class SRental
    {
        #region Factory Method

        public SRental Createrental(global::System.Int32 rental_id, global::System.DateTime rental_date, global::System.Int32 inventory_id, global::System.Int32 customer_id, global::System.Byte staff_id, global::System.DateTime last_update)
        {
            SRental rental = new SRental();
            rental.rental_id = rental_id;
            rental.rental_date = rental_date;
            rental.inventory_id = inventory_id;
            rental.customer_id = customer_id;
            rental.staff_id = staff_id;
            rental.last_update = last_update;
            return rental;
        }

        public SRental Createrental(global::System.Int32 rental_id, global::System.DateTime rental_date, Nullable<global::System.DateTime> return_date, global::System.Int32 inventory_id, global::System.Int32 customer_id, global::System.Byte staff_id, global::System.DateTime last_update)
        {
            SRental rental = new SRental();
            rental.rental_id = rental_id;
            rental.rental_date = rental_date;
            rental.return_date = return_date;
            rental.inventory_id = inventory_id;
            rental.customer_id = customer_id;
            rental.staff_id = staff_id;
            rental.last_update = last_update;
            return rental;
        }

        #endregion

        private string _filmTitle; public string Filmtitle { get; set; }

        public global::System.Int32 rental_id;
        public global::System.DateTime rental_date;
        public global::System.Int32 inventory_id;
        public global::System.Int32 customer_id;
        public Nullable<global::System.DateTime> return_date;
        public global::System.Byte staff_id;
        public global::System.DateTime last_update;

    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRentalService
    {
        [OperationContract]
        int HireMovie(int staffid, int customerid, int rentalid, double paymentAmount, string creditcardno, string expiry, string cvv);

        [OperationContract]
        int Refund(int staffid, int customerid, double paymentAmount, string creditcardno, string expiry, string cvv);
        
        [OperationContract]
        int RefundT(int staffid, int customerid, double paymentAmount, string creditcardno, string expiry, string cvv);

        [OperationContract]
        SCustomer[] GetCustomersByText(string searchtext);

        [OperationContract]
        SCustomer GetCustomer(int id);

        [OperationContract]
        SCustomer[] GetCustomers();

        [OperationContract]
        SRental[] GetRentals(int customerid);

        [OperationContract]
        SPayment[] GetPayments(int customerid);

        [OperationContract]
        SPayment[] GetPaymentsByRental(int rentalid);

        
        [OperationContract]
        SRental GetRentalByPayment(int id);

        [OperationContract]
        SPayment GetPayment(int id);

        [OperationContract]
        CategorySale[] GetSalesByCategory();

        [OperationContract]
        SCustomer[] GetRewardsReport(
                                            Nullable<global::System.SByte> min_monthly_purchases,
                                            Nullable<global::System.Decimal> min_dollar_amount_purchased,
                                            int count_rewardees);
        [OperationContract]
        bool Logon(string user, string password, ref int userID);

        [OperationContract]
        int ServiceCheck();

        [OperationContract]
        bool RefundPayment(int paymentid);
    }
}
