using System;

using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using HiLToysDataModel;
using HiLToysWebApplication.Models;

namespace HiLToysWebApplication.HiLToysDataAccessServices
{


    public class CustomerDataAccessService
    {
        private IApplicationDbContext storeDB;
        public CustomerDataAccessService()
        {
            storeDB = new HiLToysApplicationDbContext();
        }
       /* public CustomerDataAccessService(IApplicationDbContext dbContext)
        {
            storeDB = dbContext;
       }*/
        public int GetCustomerIdNumber(string username)
        {
            HiLToysDataModel.Models.Customer customer = new HiLToysDataModel.Models.Customer();
            customer = storeDB.Customers.Single(
                       susername => susername.Username == username);
            return customer.CustomerID;
        }
        public HiLToysDataModel.Models.Customer GetCustomerInformation(int customerID)
         {
            // HiLToysDataModel.Customer customer = new HiLToysDataModel.Customer();
       //   HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
          customerID = 1;
           // storeDB.Configuration.ProxyCreationEnabled = false;
            //List<HiLToysDataModel.Cart> carts = new List<HiLToysDataModel.Cart>();
           // customer = storeDB.Customers.Single(custom => custom.CustomerID == customerID);
          return storeDB.Customers.Find(customerID); 
            }
        public void MigrateUser(string Email, string FirstName, string LastName)
        {
          var  customer = new HiLToysDataModel.Models.Customer()
            {
                Username = Email,
                FirstName = FirstName,
                LastName = LastName,
                Mail = Email
               
            };

          storeDB.Customers.Add(customer);
          storeDB.SaveChanges();
        }
    }
}