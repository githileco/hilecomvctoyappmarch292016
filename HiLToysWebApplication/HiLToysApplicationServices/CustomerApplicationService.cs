using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLToysViewModel;
using HiLToysDataModel;
using HiLToysWebApplication.HiLToysDataAccessServices;
using HiLToysBusinessServices;
using HiLToysWebApplication.Models;

namespace HiLToysWebApplication
{
    public class CustomerApplicationService
    {
        public void MigrateUser(string Email, string FirstName, string LastName)
        {
            CustomerDataAccessService customerDataAccessService = new CustomerDataAccessService();

            customerDataAccessService.MigrateUser(Email, FirstName, LastName);
        }
    }
}