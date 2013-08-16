using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;
using System.Data.Objects;
using SimulationManager;

namespace VideoRentalService
{
    public class SecurityModule
    {
        public bool Logon(string user, string password, ref int userID)
        {
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from users in dc.staffs
                            where users.first_name.ToUpper() == user.ToUpper() && users.last_name.ToUpper() == password.ToUpper()
                            select users;
                //check the password

                if (model.Count() == 1)
                {
                    userID = Convert.ToInt32(model.ToArray()[0].staff_id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public abstract class DataFacade
    {
        SimulationManager.Simulator simulator = new Simulator();

        private static readonly ILog log = LogManager.GetLogger(typeof(RentalService));

        public abstract bool PayForMovie(byte staffid, byte customerid, byte rentalid, double paymentAmount);


        public abstract CategorySale[] GetSalesByFilmCategory(string category);


        public abstract SCustomer[] GetCustomersByText(string searchtext);
        public abstract SCustomer GetCustomer(int id);

        public abstract SCustomer[] GetCustomers();
        public abstract SRental[] GetRentals(int customerid);

        public abstract SRental GetRentalByPayment(int id);
        public abstract SPayment[] GetPayments(int customerid);
        public abstract int GetPaymentsByRentalCnt(int rentalid);

        public abstract SPayment[] GetPaymentsByRental(int rentalid);

        public abstract SCustomer[] GetRewardsReport(
                                            Nullable<global::System.SByte> min_monthly_purchases,
                                            Nullable<global::System.Decimal> min_dollar_amount_purchased,
                                            int count_rewardees);

        public abstract SPayment GetPayment(int id);

        public abstract bool RefundPayment(int paymentid);

    }
    public class DataFacade1 : DataFacade
    {
        private SimulationManager.Simulator simulator = new Simulator();

        private static readonly ILog log = LogManager.GetLogger(typeof(RentalService));

        public override bool PayForMovie(byte staffid, byte customerid, byte rentalid, double paymentAmount)
        {
            payment payment = new payment();
            payment.amount = (decimal)paymentAmount;
            payment.customer_id = customerid;
            payment.staff_id = staffid;
            payment.rental_id = rentalid;

            using (SakilaEntities dc = new SakilaEntities())
            {
                simulator.PerformanceSimulation();
                dc.AddTopayments(payment);
                dc.SaveChanges();
            }
            return true;
        }

        public override CategorySale[] GetSalesByFilmCategory(string category)
        {
            List<CategorySale> itemlist = new List<CategorySale>();
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from c in dc.sales_by_film_category
                            where c.category.Contains(category)
                            orderby c.category
                            select c;

                List<sales_by_film_category> sales = model.ToList();
                for (int i = 0; i < sales.Count; i++)
                {
                    CategorySale cs = new CategorySale();
                    if (sales[i].total_sales.HasValue)
                    {
                        cs = cs.Create(sales[i].category, sales[i].total_sales.Value);
                    }
                    else
                    {
                        cs = cs.Create(sales[i].category);
                    }
                    itemlist.Add(cs);
                }
            }
            return itemlist.ToArray<CategorySale>();
        }

        public override SCustomer[] GetCustomersByText(string searchtext)
        {
            List<SCustomer> scustomerlist = new List<SCustomer>();

            simulator.PerformanceSimulation();

            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from c in dc.customers
                            where c.first_name.Contains(searchtext) || c.last_name.Contains(searchtext)
                            orderby (c.first_name)
                            select c;
                List<customer> contactList = model.ToList<customer>();
                for (int i = 0; i < contactList.Count; i++)
                {
                    SCustomer scustomer = new SCustomer();
                    scustomer = scustomer.Createcustomer(contactList[i].customer_id, contactList[i].store_id, contactList[i].first_name, contactList[i].last_name, contactList[i].address_id, contactList[i].active, contactList[i].create_date, contactList[i].last_update, contactList[i].email);
                    scustomerlist.Add(scustomer);
                }
            }
            return scustomerlist.ToArray<SCustomer>();
        }

        public override SCustomer GetCustomer(int id)
        {
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from c in dc.customers
                            where c.customer_id == id
                            orderby (c.first_name)
                            select c;
                List<customer> contactList = model.ToList<customer>();
                if (contactList.Count > 0)
                {
                    SCustomer scustomer = new SCustomer();
                    simulator.PerformanceSimulation();
                    scustomer = scustomer.Createcustomer(contactList[0].customer_id, contactList[0].store_id, contactList[0].first_name, contactList[0].last_name, contactList[0].address_id, contactList[0].active, contactList[0].create_date, contactList[0].last_update, contactList[0].email);
                    return scustomer;
                }
            }
            throw new Exception("no data found");
        }

        public override SCustomer[] GetCustomers()
        {
            List<SCustomer> scustomerlist = new List<SCustomer>();

            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from c in dc.customers
                            orderby (c.first_name)
                            select c;
                List<customer> contactList = model.ToList<customer>();

                simulator.PerformanceSimulation();

                for (int i = 0; i < contactList.Count; i++)
                {
                    SCustomer scustomer = new SCustomer();
                    scustomer = scustomer.Createcustomer(contactList[i].customer_id, contactList[i].store_id, contactList[i].first_name, contactList[i].last_name, contactList[i].address_id, contactList[i].active, contactList[i].create_date, contactList[i].last_update, contactList[i].email);
                    scustomerlist.Add(scustomer);
                }
            }
            return scustomerlist.ToArray<SCustomer>();
        }

        public override SRental[] GetRentals(int customerid)
        {
            List<SRental> srentlist = new List<SRental>();
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from r in dc.rentals
                            where r.customer_id == customerid
                            orderby (r.return_date)
                            select r;

                List<rental> rentalList = model.ToList<rental>();

                simulator.PerformanceSimulation();

                for (int i = 0; i < rentalList.Count; i++)
                {
                    SRental rental = new SRental();
                    if (rentalList[i].return_date.HasValue)
                    {
                        rental = rental.Createrental(rentalList[i].rental_id, rentalList[i].rental_date, rentalList[i].return_date, rentalList[i].inventory_id, rentalList[i].customer_id, rentalList[i].staff_id, rentalList[i].last_update);
                    }
                    else
                    {
                        rental = rental.Createrental(rentalList[i].rental_id, rentalList[i].rental_date, rentalList[i].inventory_id, rentalList[i].customer_id, rentalList[i].staff_id, rentalList[i].last_update);
                    }
                    rental.Filmtitle = rentalList[i].inventory.film.title;
                    srentlist.Add(rental);
                }
            }
            return srentlist.ToArray<SRental>();
        }


        public override SRental GetRentalByPayment(int id)
        {
            //a deliberate inefficient query

            List<SRental> srentlist = new List<SRental>();
            SRental lightweightrental = new SRental();

            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from r in dc.payments
                            where r.payment_id == id
                            select r;

                List<payment> pList = model.ToList<payment>();
                payment p = pList[0];

                var rentalModel = from r in dc.rentals
                                  where r.rental_id == p.rental_id
                                  orderby (r.return_date)  //this is not neccessary but put in here for analysis (SQL end)
                                  select r;

                List<rental> rentalList = rentalModel.ToList<rental>();

                simulator.PerformanceSimulation();

                rental hwrental = rentalList[0];
                if (hwrental.return_date.HasValue)
                {
                    lightweightrental = lightweightrental.Createrental(hwrental.rental_id, hwrental.rental_date, hwrental.return_date, hwrental.inventory_id, hwrental.customer_id, hwrental.staff_id, hwrental.last_update);
                }
                else
                {
                    lightweightrental = lightweightrental.Createrental(hwrental.rental_id, hwrental.rental_date, hwrental.inventory_id, hwrental.customer_id, hwrental.staff_id, hwrental.last_update);
                }
                lightweightrental.Filmtitle = hwrental.inventory.film.title;
            }
            return lightweightrental;
        }

        public override SPayment[] GetPayments(int customerid)
        {
            List<SPayment> spayments = new List<SPayment>();
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from r in dc.payments
                            where r.customer_id == customerid
                            orderby (r.payment_date)
                            select r;
                List<payment> payments = model.ToList<payment>();

                simulator.PerformanceSimulation();

                for (int i = 0; i < payments.Count; i++)
                {
                    SPayment payment = new SPayment();
                    payment = payment.Createpayment((payments[i].rental_id ?? 0), payments[i].payment_id, payments[i].customer_id, payments[i].staff_id, payments[i].amount, payments[i].payment_date, payments[i].last_update, payments[i].rental.inventory_id, payments[i].rental.inventory.film.title);
                    spayments.Add(payment);
                }
            }
            return spayments.ToArray<SPayment>();
        }

        public override int GetPaymentsByRentalCnt(int rentalid)
        {
            List<SPayment> spayments = new List<SPayment>();
            int res = 0;
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from p in dc.payments
                            where p.rental_id == rentalid
                            //orderby (p.payment_date)
                            select p;

                res = model.Count();
            }

            simulator.PerformanceSimulation();

            return res;
        }

        public override SPayment[] GetPaymentsByRental(int rentalid)
        {
            List<SPayment> spayments = new List<SPayment>();
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from p in dc.payments
                            where p.rental_id == rentalid
                            //orderby (p.payment_date)
                            select p;
                List<payment> payments = model.ToList<payment>();

                simulator.PerformanceSimulation();

                for (int i = 0; i < payments.Count; i++)
                {
                    SPayment payment = new SPayment();
                    payment = payment.Createpayment((payments[i].rental_id ?? 0), payments[i].payment_id, payments[i].customer_id, payments[i].staff_id, payments[i].amount, payments[i].payment_date, payments[i].last_update, payments[i].rental.inventory_id, payments[i].rental.inventory.film.title);
                    spayments.Add(payment);
                }
            }
            return spayments.ToArray<SPayment>();
        }

        public override SCustomer[] GetRewardsReport(
                                            Nullable<global::System.SByte> min_monthly_purchases,
                                            Nullable<global::System.Decimal> min_dollar_amount_purchased,
                                            int count_rewardees)
        {
            List<SCustomer> scustomerList = new List<SCustomer>();
            using (SakilaEntities entitities = new SakilaEntities())
            {
                ObjectParameter cntRewardees = new ObjectParameter("count_rewardees", count_rewardees);
                List<customer> contactList = entitities.GetRewardsReport(min_monthly_purchases.Value, min_dollar_amount_purchased.Value, cntRewardees).ToList<customer>();
                count_rewardees = (int)cntRewardees.Value;

                simulator.PerformanceSimulation();

                for (int i = 0; i < contactList.Count; i++)
                {
                    SCustomer scustomer = new SCustomer();
                    scustomer = scustomer.Createcustomer(
                                                        contactList[i].customer_id,
                                                        contactList[i].store_id,
                                                        contactList[i].first_name,
                                                        contactList[i].last_name,
                                                        contactList[i].address_id,
                                                        contactList[i].active,
                                                        contactList[i].create_date,
                                                        contactList[i].last_update,
                                                        contactList[i].email
                                                        );
                    scustomerList.Add(scustomer);
                }
            }
            return scustomerList.ToArray();
        }

        public override SPayment GetPayment(int id)
        {
            SPayment spayment = new SPayment();
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from p in dc.payments
                            where p.payment_id == id
                            //orderby (p.payment_date)
                            select p;
                List<payment> payments = model.ToList<payment>();
                spayment = spayment.Createpayment((payments[0].rental_id ?? 0), payments[0].payment_id, payments[0].customer_id,
                    payments[0].staff_id, payments[0].amount, payments[0].payment_date, payments[0].last_update,
                    payments[0].rental.inventory_id, payments[0].rental.inventory.film.title);
            }

            simulator.PerformanceSimulation();

            return spayment;
        }

        public override bool RefundPayment(int paymentid)
        {
            bool result = false;

            using (SakilaEntities dc = new SakilaEntities())
            {

                SPayment spayment = new SPayment();


                var model = from p in dc.payments
                            where p.payment_id == paymentid
                            //orderby (p.payment_date)
                            select p;
                List<payment> payments = model.ToList<payment>();

                simulator.PerformanceSimulation();

                payment refundpayment = new payment();
                refundpayment.amount = payments[0].amount * -1;
                refundpayment.customer_id = payments[0].customer_id;
                refundpayment.customerReference = payments[0].customerReference;
                refundpayment.rental_id = payments[0].rental_id;
                refundpayment.rentalReference = payments[0].rentalReference;
                refundpayment.payment_date = DateTime.Now;
                refundpayment.staff_id = payments[0].staff_id;
                refundpayment.staffReference = payments[0].staffReference;

                dc.AddTopayments(refundpayment);
                dc.AcceptAllChanges();
                dc.SaveChanges();
            }
            result = true;

            return result;
        }
    }

    //slow
    public class DataFacade2: DataFacade1
    {
        private SimulationManager.Simulator simulator = new Simulator();

        public override SCustomer[] GetCustomersByText(string searchtext)
        {
            List<SCustomer> scustomerlist = new List<SCustomer>();
            using (SakilaEntities dc = new SakilaEntities())
            {
                var model = from c in dc.customers
                            where c.first_name.Contains(searchtext) || c.last_name.Contains(searchtext)
                            orderby (c.first_name)
                            select c;
                List<customer> contactList = model.ToList<customer>();
                foreach  (customer cust in contactList)
                {
                    //high freq sql calls again and again to reconstruct object, terrible pattern
                    //this should increase the sql calls by a magnitude of 2 orders (in a production environment)
                    var mymodel = from c in dc.customers
                                where c.customer_id == cust.customer_id
                                select c;
                    List<customer> slowcustomerlist = mymodel.ToList<customer>();
                    customer locCust = slowcustomerlist[0];
                    SCustomer scustomer = new SCustomer();
                    scustomer = scustomer.Createcustomer(
                        locCust.customer_id, 
                        locCust.store_id, 
                        locCust.first_name, 
                        locCust.last_name, 
                        locCust.address_id,
                        locCust.active, 
                        locCust.create_date, 
                        locCust.last_update, 
                        locCust.email);
                    scustomerlist.Add(scustomer);
                }
            }
            simulator.PerformanceSimulation();
            return scustomerlist.ToArray<SCustomer>();
        }
    }
}